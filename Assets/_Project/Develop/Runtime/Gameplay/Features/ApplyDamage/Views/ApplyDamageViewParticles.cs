using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage.Views
{
    public class ApplyDamageViewParticles : ApplyDamageViewBase
    {
        [Header("References:")]
        [SerializeField] private ParticleSystem _applyDamageEffectPrefab = null;
        [SerializeField] private Transform _effectSpawnPoint = null;

        protected override void OnTakeDamage(float damage)
        {
            base.OnTakeDamage(damage);

            Instantiate(_applyDamageEffectPrefab, _effectSpawnPoint.position, _effectSpawnPoint.rotation);
        }
    }
}
