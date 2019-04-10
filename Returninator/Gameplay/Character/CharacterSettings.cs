using Godot;

namespace Returninator.Gameplay
{
    public class CharacterSettings: Node
    {
        [Export]
        private MovementParameter m_Acceleration = new MovementParameter(10f, new Curve());
        [Export]
        private MovementParameter m_Deacceleration = new MovementParameter(5f, new Curve());
        [Export]
        private MovementParameter m_AirAcceleration = new MovementParameter(4f, new Curve());
        [Export]
        private MovementParameter m_AirDeacceleration = new MovementParameter(2f, new Curve());
        [Export]
        private float m_MaxSpeed = 10f;

        [Export]
        private MovementParameter m_Gravity = new MovementParameter(9.82f, new Curve());

        [Export]
        private float m_MaxFallSpeed = 50f;

        public float MaxAccelerationSpeed => m_MaxSpeed;
        public float MaxFallSpeed => m_MaxFallSpeed;
        public float GetAcceleration(float speed, bool grounded)
            => grounded ?
                m_Acceleration.GetValue(speed / m_MaxSpeed) :
                m_AirAcceleration.GetValue(speed / m_MaxSpeed);

        public float GetDeacceleration(float speed, bool grounded)
            => grounded ?
                m_Deacceleration.GetValue(speed / m_MaxSpeed) :
                m_AirDeacceleration.GetValue(speed / m_MaxSpeed);

        public float GetGravity(float verticalSpeed)
            => m_Gravity.GetValue(verticalSpeed / m_MaxFallSpeed);
    }
}