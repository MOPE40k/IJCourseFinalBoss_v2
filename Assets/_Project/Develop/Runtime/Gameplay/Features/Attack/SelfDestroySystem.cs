using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack
{
    public class SelfDestroySystem : IInitializableSystem
    {
        private IReadOnlyEvent _attackDelayEndEvent;

        private IReadOnlyVariable<Entity> _currentTarget = null;

        private ReactiveVariable<bool> _isDead = null;

        private IReadOnlyVariable<float> _damage;

        private IDisposable _attackDelayEndDisposable;

        public void OnInit(Entity entity)
        {
            _currentTarget = entity.CurrentTarget;

            _isDead = entity.IsDead;

            _attackDelayEndEvent = entity.AttackDelayEndEvent;

            _damage = entity.DamageAreaAttack;

            _attackDelayEndDisposable = _attackDelayEndEvent.Subscribe(OnAttackDelayEnd);
        }

        public void OnDispose()
            => _attackDelayEndDisposable.Dispose();

        private void OnAttackDelayEnd()
        {
            if (IsDamageable())
                _currentTarget.Value.TakeDamageRequest.Invoke(_damage.Value);

            _isDead.Value = true;
        }

        private bool IsDamageable()
            => _currentTarget.Value.HasComponent<TakeDamageRequest>();
    }
}