

using System;
using System.Reflection;
using UnityEngine;

namespace OcInjector
{
    public class ComponentInjector : IInjector
    {
        
        /// <summary>
        /// Sets the necessary dependencies in fields and properties
        /// </summary>
        /// <param name="component"></param>
        /// <param name="dependencyType"></param>
        /// <param name="memberInfo"></param>
        public void Inject(Component component, MemberInfo memberInfo, Type dependencyType)
        {
            try
            {
                var componentObject = component.gameObject;
                
                var attribute = memberInfo.GetCustomAttribute<InjectComponent>();
                
                var scope = attribute.InjectScope;
                var autoInject = attribute.AutoInject;
                var optional = attribute.Optional;
                
                var dependency = FindDependencyByScope(component, dependencyType, scope);
                if (dependency is null)
                {
                    if (scope != InjectScope.This && !optional) Debug.LogError($"Failed to set dependency from {componentObject.name} with use {scope} scope");
                    else if (autoInject) dependency = componentObject.AddComponent<InjectionObject>();
                }

                if (memberInfo is FieldInfo fieldInfo) fieldInfo.SetValue(component, dependency);
                if (memberInfo is PropertyInfo propertyInfo && propertyInfo.CanWrite) propertyInfo.SetValue(component, dependency);
            }
            catch (Exception e)
            {
                Debug.LogError($"Set dependency exception on {memberInfo.Name}, {memberInfo.GetType()}, {component.gameObject.name} - {e.Message}");
            }
        }

        private Component FindDependencyByScope(Component component, Type componentType, InjectScope scope)
        {
            switch (scope)
            {
                case InjectScope.This:
                    return component.GetComponent(componentType);
                case InjectScope.Children:
                    return component.GetComponentInChildren(componentType);
                case InjectScope.Parent:
                    return component.GetComponentInParent(componentType);
                default:
                    throw new ArgumentOutOfRangeException(nameof(scope), scope, null);
            }
        }
    }
}
