using System;
using SolrNet.Attributes;

namespace Solr.MODEL
{
    [Serializable]
    public class ModKeyword
    {
        private int id;
        [SolrUniqueKey("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string keyword;
        [SolrField("keyword")]
        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }
    }
}
