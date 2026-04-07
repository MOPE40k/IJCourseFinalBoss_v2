using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.LevelsProgression;
using Assets._Project.Develop.Runtime.Meta.Features.Sessions;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using Assets._Project.Develop.Runtime.Utilities.Timer;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class GameplayStatesFactory
    {
        // References
        private readonly DIContainer _container;

        public GameplayStatesFactory(DIContainer container)
            => _container = container;

        public PreperationState CreatePreperationState()
            => new PreperationState(
                _container.Resolve<StageProviderService>(),
                _container.Resolve<TimerServiceFactory>(),
                _container.Resolve<MainHeroHolderService>());

        public StageProcessState CreateStageProcessState()
            => new StageProcessState(
                _container.Resolve<StageProviderService>(),
                _container.Resolve<MainHeroHolderService>());

        public WinState CreateWinState(GameplayInputArgs inputArgs)
            => new WinState(
                _container.Resolve<IInputService>(),
                _container.Resolve<LevelsProgressionService>(),
                inputArgs,
                _container.Resolve<PlayerDataProvider>(),
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<SessionsResultsCounterService>(),
                _container.Resolve<SceneSwitcherService>());

        public DefeatState CreateDefeatState()
            => new DefeatState(
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<PlayerDataProvider>(),
                _container.Resolve<ICoroutinesPerformer>(),
                _container.Resolve<SessionsResultsCounterService>(),
                _container.Resolve<IInputService>());

        public GameplayStateMachine CreateGameplayStateMachine(GameplayInputArgs gameplayInputArgs)
        {
            StageProviderService stageProviderService = _container.Resolve<StageProviderService>();
            MainHeroHolderService mainHeroHolderService = _container.Resolve<MainHeroHolderService>();

            GameplayStateMachine coreLoopState = CreateCoreLoopState();

            WinState winState = CreateWinState(gameplayInputArgs);
            DefeatState defeatState = CreateDefeatState();

            ICompositeCondition coreLoopToWinStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => stageProviderService.CurrentStageResult.Value == StageResults.Completed))
                .Add(new FuncCondition(() => stageProviderService.HasNextStage() == false));

            ICompositeCondition coreLoopToDefeatStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() =>
                {
                    if (mainHeroHolderService.MainHero is not null)
                        return mainHeroHolderService.MainHero.IsDead.Value;

                    return false;
                }));

            GameplayStateMachine gameplayCycle = new GameplayStateMachine();

            gameplayCycle
                .AddState(coreLoopState)
                .AddState(winState)
                .AddState(defeatState)
                .AddTransition(coreLoopState, winState, coreLoopToWinStateCondition)
                .AddTransition(coreLoopState, defeatState, coreLoopToDefeatStateCondition);

            return gameplayCycle;
        }

        public GameplayStateMachine CreateCoreLoopState()
        {
            StageProviderService stageProviderService = _container.Resolve<StageProviderService>();

            StageProcessState stageProcessState = CreateStageProcessState();
            PreperationState preperationState = CreatePreperationState();

            ICompositeCondition stageProcessToPreperationCondition = new CompositeCondition()
                .Add(new FuncCondition(() => stageProviderService.CurrentStageResult.Value == StageResults.Completed));

            ICompositeCondition preperationToStageProcessCondition = new CompositeCondition()
                .Add(new FuncCondition(() => preperationState.TimeIsUp))
                .Add(new FuncCondition(() => stageProviderService.HasNextStage()));

            GameplayStateMachine coreLoopState = new GameplayStateMachine();

            coreLoopState
                .AddState(stageProcessState)
                .AddState(preperationState)
                .AddTransition(stageProcessState, preperationState, stageProcessToPreperationCondition)
                .AddTransition(preperationState, stageProcessState, preperationToStageProcessCondition);

            return coreLoopState;
        }
    }
}
