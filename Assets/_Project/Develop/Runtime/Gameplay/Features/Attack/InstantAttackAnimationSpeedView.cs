using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack
{
    public class InstantAttackAnimationSpeedView : EntityView
    {
        // Consts
        private readonly int AttackAnimationSpeedMultiplierKey = Animator.StringToHash("AttackAnimationSpeedMultiplier");

        [Header("References:")]
        [SerializeField] private Animator _animator = null;
        [SerializeField] private AnimationClip _animationClip = null;

        // Runtime
        private ReactiveVariable<float> _attackProcessTime = null;

#if UNITY_EDITOR
        private void OnValidate()
            => _animator ??= GetComponent<Animator>();
#endif

        protected override void OnEntityInitialized(Entity entity)
        {
            _attackProcessTime = entity.AttackProcessInitialTime;

            _animator.SetFloat(AttackAnimationSpeedMultiplierKey, _animationClip.length / _attackProcessTime.Value);
        }
    }
}
