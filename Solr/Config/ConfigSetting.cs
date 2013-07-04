using System.IO;
using System.Xml;
using Solr.Log;
using Solr.Helper;

namespace Solr.Config
{
    public class ConfigSetting
    {
        private static string fileName = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "FileConfig/Solr.config");
        private static object _lock = new object();
        private static string solrConfigCache = "solrConfigCache";

        public static SolrConfig GetConfig()
        {
            SolrConfig config = Cache.CacheManage.GetCache(solrConfigCache) as SolrConfig;
            if (config == null)
                config = SetCache();
            return config;
        }
        /// <summary>
        /// 写入缓存
        /// </summary>
        /// <returns></returns>
        private static SolrConfig SetCache()
        {
            SolrConfig config = (SolrConfig)SerializationHelper.Load(typeof(SolrConfig), fileName);
            Cache.CacheManage.SaveCache(solrConfigCache, config, fileName);
            return config;
        }

        #region XML写入
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="node"></param>
        /// <param name="value"></param>
        public static void SetValue(string node, int value)
        {
            lock (_lock)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNode xNode = doc.SelectSingleNode("SolrConfig/" + node);
                xNode.InnerText = value.ToString();
                doc.Save(fileName);
            }
        }
        public static string GetValue(string node)
        {
            lock (_lock)
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);
                XmlNode xNode = doc.SelectSingleNode("SolrConfig/" + node);
                return xNode.InnerText;
            }
        }
        #endregion
    }
}
