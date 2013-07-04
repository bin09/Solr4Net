using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.Core;

namespace Solr.SolrNetIntegration
{
    /// <summary>
    /// Resolves all components with a particular service interface, without subtyping
    /// </summary>
    public class StrictArrayResolver : ISubDependencyResolver
    {
        private readonly IKernel kernel;

        /// <summary>
        /// Resolves all components with a particular service interface, without subtyping
        /// </summary>
        /// <param name="kernel"></param>
        public StrictArrayResolver(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public object Resolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
        {
            return kernel.ResolveAll(dependency.TargetType.GetElementType());
        }

        public bool CanResolve(CreationContext context, ISubDependencyResolver contextHandlerResolver, ComponentModel model, DependencyModel dependency)
        {
            return dependency.TargetType != null &&
                   dependency.TargetType.IsArray;
        }
    }
}
