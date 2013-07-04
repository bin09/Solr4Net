﻿using System;
using System.Collections.Generic;
using SolrNet;
using Solr.MODEL;
using Solr.DBUtility;
using Solr.Helper;
using System.Threading;
using Solr.Log;
using Solr.Config;

namespace Solr.Core.Index
{
    public class MemberIndex:BaseIndex
    {
        private SolrHelper<ModMember> solrHelper = SolrHelper<ModMember>.Instance(ECore.MemberCore);
        private ISolrOperations<ModMember> solr = null;
        private DalMember dal = null;

        public EProcessState State { get; set; }
        public Thread CurrentThread { get; set; }
        public LogMsg logMsg = null;

        private int count = 0, value = 0;
        private string solrUrl = string.Empty;

        public MemberIndex(LogMsg logMsg)
        {
            this.logMsg = logMsg;
            dal = new DalMember();
            if (solr == null)
            {
                solr = solrHelper.GetSolrInstance();
            }
            this.count = this.dal.GetCount(Convert.ToInt32(ConfigSetting.GetValue("LastMemberId")));
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
                logMsg.writeLog(0, "会员线程已停止");
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
                //if (this.count == 0)
                //    this.count = this.dalSiteInfo.GetCount(Convert.ToInt32(ConfigSetting.GetValue("LastId")));
                this.count = this.dal.GetCount(Convert.ToInt32(ConfigSetting.GetValue("LastMemberId")));
                this.value = 0;
                this.CreateIndex(isCreate);
            }
            logMsg.writeLog(0, "线程已停止");
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        private void CreateIndex(bool isCreate)
        {
            List<ModMember> list = this.dal.GetList();
            if (list == null)
            {
                this.count = 0;
                this.value = 0;
                if (isCreate)
                {
                    logMsg.writeLog(0, "正在优化索引...........");
                    this.solr.Optimize();
                    this.solr.Commit();
                    logMsg.writeLog(0, "优化完成");
                    this.Stop();
                }
                else
                {
                    logMsg.writeLog(0, string.Concat("暂无数据：休眠：", solrConfig.SleepTime / 1000, "秒"));
                    Thread.Sleep(solrConfig.SleepTime);
                }
            }
            else
                this.CreateIndex(list, isCreate);
        }
        /// <summary>
        /// 创建索引
        /// </summary>
        /// <param name="list"></param>
        private void CreateIndex(List<ModMember> list, bool isCreate)
        {
            if (list != null)
            {
                foreach (var item in list)
                {
                    this.solr.Add(item);
                    if (!isCreate)
                        this.solr.Commit();                                //逐条提交变更                
                    ConfigSetting.SetValue("LastMemberId", item.Id);
                    logMsg.writeBarLog(++value, this.count);
                    logMsg.writeLog(item.Id, string.Concat("索引创建成功：", item.CompanyName));
                }
                if (isCreate)
                    this.solr.Commit();                                   //创建时：批量提交
            }
            else
            {
                this.count = 0;
                this.value = 0;
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
                this.solr.Optimize();
                this.solr.Commit();
                logMsg.writeLog(0, "索引优化完毕，正在开启线程...........");
                this.Start(false);
                logMsg.writeLog(0, "会员线程已开启");
            }
        }
        #endregion
    }
}
