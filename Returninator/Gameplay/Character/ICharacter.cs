using Godot;

namespace Returninator.Gameplay
{
    public interface ICharacter: IEntity
    {
        void SetResetPosition(Vector2 resetPosition);
        void SetInput(IInputChannel input);
    }
}
