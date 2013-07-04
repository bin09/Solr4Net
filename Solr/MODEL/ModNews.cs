using System;
using SolrNet.Attributes;

namespace Solr.MODEL
{
    [Serializable]
    public class ModNews
    {
        private int id;
        [SolrUniqueKey("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string title;
        [SolrField("title")]
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        private string content;
        [SolrField("newsContent")]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
    }
}
