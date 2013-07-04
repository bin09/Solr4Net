using System;
using SolrNet.Attributes;

namespace Solr.MODEL
{
    [Serializable]
    public class ModMember
    {
        private int id;
        [SolrUniqueKey("id")]
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        private string companyName;
        [SolrField("companyName")]
        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }
        private string content;
        [SolrField("profile")]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
    }
}
