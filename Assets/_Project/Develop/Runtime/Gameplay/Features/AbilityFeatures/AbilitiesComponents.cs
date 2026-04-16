using System.Collections.Generic;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AbilityFeatures
{
    public class CurrentAbility : IEntityComponent
    {
        public ReactiveVariable<Abilities> Value = null;
    }

    public class AbilitiesList : IEntityComponent
    {
        public ReactiveVariable<Dictionary<Abilities, ReactiveEvent>> Value = null;
    }
}