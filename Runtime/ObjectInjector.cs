
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace OcInjector
{
    public static class ObjectInjector
    {
        
        /// <summary>
        /// Inject dependencies to all fields and properties that use the InjectComponent attribute on each component of the same object.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns></returns>
        public static Component[] Inject(InjectionObject gameObject)
        {
            try
            {
                var components = gameObject.GetComponents<Component>();

                foreach (var component in components)
                {
                    var componentType = component.GetType();
                    var injectComponentMembers = componentType
                        .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(m => m.IsDefined(typeof(InjectComponent), true));

                    var injectComponentsMembers = componentType
                        .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                        .Where(m => m.IsDefined(typeof(InjectComponents), true));
                    
                    Type dependencyType = null;
                    IInjector injector = null;
                    
                    foreach (var memberInfo in injectComponentMembers)
                    {
                        SetDependencyType(ref dependencyType, memberInfo);
                        injector = new ComponentInjector();
                        injector.Inject(component, memberInfo, dependencyType);
                    }

                    foreach (var memberInfo in injectComponentsMembers)
                    {
                        SetDependencyType(ref dependencyType, memberInfo);
                        injector = new ComponentsInjector();
                        injector.Inject(component, memberInfo, dependencyType);
                    }
                }

                return components;
            }
            catch (Exception e)
            {
                Debug.LogError($"Inject components exception on {gameObject.name} - {e.Message}");
                return null;
            }
        }

        private static void SetDependencyType(ref Type dependencyType, MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo fieldInfo) dependencyType = fieldInfo.FieldType;
            if (memberInfo is PropertyInfo propertyInfo) dependencyType = propertyInfo.PropertyType;
        }
        
        /// <summary>
        /// Sets the necessary dependencies in fields and properties
        /// </summary>
        /// <param name="component"></param>
        /// <param name="dependencyType"></param>
        /// <param name="memberInfo"></param>
        private static void SetDependency(Component component, Type dependencyType, MemberInfo memberInfo)
        {
            try
            {
                var componentObject = component.gameObject;
                
                var attribute = memberInfo.GetCustomAttribute<InjectComponent>();
                
                var scope = attribute.InjectScope;
                var autoInject = attribute.AutoInject;
                var optional = attribute.Optional;
                
                var dependency = FindDependency(component, dependencyType, scope);
                if (dependency is null)
                {
                    if (scope != InjectScope.This && !optional) Debug.LogError($"Failed to set dependency from {componentObject.name} with use {scope} scope");
                    else if (autoInject) dependency = componentObject.AddComponent(dependencyType);
                }

                if (memberInfo is FieldInfo fieldInfo) fieldInfo.SetValue(component, dependency);
                if (memberInfo is PropertyInfo propertyInfo && propertyInfo.CanWrite) propertyInfo.SetValue(component, dependency);
            }
            catch (Exception e)
            {
                Debug.LogError($"Set dependency exception on {memberInfo.Name}, {memberInfo.GetType()}, {component.gameObject.name} - {e.Message}");
            }
        }
        
        /// <summary>
        /// Searches for a component depending on the selected search scope
        /// </summary>
        /// <param name="component"></param>
        /// <param name="componentType"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private static Component FindDependency(Component component, Type componentType, InjectScope scope)
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
