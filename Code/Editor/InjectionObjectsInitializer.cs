
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace OcInjector
{
    [InitializeOnLoad]
    public static class InjectionObjectsInitializer
    {
        static InjectionObjectsInitializer()
        {
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
        }
        
        /// <summary>
        /// Automatically searches the scene for all objects containing components using the InjectComponent attribute and adds the InjectionObject component to them.
        /// </summary>
        private static void OnHierarchyChanged()
        {
            var modifiedObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None)
                .Where(obj => EditorUtility.IsDirty(obj));

            foreach (var obj in modifiedObjects)
            {
                ProcessModifiedObject(obj);
            }
        }

        private static void ProcessModifiedObject(GameObject obj)
        {
            bool needsInjection = false;

            var components = obj.GetComponents<Component>();

            foreach (var component in components)
            {
                var componentType = component.GetType();

                if (CanInjection(componentType))
                {
                    needsInjection = true;
                    break;
                }
            }

            var injectionObject = obj.GetComponent<InjectionObject>();

            if (needsInjection && injectionObject is null) obj.AddComponent<InjectionObject>();
            else if(!needsInjection && injectionObject is not null) Object.DestroyImmediate(injectionObject);

        }

        private static bool CanInjection(Type type)
        {
            return type.GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Any(m => m.IsDefined(typeof(InjectComponent), true)) ;
        }
        
    }
}
