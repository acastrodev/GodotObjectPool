using Godot;

namespace Example
{
    public class Hud : CanvasLayer
    {
        private Label _available;
        private Label _inUse;
        private Label _releasing;

        public override void _Ready()
        {
            _available = GetNode<Label>("Available");
            _inUse = GetNode<Label>("InUse");
            _releasing = GetNode<Label>("Releasing");
        }

        public void UpdateStatus(int available, int inUse, string objectName)
        {
            _available.Text = $"Objects avaiable: {available}";
            _inUse.Text = $"Objects in use: {inUse}";
            _releasing.Text = $"Last object released: {objectName}";
        }
    }
}