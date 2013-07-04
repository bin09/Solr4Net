using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Solr.SolrNetIntegration
{
    /// <summary>
    /// Represents a Solr core for configuration
    /// </summary>
    internal class SolrCore
    {
        public string Id { get; private set; }
        public Type DocumentType { get; private set; }
        public string Url { get; private set; }

        /// <summary>
        /// Creates a new Solr core for configuration
        /// </summary>
        /// <param name="id">Component name for <see cref="ISolrOperations{T}"/></param>
        /// <param name="documentType">Document type</param>
        /// <param name="url">Core url</param>
        public SolrCore(string id, Type documentType, string url)
        {
            Id = id;
            DocumentType = documentType;
            Url = url;
        }

        /// <summary>
        /// Creates a new Solr core for configuration
        /// </summary>
        /// <param name="documentType">Document type</param>
        /// <param name="url">Core url</param>
        public SolrCore(Type documentType, string url) : this(Guid.NewGuid().ToString(), documentType, url) { }
    }
}
