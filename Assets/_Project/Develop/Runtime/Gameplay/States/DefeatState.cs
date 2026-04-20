using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Meta.Features.Sessions;
using Assets._Project.Develop.Runtime.UI.Gameplay;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class DefeatState : EndGameState, IUpdatableState
    {
        // References
        private readonly PlayerDataProvider _playerDataProvider = null;
        private readonly ICoroutinesPerformer _coroutinesPerformer = null;
        private readonly SessionsResultsCounterService _sessionsResultsCounterService = null;
        private readonly GameplayPopupService _gameplayPopupService = null;

        public DefeatState(
            PlayerDataProvider playerDataProvider,
            ICoroutinesPerformer coroutinesPerformer,
            SessionsResultsCounterService sessionsResultsCounterService,
            IInputService inputService,
            GameplayPopupService gameplayPopupService) : base(inputService)
        {
            _playerDataProvider = playerDataProvider;
            _coroutinesPerformer = coroutinesPerformer;
            _sessionsResultsCounterService = sessionsResultsCounterService;
            _gameplayPopupService = gameplayPopupService;
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log($"{this.GetType().Name} Enter!");

            _sessionsResultsCounterService.Add(SessionEndConditionTypes.Defeat);

            _coroutinesPerformer.StartPerform(_playerDataProvider.SaveAsync());

            // _coroutinesPerformer.StartPerform(_sceneSwitcherService.ProcessSwitchTo(Scenes.MainMenu));
            _gameplayPopupService.OpenDefeatPopup();
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
