using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpawnFeatures
{
    [RequireComponent(typeof(Animator))]
    public class SpawnProcessView : EntityView
    {
        // Consts
        private readonly int InSpawnProcessKey = Animator.StringToHash("IsSpawnProcess");

        [Header("References:")]
        [SerializeField] private Animator _animator = null;
        [SerializeField] private ParticleSystem _spawnProcessParticles = null;

        // Runtime
        private Transform _entityTransform = null;
        private Collider _entityBodyCollider = null;
        private ReactiveVariable<bool> _inSpawnProcess = null;

        private IDisposable _inSpawnProcessChangedDisposable = null;

#if UNITY_EDITOR
        private void OnValidate()
        {
            _animator ??= GetComponent<Animator>();
        }
#endif

        protected override void OnEntityInitialized(Entity entity)
        {
            _entityTransform = entity.Transform;
            _entityBodyCollider = entity.BodyCollider;
            _inSpawnProcess = entity.InSpawnProcess;

            _inSpawnProcessChangedDisposable = _inSpawnProcess.Subscribe(OnSpawnProcessChanged);

            UpdateInSpawnProcessKey(_inSpawnProcess.Value);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _inSpawnProcessChangedDisposable.Dispose();
        }

        private void OnSpawnProcessChanged(bool oldValue, bool inSpawnProcess)
            => UpdateInSpawnProcessKey(inSpawnProcess);

        private void UpdateInSpawnProcessKey(bool inSpawnProcess)
        {
            _animator.SetBool(InSpawnProcessKey, inSpawnProcess);

            if (inSpawnProcess)
            {
                _entityBodyCollider.enabled = false;

                Instantiate(_spawnProcessParticles, _entityTransform.position, Quaternion.identity);
            }

            if (inSpawnProcess == false)
                _entityBodyCollider.enabled = true;
        }
    }
}