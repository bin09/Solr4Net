using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrNet.Attributes;

namespace Solr.MODEL
{
    [Serializable]
    public class ModBuys
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
        [SolrField("buyContent")]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
    }
}
