using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle.Views.Dead
{
    public class DeadViewParticles : DeadViewBase
    {
        [Header("References:")]
        [SerializeField] private ParticleSystem _deadEffectPrefab = null;
        [SerializeField] private Transform _effectSpawnPoint = null;

        protected override void UpdateIsDead(bool isDead)
        {
            base.UpdateIsDead(isDead);
            if (isDead)
                Instantiate(_deadEffectPrefab, _effectSpawnPoint.position, _effectSpawnPoint.rotation);
        }
    }
}