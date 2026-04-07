using System;

namespace Assets._Project.Develop.Runtime.Gameplay.States
{
    public class GameplayStatesContext : IDisposable
    {
        // References
        private readonly GameplayStateMachine _gameplayStateMachine = null;

        // Runtime
        private bool _isRunning = false;

        public GameplayStatesContext(GameplayStateMachine gameplayStateMachine)
            => _gameplayStateMachine = gameplayStateMachine;

        public void Run()
        {
            _gameplayStateMachine.Enter();

            _isRunning = true;
        }

        public void Update(float deltaTime)
        {
            if (_isRunning == false)
                return;

            _gameplayStateMachine.Update(deltaTime);
        }

        public void Dispose()
        {
            _isRunning = false;

            _gameplayStateMachine.Dispose();
        }
    }
}
