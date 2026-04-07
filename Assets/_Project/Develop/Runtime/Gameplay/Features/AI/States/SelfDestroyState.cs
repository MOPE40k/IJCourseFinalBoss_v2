using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class SelfDestroyState : State, IUpdatableState
    {
        private readonly ReactiveVariable<bool> _isDead = null;
        private readonly ReactiveVariable<float> _damage = null;
        private readonly ReactiveVariable<Entity> _currentTarget = null;

        public SelfDestroyState(Entity entity)
        {
            _isDead = entity.IsDead;
            _damage = entity.BodyContactDamage;
            _currentTarget = entity.CurrentTarget;
        }

        public override void Enter()
        {
            base.Enter();

            if (_currentTarget.Value.HasComponent<TakeDamageRequest>())
                _currentTarget.Value.TakeDamageRequest.Invoke(_damage.Value);

            _isDead.Value = true;
        }

        public void Update(float deltaTime)
        { }
    }
}
