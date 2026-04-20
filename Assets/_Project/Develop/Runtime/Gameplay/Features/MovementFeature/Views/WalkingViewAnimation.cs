using UnityEngine;

namespace CourseGameVideo.Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature.Views
{
    [RequireComponent(typeof(Animator))]
    public class WalkingViewAnimation : WalkingViewBase
    {
        // Consts
        private readonly int IsMovingKey = Animator.StringToHash("IsWalking");

        [Header("References:")]
        [SerializeField] private Animator _animator = null;

#if UNITY_EDITOR
        private void OnValidate()
            => _animator ??= GetComponent<Animator>();
#endif

        protected override void UpdateIsMoving(bool isMoving)
            => _animator.SetBool(IsMovingKey, isMoving);
    }
}