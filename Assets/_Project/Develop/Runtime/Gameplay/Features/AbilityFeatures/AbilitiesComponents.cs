using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace CourseGameVideo.Assets._Project.Develop.Runtime.Gameplay.Features.AbilityFeatures
{
    public class CurrentAbility : IEntityComponent
    {
        public ReactiveVariable<Abilities> Value = null;
    }
}