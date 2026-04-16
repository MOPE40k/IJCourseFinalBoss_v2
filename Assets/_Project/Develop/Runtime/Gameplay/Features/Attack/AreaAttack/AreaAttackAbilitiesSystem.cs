using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilityFeatures;
using System;
using System.Collections.Generic;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.AreaAttack
{
    public class AreaAttackAbilitiesSystem : IInitializableSystem, IDisposableSystem
    {
        private ReactiveVariable<Dictionary<Abilities, ReactiveEvent>> _abilitiesList = null;
        private ReactiveVariable<Abilities> _currentAbility = null;

        private ReactiveEvent _startAttackEvent = null;

        private IDisposable _startAttackDisposable = null;

        public void OnInit(Entity entity)
        {
            _abilitiesList = entity.AbilitiesList;
            _currentAbility = entity.CurrentAbility;

            _startAttackEvent = entity.StartAttackEvent;

            _startAttackDisposable = _startAttackEvent.Subscribe(OnStartAttack);
        }

        public void OnDispose()
            => _startAttackDisposable.Dispose();

        private void OnStartAttack()
            => _abilitiesList.Value[_currentAbility.Value].Invoke();
    }
}