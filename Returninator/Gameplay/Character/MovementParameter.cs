using System;
using Godot;

namespace Returninator.Gameplay
{
    public class MovementParameter: Node
    {
        public MovementParameter(float value, Curve factor)
        {
            m_Value = value;
            m_Factor = factor;
        }

        [Export]
        private float m_Value;
        [Export]
        private Curve m_Factor;

        public float GetValue(float atPosition)
            => m_Value * m_Factor.Interpolate(atPosition);
    }
}