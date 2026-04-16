using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public interface ITargetProvider
    {
        bool TryGetTarget(out Entity target);
    }
}
