using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.AreaAttack
{
    public class AreaDealDamageOnContactAndSelfReleaseSystem : IInitializableSystem
    {
        private Entity _entity;
        private Buffer<Entity> _contacts;
        private ReactiveVariable<float> _damage;
        private ReactiveVariable<bool> _isDead = null;

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _contacts = entity.ContactEntitiesBuffer;
            _damage = entity.DamageAreaAttack;
            _isDead = entity.IsDead;

            TakeDamageToContacts();

            _isDead.Value = true;
        }

        private void TakeDamageToContacts()
        {
            for (int i = 0; i < _contacts.Count; i++)
                EntitiesHelper.TryTakeDamageFrom(_entity, _contacts.Items[i], _damage.Value);
        }
    }
}
