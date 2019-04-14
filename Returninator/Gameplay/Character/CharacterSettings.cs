using Godot;

namespace Returninator.Gameplay
{
    public class CharacterSettings: Resource
    {
        [Export]
        public MovementParameter Acceleration { get; private set; }
            //= new MovementParameter(10f, new Curve());
        [Export]
        public MovementParameter Deacceleration { get; private set; } 
            //= new MovementParameter(5f, new Curve());
        [Export]
        public MovementParameter AirAcceleration { get; private set; } 
            //= new MovementParameter(4f, new Curve());
        [Export]
        public MovementParameter AirDeacceleration { get; private set; } 
            //= new MovementParameter(2f, new Curve());
        [Export]
        public float MaxSpeed { get; private set; }
            //= 10f;
        public float MaxAccelerationSpeed => MaxSpeed;

        [Export]
        public MovementParameter JumpForce { get; private set; }

        [Export]
        public MovementParameter Gravity { get; private set; } 
            //= new MovementParameter(9.82f, new Curve());

        [Export]
        public float MaxFallSpeed { get; private set; }
            //= 50f;

        public float GetAcceleration(float speed, bool grounded)
            => grounded ?
                Acceleration.GetValue(speed / MaxSpeed) :
                AirAcceleration.GetValue(speed / MaxSpeed);

        public float GetDeacceleration(float speed, bool grounded)
            => grounded ?
                Deacceleration.GetValue(speed / MaxSpeed) :
                AirDeacceleration.GetValue(speed / MaxSpeed);

        public float GetJumpForce(float speed)
            => JumpForce.GetValue(speed / MaxSpeed);

        public float GetGravity(float verticalSpeed)
            => Gravity.GetValue(verticalSpeed / MaxFallSpeed);
    }
}