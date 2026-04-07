using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using CourseGameVideo.Assets._Project.Develop.Runtime.Gameplay.Features.AbilityFeatures;
using System;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.AreaAttack
{
    public class AreaAttackAbilitiesSystem : IInitializableSystem, IDisposableSystem
    {
        private readonly EntitiesFactory _entitiesFactory = null;
        private readonly HeroConfig _heroConfig = null;
        private readonly WalletService _walletService = null;

        private ReactiveEvent _startAttackEvent = null;

        private Entity _entity;

        private ReactiveVariable<Abilities> _currentAbility = null;

        private ReactiveVariable<Vector3> _mousePositionOnPlane = null;

        private ReactiveVariable<float> _damage = null;
        private ReactiveVariable<float> _radius = null;

        private int _miningCost = 0;

        private IDisposable _startAttackDisposable = null;

        public AreaAttackAbilitiesSystem(
            EntitiesFactory entitiesFactory,
            HeroConfig heroConfig,
            WalletService walletService)
        {
            _entitiesFactory = entitiesFactory;
            _heroConfig = heroConfig;
            _walletService = walletService;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;

            _currentAbility = entity.CurrentAbility;

            _mousePositionOnPlane = entity.MousePositionOnPlane;

            _startAttackEvent = entity.StartAttackEvent;

            _damage = entity.DamageAreaAttack;

            _radius = entity.RadiusAreaAttack;

            _miningCost = _heroConfig.CostPerMine;

            _startAttackDisposable = _startAttackEvent.Subscribe(OnStartAttack);
        }

        public void OnDispose()
            => _startAttackDisposable.Dispose();

        private void OnStartAttack()
        {
            if (_currentAbility.Value == Abilities.AirStrike)
                AirStrikeAttack();
            else if (_currentAbility.Value == Abilities.Mine)
                MiningAttack();
            else
                throw new InvalidOperationException($"Ability: {_currentAbility.Value.ToString()} is unknown!");
        }

        private void AirStrikeAttack()
            => _entitiesFactory.CreateAirStrike(
                    _mousePositionOnPlane.Value,
                    _radius.Value,
                    _damage.Value,
                    _entity);

        private void MiningAttack()
        {
            if (_walletService.Enough(CurrencyTypes.Gold, _miningCost))
            {
                _entitiesFactory.CreateMine(
                    _mousePositionOnPlane.Value,
                    _radius.Value,
                    _damage.Value,
                    _entity);

                _walletService.Spend(CurrencyTypes.Gold, _miningCost);
            }
            else
            {
                Debug.Log($"Not enough {CurrencyTypes.Gold}");
            }
        }
    }
}