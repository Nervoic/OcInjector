using System;
using System.Reflection;
using UnityEngine;

namespace OcInjector
{
    public interface IInjector
    {
        public void Inject(Component component, MemberInfo memberInfo, Type dependencyType);
    }
}
