using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilityFeatures;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.AreaAttack
{
    public class AirStrikeAttackSystem : IInitializableSystem, IDisposableSystem
    {
        // References
        private readonly AbilitiesFactory _abilitiesFactory = null;

        // Runtime
        private ReactiveEvent _airStrikeAttackRequest = null;
        private Entity _entity = null;

        private IDisposable _airStrikeAttackRequestDisposable = null;

        public AirStrikeAttackSystem(AbilitiesFactory abilitiesFactory)
        {
            _abilitiesFactory = abilitiesFactory;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _airStrikeAttackRequest = entity.AirStrikeAttackRequest;

            _airStrikeAttackRequestDisposable = _airStrikeAttackRequest.Subscribe(OnAirStrikeAttackRequest);
        }

        public void OnDispose()
        {
            _airStrikeAttackRequestDisposable.Dispose();
        }

        private void OnAirStrikeAttackRequest()
            => _abilitiesFactory.CreateAirStrike(_entity);
    }
}