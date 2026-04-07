using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.AreaAttack
{
    public class RadiusAreaAttack : IEntityComponent
    {
        public ReactiveVariable<float> Value = null;
    }

    public class DamageAreaAttack : IEntityComponent
    {
        public ReactiveVariable<float> Value = null;
    }
}