using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilityFeatures;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class StageProcessState : State, IUpdatableState, IDisposable
    {
        // References
        private readonly StageProviderService _stageProviderService;

        // Runtime
        private IDisposable _mainHeroRegisteredDisposable = null;
        private ReactiveVariable<Abilities> _mainHeroCurrentAbility = null;

        public StageProcessState(
            StageProviderService stageProviderService,
            MainHeroHolderService mainHeroHolderService)
        {
            _stageProviderService = stageProviderService;

            _mainHeroRegisteredDisposable = mainHeroHolderService
                .HeroRegistred.Subscribe(OnHeroRegistered);
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log($"{this.GetType().Name} Enter!");

            _stageProviderService.SwitchToNext();
            _stageProviderService.StartCurrent();

            _mainHeroCurrentAbility.Value = Abilities.AirStrike;
        }

        public void Update(float deltaTime)
            => _stageProviderService.UpdateCurrent(deltaTime);

        public override void Exit()
        {
            base.Exit();

            Debug.Log($"{this.GetType().Name} Exit!");

            _stageProviderService.CleanupCurrent();
        }

        public void Dispose()
            => _mainHeroRegisteredDisposable.Dispose();

        private void OnHeroRegistered(Entity mainHero)
        {
            mainHero.MaxHealth.Value = _stageProviderService.StageMainHeroHealth;
            mainHero.CurrentHealth.Value = _stageProviderService.StageMainHeroHealth;

            _mainHeroCurrentAbility = mainHero.CurrentAbility;
        }
    }
}
