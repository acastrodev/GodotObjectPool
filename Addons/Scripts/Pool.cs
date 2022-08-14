using Godot;
using Godot.Collections;

namespace ObjectPool
{
    public class Pool : Node
    {
        private readonly Array<Node> _available = new Array<Node>();
        private readonly Array<Node> _inUse = new Array<Node>();

        public int AvailableCount => _available.Count;
        public int InUseCount => _inUse.Count;

        [Signal]
        public delegate void PoolRelease(Node target);

        public void Initialize<Type>(PackedScene scene, int size, Node parentNode, string signalToRelease) where Type : Node
        {
            lock (_available)
            {
                for (int i = 0; i < size; i++)
                {
                    var target = scene.Instance<Type>();
                    target.Name = $"{target.Name}_{i}";
                    target.Connect(signalToRelease, this, nameof(OnRelease));
                    _available.Add(target);
                    parentNode.AddChild(target);
                }
            }
        }

        public Type Get<Type>() where Type : Node
        {
            lock (_available)
            {
                Type target = null;

                if (_available.Count > 0)
                {
                    target = (Type)_available[0];
                    _inUse.Add(target);
                    _available.RemoveAt(0);
                }

                return target;
            }
        }

        public void Release(Node target)
        {
            lock (_available)
            {
                _available.Add(target);
                _inUse.Remove(target);
            }
        }

        public void OnRelease(Node target)
        {
            Release(target);
            EmitSignal(nameof(PoolRelease), target);
        }

        public void ReleaseAll()
        {
            lock (_available)
            {
                for (int i = 0; i < _inUse.Count; i++)
                {
                    Release(_inUse[i]);
                }
            }
        }
    }
}