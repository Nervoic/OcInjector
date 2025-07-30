using System;

namespace OcInjector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InjectComponents : Attribute
    {
        public InjectScope InjectScope { get; }
        public bool Optional { get; }
        
        
        /// <summary>
        /// Inject a all dependencies located on this object, its parent or its children
        /// </summary>
        /// <param name="scope">Dependencies search scope</param>
        /// <param name="optional">Can a dependency not be found without displaying an error </param>
        public InjectComponents(InjectScope scope = InjectScope.This, bool optional = true)
        {
            InjectScope = scope;
            Optional = optional;
        }
    }
}
