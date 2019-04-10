using Godot;
using System;
using System.Collections.Generic;

namespace Returninator.Gameplay
{
    public class PlayerInputChannel: IInputChannel
    {
        private InputState m_LastInput = default(InputState);

        private void UpdateAxis(InputAxis axis, Dictionary<InputAxis, float> values)
        {
            var inputValue = 
                Input.GetActionStrength(axis.GetActionName(AxisDirection.Positive)) -
                Input.GetActionStrength(axis.GetActionName(AxisDirection.Negative));
            if (Mathf.Abs(inputValue - m_LastInput[axis]) > 0.0001f)
                values.Add(axis, inputValue);
        }
        private void UpdateAction(InputAction action, Dictionary<InputAction, bool> values)
        {
            var inputValue = Input.IsActionPressed(action.GetActionName());
            if (inputValue != m_LastInput[action])
                values.Add(action, inputValue);
        }

        public InputChange GetInputChange()
        {
            var axes = new Dictionary<InputAxis, float>(2);
            UpdateAxis(InputAxis.Horizontal, axes);
            UpdateAxis(InputAxis.Vertical, axes);
            
            var actions = new Dictionary<InputAction, bool>(3);
            UpdateAction(InputAction.Fire, actions);
            UpdateAction(InputAction.Interact, actions);
            UpdateAction(InputAction.Jump, actions);

            var change = new InputChange(actions, axes);
            m_LastInput.UpdateState(change);
            return change;
        }
    }

    public enum AxisDirection
    {
        Positive,
        Negative,
    }

    public static class InputEnumExtensions
    {
        public static string GetActionName(this InputAxis axis, AxisDirection direction)
        {
            switch (axis)
            {
                case InputAxis.Horizontal:
                    switch (direction)
                    {
                        case AxisDirection.Positive:
                            return "gameplay_right";
                        case AxisDirection.Negative:
                            return "gameplay_left";
                        default:
                            throw new NotImplementedException("Missing AxisDirection " + direction.ToString());
                    }
                case InputAxis.Vertical:
                    switch (direction)
                    {
                        case AxisDirection.Positive:
                            return "gameplay_down";
                        case AxisDirection.Negative:
                            return "gameplay_up";
                        default:
                            throw new NotImplementedException("Missing AxisDirection " + direction.ToString());
                    }

                default:
                    throw new NotImplementedException("Missing InputAxis " + axis.ToString());
            }
        }
        public static string GetActionName(this InputAction action)
        {
            switch (action)
            {
                case InputAction.Jump:
                    return "gameplay_jump";
                case InputAction.Fire:
                    return "gameplay_fire";
                case InputAction.Interact:
                    return "gameplay_interact";
                default:
                    throw new NotImplementedException("Missing InputAction " + action.ToString());
            }
        }
    }
}