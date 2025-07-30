

using System;
using System.Collections.Generic;
using UnityEngine;

namespace OcInjector
{
    public class ObjectContainer
    {
        private readonly Dictionary<Type, Component> _objectComponents = new();

        private readonly GameObject _gameObject;

        public ObjectContainer(GameObject gameObject, Component[] components)
        {
            _gameObject = gameObject;
            AddRangeToContainer(components);
        }
        
        
        /// <summary>
        /// Trying to get a component from a container. If it is missing, it automatically adds the component and returns it.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetComponent<T>() where T : Component
        {
            if (_objectComponents.TryGetValue(typeof(T), out var component)) return (T)component;

            component = _gameObject.GetComponent<T>() ?? _gameObject.AddComponent<T>();
            AddToContainer(component);
            
            return (T)component;
        }

        private void AddToContainer<T>(T component) where T : Component
        {
            if (component.gameObject != _gameObject) return;
            
            _objectComponents[typeof(T)] = component;
        }

        private void AddRangeToContainer<T>(T[] components) where T : Component
        {
            foreach (var component in components)
            {
                if (component.gameObject != _gameObject) continue;
                
                AddToContainer(component);
            }
        }

        private void RemoveFromContainer<T>() where T : Component
        {
            if (_objectComponents[typeof(T)] is null) return;

            _objectComponents.Remove(typeof(T));
        }
    }
}
