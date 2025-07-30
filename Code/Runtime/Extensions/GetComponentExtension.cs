
using UnityEngine;

namespace OcInjector
{
    public static class GetComponentExtension
    {
        
        /// <summary>
        /// Allows you to get the component directly from the object container. If there is no component, it is added to the object automatically.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetComponentFromContainer<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject is null) return null;
            
            if (!gameObject.TryGetComponent<InjectionObject>(out var injectionObject)) injectionObject = gameObject.AddComponent<InjectionObject>();
            var component = injectionObject.Container.GetComponent<T>();

            return component;
        }
    }
}
