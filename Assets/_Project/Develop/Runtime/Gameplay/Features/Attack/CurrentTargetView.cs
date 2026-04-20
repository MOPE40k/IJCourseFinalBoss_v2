using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack
{
    public class CurrentTargetView : EntityView
    {
        [Header("References:")]
        [SerializeField] private ParticleSystem _highlightPrefab = null;

        // Runtime
        private ParticleSystem _highlight = null;
        private ReactiveVariable<Entity> _currentTarget = null;

        private Transform _currentTargetTransform = null;

        private IDisposable _currentTargetChangedDisposable = null;

        private void LateUpdate()
        {
            if (_currentTargetTransform == null)
                return;

            _highlight.transform.position = _currentTargetTransform.position;
        }

        protected override void OnEntityInitialized(Entity entity)
        {
            _currentTarget = entity.CurrentTarget;

            _highlight = Instantiate(_highlightPrefab);

            _currentTargetChangedDisposable = _currentTarget.Subscribe(OnCurrentTargetChanged);

            UpdateHighlightFor(_currentTarget.Value);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _currentTargetChangedDisposable.Dispose();

            Destroy(_highlight.gameObject);
        }

        private void OnCurrentTargetChanged(Entity oldValue, Entity newTarget)
            => UpdateHighlightFor(newTarget);

        private void UpdateHighlightFor(Entity newTarget)
        {
            if (newTarget is null)
            {
                _highlight.gameObject.SetActive(false);

                _currentTargetTransform = null;

                return;
            }

            _highlight.gameObject.SetActive(true);

            _currentTargetTransform = newTarget.Transform;
        }

    }
}
