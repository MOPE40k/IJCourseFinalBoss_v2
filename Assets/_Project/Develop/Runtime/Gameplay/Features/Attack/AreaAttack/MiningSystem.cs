using System;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilityFeatures;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Attack.AreaAttack
{
    public class MiningSystem : IInitializableSystem, IDisposableSystem
    {
        // References
        private readonly AbilitiesFactory _abilitiesFactory = null;
        private readonly WalletService _walletService = null;

        // Runtime
        private ReactiveEvent _miningRequest = null;
        private int _miningCost = 0;
        private Entity _entity = null;

        private IDisposable _miningRequestDisposable = null;

        public MiningSystem(
            AbilitiesFactory abilitiesFactory,
            WalletService walletService,
            HeroConfig heroConfig)
        {
            _abilitiesFactory = abilitiesFactory;
            _walletService = walletService;
            _miningCost = heroConfig.CostPerMine;
        }

        public void OnInit(Entity entity)
        {
            _entity = entity;
            _miningRequest = entity.MiningRequest;

            _miningRequestDisposable = _miningRequest.Subscribe(OnMiningRequest);
        }

        public void OnDispose()
            => _miningRequestDisposable.Dispose();

        private void OnMiningRequest()
        {
            if (_walletService.Enough(CurrencyTypes.Gold, _miningCost))
            {
                _abilitiesFactory.CreateMine(_entity);

                _walletService.Spend(CurrencyTypes.Gold, _miningCost);
            }
            else
            {
                Debug.Log($"Not enough {CurrencyTypes.Gold}");
            }
        }
    }
}