using Godot;

namespace Example
{
    public class Bullet : Area2D
    {
        private const int speed = 1000;
        private bool _isFired = false;

        private float _shapeRadius;

        [Signal]
        public delegate void Release(Node target);

        public override void _Ready()
        {
            Visible = false;
            Shape2D shape = GetNode<CollisionShape2D>(nameof(CollisionShape2D)).Shape;
            _shapeRadius = shape.GetPropertyValue<float>("Radius");
        }

        public override void _Process(float delta)
        {
            if (_isFired)
            {
                Position += Vector2.Right * speed * delta;
            }
        }

        public void Shoot()
        {
            float x = -100f;
            float y = (float)GD.RandRange(60, GetViewport().Size.y - _shapeRadius);

            Position = new Vector2(x, y);
            Visible = true;
            _isFired = true;
        }

        public void OnVisibilityNotifier2DScreenExited()
        {
            _isFired = false;
            Visible = false;
            EmitSignal(nameof(Release), this);
        }
    }
}