using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using Assets._Project.Develop.Runtime.Utilities.Timer;
using CourseGameVideo.Assets._Project.Develop.Runtime.Gameplay.Features.AbilityFeatures;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class PreperationState : State, IUpdatableState, IDisposable
    {
        // References
        private readonly TimerServiceFactory _timerServiceFactory = null;

        // Runtime
        private TimerService _timerService = null;
        private ReactiveVariable<Abilities> _mainHeroCurrentAbility = null;
        private IDisposable _cooldownEndedDisposable = null;
        private IDisposable _mainHeroRegisterDisposable = null;

        public PreperationState(
            StageProviderService stageProviderService,
            TimerServiceFactory timerServiceFactory,
            MainHeroHolderService mainHeroHolderService)
        {
            _timerServiceFactory = timerServiceFactory;

            _timerService = _timerServiceFactory.Create(stageProviderService.PreparationStateTime);
            _cooldownEndedDisposable = _timerService.CooldownEnded.Subscribe(OnCooldownEnded);
            _mainHeroRegisterDisposable = mainHeroHolderService.HeroRegistred.Subscribe(OnMainHeroRegistered);
        }

        public bool TimeIsUp { get; private set; } = false;

        public override void Enter()
        {
            base.Enter();

            Debug.Log($"{this.GetType().Name} Enter!");

            TimeIsUp = false;

            _timerService.Restart();

            _mainHeroCurrentAbility.Value = Abilities.Mine;
        }

        public void Update(float deltaTime)
        { }

        public override void Exit()
        {
            base.Exit();

            Debug.Log($"{this.GetType().Name} Exit!");

            TimeIsUp = false;
        }

        private void OnCooldownEnded()
            => TimeIsUp = true;

        private void OnMainHeroRegistered(Entity entity)
            => _mainHeroCurrentAbility = entity.CurrentAbility;

        public void Dispose()
        {
            _cooldownEndedDisposable.Dispose();
            _mainHeroRegisterDisposable.Dispose();
        }
    }
}
