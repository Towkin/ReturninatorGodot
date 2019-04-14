using Godot;

namespace Returninator.Gameplay
{
    public class MovementParameter: Resource
    {
        public MovementParameter() { }

        public MovementParameter(float value, Curve factor)
        {
            Value = value;
            Factor = factor;
        }

        [Export]
        public float Value { get; private set; }
        [Export]
        public Curve Factor { get; private set; }

        public float GetValue(float atPosition)
            => Value * Factor.Interpolate(System.Math.Abs(atPosition));
    }
}