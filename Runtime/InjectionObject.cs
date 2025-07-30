
using UnityEngine;

namespace OcInjector
{
    public class InjectionObject : MonoBehaviour
    {
        public ObjectContainer Container { get; private set; }

        private void Awake()
        {
            var components = InjectComponents();
            InitializeContainer(components);
        }

        private Component[] InjectComponents()
        { 
            return ObjectInjector.Inject(this);
        }

        private void InitializeContainer(Component[] components)
        {
            Container = new ObjectContainer(gameObject, components);
        }
    }
}
