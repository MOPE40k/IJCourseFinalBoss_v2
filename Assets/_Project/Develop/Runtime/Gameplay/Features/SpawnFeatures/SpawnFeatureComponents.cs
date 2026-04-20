using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpawnFeatures
{
    public class SpawnInitialTime : IEntityComponent
    {
        public ReactiveVariable<float> Value = null;
    }

    public class SpawnCurrentTime : IEntityComponent
    {
        public ReactiveVariable<float> Value = null;
    }

    public class InSpawnProcess : IEntityComponent
    {
        public ReactiveVariable<bool> Value = null;
    }
}