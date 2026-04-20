using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle
{
    public class InstantDeathSystem : IInitializableSystem
    {
        private ReactiveVariable<bool> _isDead;

        public void OnInit(Entity entity)
        {
            _isDead = entity.IsDead;

            _isDead.Value = true;
        }
    }
}