using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature
{
    public class InstantAttackView : EntityView
    {
        // Consts
        private readonly int InAttackProcessKey = Animator.StringToHash("InAttack");

        [Header("References:")]
        [SerializeField] private Animator _animator = null;

        // Runtime
        private ReactiveVariable<bool> _inAttackProcess = null;
        private IDisposable _inAttackProcessChangedDisposable = null;

#if UNITY_EDITOR
        private void OnValidate()
            => _animator ??= GetComponent<Animator>();
#endif

        protected override void OnEntityInitialized(Entity entity)
        {
            _inAttackProcess = entity.InAttackProcess;

            _inAttackProcessChangedDisposable = _inAttackProcess.Subscribe(OnInAttackProcess);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _inAttackProcessChangedDisposable.Dispose();
        }

        private void OnInAttackProcess(bool oldValue, bool inAttackProcess)
            => UpdateInAttackProcess(inAttackProcess);

        private void UpdateInAttackProcess(bool inAttackProcess)
            => _animator.SetBool(InAttackProcessKey, inAttackProcess);
    }
}
