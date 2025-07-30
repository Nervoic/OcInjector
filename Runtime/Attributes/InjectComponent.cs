using System;

namespace OcInjector
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class InjectComponent : Attribute
    {
        public InjectScope InjectScope { get; }
        public bool AutoInject { get; }
        public bool Optional { get; }
        
        
        /// <summary>
        /// Inject a dependency located on this object, its parent or its children
        /// </summary>
        /// <param name="scope">Dependency search scope</param>
        /// <param name="autoInject">Do I need to add a component to an object if it is missing (only when searching for a component on the current object)</param>
        /// <param name="optional">Can a dependency not be found without displaying an error </param>
        public InjectComponent(InjectScope scope = InjectScope.This, bool autoInject = false, bool optional = true)
        {
            InjectScope = scope;
            AutoInject = autoInject;
            Optional = optional;
        }
    }
}
