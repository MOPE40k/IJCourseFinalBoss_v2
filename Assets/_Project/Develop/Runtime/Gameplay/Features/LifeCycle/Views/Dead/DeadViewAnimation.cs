using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle.Views.Dead
{
    [RequireComponent(typeof(Animator))]
    public class DeadViewAnimation : DeadViewBase
    {
        // Consts
        private readonly int IsDeadKey = Animator.StringToHash("IsDead");

        [Header("References:")]
        [SerializeField] private Animator _animator = null;

#if UNITY_EDITOR
        private void OnValidate()
            => _animator ??= GetComponent<Animator>();
#endif
        protected override void UpdateIsDead(bool isDead)
        {
            base.UpdateIsDead(isDead);

            _animator.SetBool(IsDeadKey, isDead);
        }
    }
}