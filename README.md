🚀 **Lightweight Dependency Injection Framework for Unity**
OcInjector is a high-performance DI container designed specifically for Unity projects. It provides clean, annotation-based dependency injection while maintaining minimal overhead and maximum flexibility.

✨ ***Key Features**
Annotation-Driven DI with [Inject] and [InjectComponents] attributes

Multiple Injection Scopes (Component, Parent, Children)

Array/List Injection for multi-dependencies

Optional Dependencies with Optional=true

Auto-Component Creation when dependencies are missing

Editor Integration with automatic [InitializeOnLoad] setup

🚀 **Quick Start**
Add [Inject] to your fields:

`public class Player : MonoBehaviour 
{
    [Inject] private AudioService _audio;
    [Inject(Scope=InjectScope.Parent)] private Rigidbody _rb;
}`
Attach InjectionObject component to your GameObject

⚡ **Performance Optimized**
Smart component caching

Minimal reflection overhead

Lightweight container implementation

🔌 **Compatibility**
Unity 2019.4+

Supports both MonoBehaviour and ScriptableObject

Works in Editor and Runtime

**Perfect for**:

Indie game development

Tool development

Medium-sized Unity projects
