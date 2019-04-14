using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Godot;

namespace Returninator.Gameplay
{
    public class Character: KinematicBody2D, ICharacter
    {
        private InputState m_CurrentInput;
        private IInputChannel m_InputChannel;
        private CharacterMovement m_Movement;
        [Export]
        private CharacterSettings Settings { get; set; }
        
        public override void _Ready() => Reset();
        public void Reset()
        {
            m_CurrentInput = default(InputState);
            m_Movement = default(CharacterMovement);
            SetInput(new PlayerInputChannel());
        }

        public void SetResetPosition(Vector2 resetPosition)
        {

        }

        public void SetInput(IInputChannel input)
        {
            m_InputChannel = input;
        }

        float TickTime { get; set; } = 0.01f;
        bool Grounded { get; set; } = false;

        public override void _PhysicsProcess(float delta)
        {
            TickTime = delta;
            Tick();
        }

        int CurrentTick { get; set; }
        public void Tick()
        {
            UpdateInput();
            UpdateVelocity();
            UpdatePosition();
            CurrentTick++;
        }

        private void UpdateInput()
        {
            m_CurrentInput.UpdateState(m_InputChannel.GetInputChange());
        }

        private void UpdateVelocity()
        {
            var deacceleration = Settings.GetDeacceleration(m_Movement.Speed, Grounded);
            var speedRemove = deacceleration * TickTime;
            m_Movement.Speed = Mathf.Max(0, m_Movement.Speed - speedRemove);

            if (m_Movement.Speed < Settings.MaxAccelerationSpeed)
            {
                var inputVector = new Vector2(m_CurrentInput.Horizontal, 0f).Clamped(1.0f);
                var acceleration = Settings.GetAcceleration(inputVector.Dot(m_Movement.Velocity), Grounded);
                var velocityAdd = inputVector * acceleration * TickTime;

                m_Movement.Velocity = (m_Movement.Velocity + velocityAdd).Clamped(Settings.MaxAccelerationSpeed);
            }

            if (Grounded && m_CurrentInput.Jump)
                m_Movement.Velocity += new Vector2(0f, -Settings.GetJumpForce(m_Movement.Speed));
        }
        
        private void UpdatePosition()
        {
            m_Movement.SpeedY = Math.Min(Settings.MaxFallSpeed, m_Movement.SpeedY + Settings.GetGravity(m_Movement.SpeedY) * TickTime);
            Grounded = TestMove(Transform, new Vector2(0f, m_Movement.SpeedY) * TickTime);

            float bounciness = 0.05f;
            var beginSpeed = m_Movement.Speed;

            var timeLeft = TickTime;

            const int MaxIterations = 10;
            for (int iteration = 0; iteration < MaxIterations && (m_Movement.Speed * timeLeft) > 0.0001f; iteration++)
            {
                var moveSlice = m_Movement.Velocity * timeLeft;
                var collision = MoveAndCollide(moveSlice);
                if (collision == null)
                    break;
                
                var timeSlice = timeLeft * (collision.Travel.Length() / moveSlice.Length());
                timeLeft -= timeSlice;

                var projected = m_Movement.Velocity.Slide(collision.Normal);
                var reflected = m_Movement.Velocity.Bounce(collision.Normal);
                var interpolated = projected.LinearInterpolate(reflected, bounciness);
                
                m_Movement.Velocity = interpolated;
                //float friction = 0;
                // Perform friction based on time slice and how much we're following along the ground.
                //m_Movement.Speed = Mathf.Max(0f, m_Movement.Speed - alongGround * friction * timeSlice);

            }
        }
    }

    [Serializable]
    public struct CharacterMovement
    {
        private Vector2 m_Direction;
        private float m_Magnitude;

        public float Speed
        {
            get => m_Magnitude;
            set
            {
                if (value < 0f)
                {
                    m_Direction = -m_Direction;
                    m_Magnitude = -value;
                }
                else
                {
                    m_Magnitude = value;
                }
            }
        }
        public float SpeedX
        {
            get => Direction.x * Speed;
            set => Velocity = new Vector2(value, SpeedY);
        }
        public float SpeedY
        {
            get => Direction.y * Speed;
            set => Velocity = new Vector2(SpeedX, value);
        }

        public Vector2 Direction
        {
            get => m_Direction;
            set
            {
                if (value == Vector2.Zero)
                    return;

                m_Direction = value.Normalized();
            }
        }

        public Vector2 Velocity
        {
            get => Direction * Speed;
            set
            {
                Speed = value.Length();
                if (Speed > float.Epsilon)
                    m_Direction = value / Speed;
            }
        }
    }
}