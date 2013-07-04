using Solr.Core.Index;

namespace Solr.Task
{
    public class TaskCalls
    {
        private static TaskCalls taskCalls = null;
        private TaskCalls() { }
        public static TaskCalls Instance()
        {
            if (taskCalls == null)
                return new TaskCalls();
            return taskCalls;
        }
        /// <summary>
        /// 启动所有任务
        /// </summary>
        /// <param name="task"></param>
        public void StartTask(bool isCreate, params BaseIndex[] task)
        {
            if (task != null)
            {
                foreach (var item in task)
                    item.Start(isCreate);
            }
        }
        /// <summary>
        /// 停止所有任务
        /// </summary>
        /// <param name="task"></param>
        public void StopTask(params BaseIndex[] task)
        {
            if (task != null)
            {
                foreach (var item in task)
                    item.Stop();
            }
        }
        /// <summary>
        /// 优化
        /// </summary>
        /// <param name="task"></param>
        public void Optimize(params BaseIndex[] task)
        {
            if (task != null)
            {
                foreach (var item in task)
                    item.Optimize();
            }
        }
    }
}
