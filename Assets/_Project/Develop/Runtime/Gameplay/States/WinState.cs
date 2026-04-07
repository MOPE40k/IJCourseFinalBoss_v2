using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Meta.Features.LevelsProgression;
using Assets._Project.Develop.Runtime.Meta.Features.Sessions;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class WinState : EndGameState, IUpdatableState
    {
        private readonly LevelsProgressionService _levelsProgressionService = null;
        private readonly GameplayInputArgs _gameplayInputArgs = null;
        private readonly PlayerDataProvider _playerDataProvider = null;
        private readonly ICoroutinesPerformer _coroutinesPerformer = null;
        private readonly SessionsResultsCounterService _sessionsResultsCounterService = null;
        private readonly SceneSwitcherService _sceneSwitcherService = null;

        public WinState(
            IInputService inputService,
            LevelsProgressionService levelsProgressionService,
            GameplayInputArgs gameplayInputArgs,
            PlayerDataProvider playerDataProvider,
            ICoroutinesPerformer coroutinesPerformer,
            SessionsResultsCounterService sessionsResultsCounterService,
            SceneSwitcherService sceneSwitcherService) : base(inputService)
        {
            _levelsProgressionService = levelsProgressionService;
            _gameplayInputArgs = gameplayInputArgs;
            _playerDataProvider = playerDataProvider;
            _coroutinesPerformer = coroutinesPerformer;
            _sessionsResultsCounterService = sessionsResultsCounterService;
            _sceneSwitcherService = sceneSwitcherService;
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log($"{this.GetType().Name} Enter!");

            // _levelsProgressionService.AddLevelToCompleted(_gameplayInputArgs.LevelNumber);

            _sessionsResultsCounterService.Add(SessionEndConditionTypes.Win);

            _coroutinesPerformer.StartPerform(_playerDataProvider.SaveAsync());

            _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
        }

        public void Update(float deltaTime)
        { }

        public override void Exit()
        {
            base.Exit();

            Debug.Log($"{this.GetType().Name} Exit!");
        }
    }
}
