using System;
using System.Windows.Forms;
using System.IO;
using Solr.Core.Index;
using Solr.Task;
using Solr.Log;

namespace Solr
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private BaseIndex[] baseIndex = null;
        private TaskCalls task = null;
        private LogMsg logPro = null;
        private LogMsg logMember = null;
        private LogMsg logKeyword = null;
        private LogMsg logBuy = null;
        private LogMsg logNews = null;
        private LogMsg logBest = null;
        private LogMsg logLog = null;
        /// <summary>
        /// Start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.btnCreate.Enabled = false;
            this.btnStop.Enabled = true;
            this.CreateIndex(true);
        }
        /// <summary>
        /// Stop
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, EventArgs e)
        {

        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            FileInfo file = new FileInfo(Path.Combine(System.IO.Directory.GetCurrentDirectory(), "FileConfig/Log4.xml"));
            log4net.Config.XmlConfigurator.Configure(file);

            this.btnStop.Enabled = false;
            this.Initializenotifyicon();
            task = TaskCalls.Instance();

            this.InitUI();
        }

        #region 索引
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="isCreate"></param>
        private void CreateIndex(bool isCreate)
        {
            baseIndex = new BaseIndex[] { new BuyIndex(logBuy), new MemberIndex(logMember), new NewsIndex(logNews), new KeywordIndex(logKeyword), new ProIndex(logPro), new ProBestIndex(logBest) };
            this.task.StartTask(isCreate, baseIndex);

            //MemberIndex member = new MemberIndex(logMember);
            //member.Start(true);
            //BuyIndex buy = new BuyIndex(logBuy);
            //buy.Start(true);
            //KeywordIndex keyword = new KeywordIndex(logKeyword);
            //keyword.Start(true);
            //NewsIndex news = new NewsIndex(logNews);
            //news.Start(true);
            //ProIndex pro = new ProIndex(logPro);
            //pro.Start(true);
            //ProBestIndex proBest = new ProBestIndex(logBest);
            //proBest.Start(true);
        }
        #endregion

        #region 窗体操作
        /// <summary>
        /// 初始化
        /// </summary>
        void InitUI()
        {
            logPro = new LogMsg(proView, proBar);
            logMember = new LogMsg(memberView, memberBar);
            logBuy = new LogMsg(buyView, null);
            logKeyword = new LogMsg(keywordView, keywordBar);
            logNews = new LogMsg(newsView, null);
            logBest = new LogMsg(proBestView, null);
            logLog = new LogMsg(logView, null);


            logPro.WriteLog += new LogMsg.LogEvent(p_WriteLog);
            logPro.WriteBarLog += new LogMsg.LogBarEvent(b_WriterLog);
            logMember.WriteLog += new LogMsg.LogEvent(p_WriteLog);
            logMember.WriteBarLog += new LogMsg.LogBarEvent(b_WriterLog);

            logBuy.WriteLog += new LogMsg.LogEvent(p_WriteLog);
            logNews.WriteLog += new LogMsg.LogEvent(p_WriteLog);
            logBest.WriteLog += new LogMsg.LogEvent(p_WriteLog);

            logKeyword.WriteLog += new LogMsg.LogEvent(p_WriteLog);
            logKeyword.WriteBarLog += new LogMsg.LogBarEvent(b_WriterLog);

            logLog.WriteLog += new LogMsg.LogEvent(p_WriteLog);
        }
        private ContextMenu notifyiconMnu;                          //右键菜单
        private void Initializenotifyicon()
        {
            //设定托盘程序的各个属性 
            this.notifyIcon1.Text = "bin--Solr索引创建";
            notifyIcon1.Visible = true;
            notifyIcon1.DoubleClick += new EventHandler(ShowMain_Click);

            //定义一个MenuItem数组，并把此数组同时赋值给ContextMenu对象 
            MenuItem[] mnuItms = new MenuItem[3];
            mnuItms[0] = new MenuItem();
            mnuItms[0].Text = "显示主界面";
            mnuItms[0].Enabled = true;
            mnuItms[0].Click += new EventHandler(ShowMain_Click);

            mnuItms[1] = new MenuItem("-");

            mnuItms[2] = new MenuItem();
            mnuItms[2].Text = "退出系统";
            mnuItms[2].Click += new EventHandler(Exit_Click);
            mnuItms[2].DefaultItem = true;

            notifyiconMnu = new ContextMenu(mnuItms);
            notifyIcon1.ContextMenu = notifyiconMnu;
        }
        private void SolrForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                this.notifyIcon1.Visible = true;
                notifyiconMnu.MenuItems[0].Enabled = true;
            }
        }
        /// <summary>
        /// 显示主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ShowMain_Click(object sender, EventArgs e)
        {
            notifyiconMnu.MenuItems[0].Enabled = true;
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }
        /// <summary>
        /// 关闭系统
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void SolrForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
        }
        private void SolrForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!(MessageBox.Show("请您确认是否退出(Y/N)", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                e.Cancel = true;//阻止退出系统
            }
        }
        #endregion

        #region  日志
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="message"></param>
        /// <param name="dgv"></param>
        void p_WriteLog(int id, string message, DataGridView dgv)
        {
            this.BeginInvoke(new writetLogEvent(writeLog), id, message, dgv);
        }
        private delegate void writetLogEvent(int id, string message, DataGridView dgv);
        /// <summary>
        /// 写入一条请求消息
        /// </summary>
        /// <param name="message">日志消息</param>
        /// <param name="dgv">日志视图</param>
        private void writeLog(int id, string message, DataGridView dgv)
        {
            if (dgv.Rows.Count > 10000)
            {
                dgv.Rows.Clear();
            }
            DataGridViewRow dgvr = new DataGridViewRow();
            if (id > 0)
                dgvr.CreateCells(dgv, id.ToString(), message);
            else
                dgvr.CreateCells(dgv, "Msg", message);
            dgv.Rows.Insert(0, dgvr);
        }
        private void writerBarLog(int value, int count, ProgressBar bar)
        {
            bar.Maximum = count;
            bar.Value = this.SetBarPercent(count, value);
        }
        private delegate void writeBarLogEvent(int value, int count, ProgressBar bar);
        void b_WriterLog(int value, int count, ProgressBar bar)
        {
            this.BeginInvoke(new writeBarLogEvent(writerBarLog), value, count, bar);
        }
        /// <summary>
        /// 设置进度条百分比
        /// </summary>
        /// <param name="count"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private int SetBarPercent(int count, int value)
        {
            int avg = Convert.ToInt32(count * 0.01);
            if (value < avg)
                return avg;
            return value;
        }
        #endregion
    }
}
