using System;
using System.Collections.Generic;
using Solr.MODEL;
using SolrNet;
using Solr.DBUtility;
using System.Threading;
using Solr.Helper;
using Solr.Log;
using Solr.Config;

namespace Solr.Core.Index
{
    public class UpdateLogIndex : BaseIndex
    {
        private SolrHelper<ModProduct> solrProHelper = SolrHelper<ModProduct>.Instance(ECore.ProCore);
        private ISolrOperations<ModProduct> solrPro = null;
        private SolrHelper<ModMember> solrMemberHelper = SolrHelper<ModMember>.Instance(ECore.MemberCore);
        private ISolrOperations<ModMember> solrMember = null;
        private SolrHelper<ModKeyword> solrKeywordHelper = SolrHelper<ModKeyword>.Instance(ECore.KeywordCore);
        private ISolrOperations<ModKeyword> solrKeyword = null;
        private SolrHelper<ModBuys> solrBuysHelper = SolrHelper<ModBuys>.Instance(ECore.BuyCore);
        private ISolrOperations<ModBuys> solrBuys = null;
        private SolrHelper<ModNews> solrNewsHelper = SolrHelper<ModNews>.Instance(ECore.NewsCore);
        private ISolrOperations<ModNews> solrNews = null;

        private DalUserLog dal = new DalUserLog();
        private DalMember dalMember = new DalMember();
        public EProcessState State { get; set; }
        public Thread CurrentThread { get; set; }
        public LogMsg logMsg = null;

        private string solrUrl = string.Empty;

        public UpdateLogIndex(LogMsg logMsg)
        {
            this.logMsg = logMsg;
            this.InitSolrOperations();
        }
        private void InitSolrOperations()
        {
            if (solrPro == null)
                solrPro = solrProHelper.GetSolrInstance();
            if (solrMember == null)
                solrMember = solrMemberHelper.GetSolrInstance();
            if (solrKeyword == null)
                solrKeyword = solrKeywordHelper.GetSolrInstance();
            if (solrBuys == null)
                solrBuys = solrBuysHelper.GetSolrInstance();
            if (solrNews == null)
                solrNews = solrNewsHelper.GetSolrInstance();
        }

        #region 线程
        public override void Start(bool isCreate)
        {
            State = EProcessState.Run;
            ThreadStart thread = new ThreadStart(() => this.Process(isCreate));
            CurrentThread = new Thread(thread);                       //获取当前线程实例
            CurrentThread.Start();
        }
        public override void Stop()
        {
            State = EProcessState.Stop;
            logMsg.writeLog(0, "正在停止。。。");
            if (this.CurrentThread.ThreadState == ThreadState.WaitSleepJoin)
            {
                this.CurrentThread.Abort();
                logMsg.writeLog(0, "日志线程已停止");
            }
        }
        #endregion

        #region 创建索引
        /// <summary>
        /// 处理
        /// </summary>
        private void Process(bool isCreate)
        {
            while (EProcessState.Run == State)
            {
                this.DeleteIndex();
            }
            logMsg.writeLog(0, "线程已停止");
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        private void DeleteIndex()
        {
            List<ModUserLog> list = this.dal.GetList();
            if (list == null)
            {
                logMsg.writeLog(0, string.Concat("暂无数据：休眠：", solrConfig.SleepTime / 1000, "秒"));
                Thread.Sleep(solrConfig.SleepTime);
            }
            else
                this.DeleteIndex(list);
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="list"></param>
        private void DeleteIndex(List<ModUserLog> list)
        {
            if (list != null)
            {
                foreach (var item in list)
                {
                    this.DeleteIndex(item);
                    this.solrPro.Commit();                                //逐条提交变更                
                    ConfigSetting.SetValue("LastLogId", item.Id);
                    logMsg.writeLog(item.Id, string.Concat("索引删除成功：", item.Id));
                }
            }
        }
        private void DeleteIndex(ModUserLog mod)
        {
            if (mod.Type == Convert.ToByte(UserLogType.proSellsDel) || mod.Type == Convert.ToByte(UserLogType.proBestDel))
            {
                this.solrPro.Delete(mod.TypeProId);
            }
            else if (mod.Type == Convert.ToByte(UserLogType.proSellsEdit) || mod.Type == Convert.ToByte(UserLogType.proBestEdit))
            {
                //修改的信息，修改的信息必须是审核通过的才提交变更
            }
            else if (mod.Type == Convert.ToByte(UserLogType.memberDel))
            {
                this.solrMember.Delete(mod.LinkId.ToString());
            }
            else if (mod.Type == Convert.ToByte(UserLogType.memberEdit))                  //修改，重新替换索引
            {
                ModMember modMember = dalMember.GetModel(mod.LinkId);
                if (modMember != null)
                    this.solrMember.Add(modMember);
            }
            else if (mod.Type == Convert.ToByte(UserLogType.buyDel))
            {
                this.solrBuys.Delete(mod.LinkId.ToString());
            }
            else if (mod.Type == Convert.ToByte(UserLogType.buyEdit))
            {
                //修改同上
            }
            else if (mod.Type == Convert.ToByte(UserLogType.keywordDel))
            {
                this.solrKeyword.Delete(mod.LinkId.ToString());
            }
            else if (mod.Type == Convert.ToByte(UserLogType.newsDel))
            {
                this.solrNews.Delete(mod.LinkId.ToString());
            }
        }
        #endregion

        #region  索引优化
        /// <summary>
        /// 检测线程状态
        /// </summary>
        /// <returns></returns>
        private bool IsStop()
        {
            if (this.CurrentThread.ThreadState == System.Threading.ThreadState.Running)
                return false;
            return true;
        }
        public override void Optimize()
        {
            if (!this.IsStop())
            {
                logMsg.writeLog(0, "线程正在处理索引，请等待线程处理完成");
            }
            else
            {
                logMsg.writeLog(0, "正在停止线程");
                this.Stop();
                logMsg.writeLog(0, "索引优化中...........");
                this.solrPro.Optimize();
                this.solrPro.Commit();
                logMsg.writeLog(0, "索引优化完毕，正在开启线程...........");
                this.Start(false);
                logMsg.writeLog(0, "产品线程已开启");
            }
        }
        #endregion
    }
}
