using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solr.Config;

namespace Solr.Core.Index
{
    public abstract class BaseIndex
    {
        public abstract void Start(bool isCreate);
        public abstract void Stop();
        public abstract void Optimize();
        protected SolrConfig solrConfig = ConfigSetting.GetConfig();
    }
}
