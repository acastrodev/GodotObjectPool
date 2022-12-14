# GodotObjectPool
Simple object pool design pattern implementation using C# in Godot Game Engine.

![Sreenshot](https://github.com/acastrodev/GodotObjectPool/blob/main/Screenshot.png?raw=true)

# References
- https://github.com/godot-addons/godot-object-pool
- https://en.wikipedia.org/wiki/Object_pool_pattern

# Usage example:

Main.cs

```csharp
using Godot;

using ObjectPool;

namespace Example
{
    public class Main : Node
    {
        private const int PoolSize = 50;
        private readonly PackedScene _bulletPackedScene = ResourceLoader.Load("res://Scenes/Bullet.tscn") as PackedScene;
        private readonly PackedScene _poolPackedScene = ResourceLoader.Load("res://Addons/Scenes/Pool.tscn") as PackedScene;

        private Pool _bulletPool;
        private Hud _hud;

        public override void _Ready()
        {
            _hud = GetNode<Hud>(nameof(Hud));

            Node bulletsNode = new Node
            {
                Name = "Bullets"
            };

            AddChild(bulletsNode);

            _bulletPool = _poolPackedScene.Instance<Pool>();
            _bulletPool.Initialize<Bullet>(_bulletPackedScene, PoolSize, bulletsNode, nameof(Bullet.Release));
            _bulletPool.Connect(nameof(Pool.PoolRelease), this, nameof(OnPoolRelease));
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("mouse_click"))
            {
                Bullet bullet = _bulletPool.Get<Bullet>();

                if (bullet != null)
                {
                    bullet.Shoot();
                }
            }
        }

        public void OnPoolRelease(Bullet target)
        {
            _hud.UpdateStatus(_bulletPool.AvailableCount, _bulletPool.InUseCount, target.Name);
        }
    }
}
```
