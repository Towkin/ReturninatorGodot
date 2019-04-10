using System.Collections.Generic;

namespace Returninator.Gameplay
{
    public struct InputChange
    {
        public InputChange(IReadOnlyDictionary<InputAction, bool> actions, IReadOnlyDictionary<InputAxis, float> axes)
        {
            Actions = actions;
            Axes = axes;
        }

        public IReadOnlyDictionary<InputAction, bool> Actions { get; }
        public IReadOnlyDictionary<InputAxis, float> Axes { get; }

        public int Count => (Actions?.Count ?? 0) + (Axes?.Count ?? 0);
    }
}