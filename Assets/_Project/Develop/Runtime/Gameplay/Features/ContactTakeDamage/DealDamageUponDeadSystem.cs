using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace CourseGameVideo.Assets._Project.Develop.Runtime.Gameplay.Features.ContactTakeDamage
{
    public class DealDamageUponDeadSystem : IInitializableSystem, IDisposableSystem
    {
        private Entity _entity = null;
        private Buffer<Entity> _contacts = null;
        private ReactiveVariable<bool> _isDead = null;
        private ReactiveVariable<float> _damage = null;

        private IDisposable _isDeadDisposable = null;

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _contacts = entity.ContactEntitiesBuffer;
            _isDead = entity.IsDead;
            _damage = entity.DamageAreaAttack;

            _isDeadDisposable = _isDead.Subscribe(OnIsDead);
        }

        public void OnDispose()
        {
            _isDeadDisposable.Dispose();
        }

        private void OnIsDead(bool oldValue, bool isDead)
        {
            for (int i = 0; i < _contacts.Count; i++)
                EntitiesHelper.TryTakeDamageFrom(_entity, _contacts.Items[i], _damage.Value);
        }
    }
}