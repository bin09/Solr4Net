using System.Windows.Forms;

namespace Solr.Log
{
    public class LogMsg
    {
        public DataGridView LogView { get; set; }
        public ProgressBar LogBar { get; set; }
        public LogMsg(DataGridView LogView, ProgressBar LogBar)
        {
            this.LogView = LogView;
            if (LogBar != null)
                this.LogBar = LogBar;
        }
        public delegate void LogEvent(int id, string message, DataGridView dgv);
        public event LogEvent WriteLog;
        /// <summary>
        /// 写入日志消息
        /// </summary>
        /// <param name="message"></param>
        public void writeLog(int id, string message)
        {
            if (WriteLog != null)
            {
                WriteLog(id, message, LogView);
            }
        }
        //进度条
        public delegate void LogBarEvent(int value, int count, ProgressBar bar);
        public event LogBarEvent WriteBarLog;
        public void writeBarLog(int value, int count)
        {
            if (WriteBarLog != null)
            {
                WriteBarLog(value, count, this.LogBar);
            }
        }
    }
}
