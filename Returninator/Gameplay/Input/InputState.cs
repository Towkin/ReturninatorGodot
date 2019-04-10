
namespace Returninator.Gameplay
{
    public struct InputState
    {
        public bool this[InputAction action]
        {
            get
            {
                switch (action)
                {
                    case InputAction.Jump: return Jump;
                    case InputAction.Fire: return Fire;
                    case InputAction.Interact: return Interact;
                    default:
                        throw new System.NotImplementedException("Missing InputAction " + action.ToString());
                }
            }
            private set
            {

                switch (action)
                {
                    case InputAction.Jump:
                        Jump = value;
                        break;
                    case InputAction.Fire:
                        Fire = value;
                        break;
                    case InputAction.Interact:
                        Interact = value;
                        break;
                    default:
                        throw new System.NotImplementedException("Missing InputAction " + action.ToString());
                }
            }
        }
        public float this[InputAxis axis]
        {
            get
            {
                switch (axis)
                {
                    case InputAxis.Horizontal: return Horizontal;
                    case InputAxis.Vertical: return Vertical;
                    default:
                        throw new System.NotImplementedException("Missing InputAxis " + axis.ToString());
                }
            }
            private set
            {
                switch (axis)
                {
                    case InputAxis.Horizontal:
                        Horizontal = value;
                        break;
                    case InputAxis.Vertical:
                        Vertical = value;
                        break;
                    default:
                        throw new System.NotImplementedException("Missing InputAxis " + axis.ToString());
                }
            }
        }
        
        public bool Jump        { get; private set; }
        public bool Fire        { get; private set; }
        public bool Interact    { get; private set; }
        
        public float Horizontal { get; private set; }
        public float Vertical   { get; private set; }

        public void UpdateState(InputChange change)
        {
            if (change.Count == 0)
                return;

            foreach (var action in change.Actions)
                this[action.Key] = action.Value;

            foreach (var axis in change.Axes)
                this[axis.Key] = axis.Value;
        }
    }
}