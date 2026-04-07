using Assets._Project.Develop.Runtime.Configs.Gameplay.Levels;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature
{
    public class StageProviderService : IDisposable
    {
        // References
        private readonly LevelConfig _levelConfig = null;
        private readonly StagesFactory _stagesFactory = null;

        // Runtime
        private ReactiveVariable<int> _currentStageNumber = new();
        private ReactiveVariable<StageResults> _currentStageResult = new();

        private IStage _currentStage = null;

        private IDisposable _stageEndedDisposable = null;

        public StageProviderService(
            LevelConfig levelConfig,
            StagesFactory stagesFactory)
        {
            _levelConfig = levelConfig;
            _stagesFactory = stagesFactory;
        }

        // Runtime
        public IReadOnlyVariable<int> CurrentStageNumber => _currentStageNumber;
        public IReadOnlyVariable<StageResults> CurrentStageResult => _currentStageResult;

        public int StagesCount => _levelConfig.StageConfigs.Count;
        public float StageMainHeroHealth => _levelConfig.HeroHealth;
        public float PreparationStateTime => _levelConfig.PreparationStateTime;

        public bool HasNextStage()
            => CurrentStageNumber.Value < StagesCount;

        public void SwitchToNext()
        {
            if (HasNextStage() == false)
                throw new InvalidOperationException();

            if (_currentStage is not null)
                CleanupCurrent();

            _currentStageNumber.Value++;

            _currentStageResult.Value = StageResults.Uncompleted;

            _currentStage = _stagesFactory.Create(_levelConfig.StageConfigs[_currentStageNumber.Value - 1]);
        }

        public void SwitchToRandom()
        {
            if (_currentStage is not null)
                CleanupCurrent();

            _currentStageResult.Value = StageResults.Uncompleted;

            int randomStage = Random.Range(0, _levelConfig.StageConfigs.Count - 1);

            _currentStage = _stagesFactory.Create(_levelConfig.StageConfigs[randomStage]);
        }

        public void StartCurrent()
        {
            _stageEndedDisposable = _currentStage.Completed.Subscribe(OnStageCompleted);

            _currentStage.Start();
        }

        private void OnStageCompleted()
            => _currentStageResult.Value = StageResults.Completed;

        public void UpdateCurrent(float deltaTime)
            => _currentStage.Update(deltaTime);

        public void CleanupCurrent()
            => _currentStage.Cleanup();

        public void Dispose()
        {
            _currentStage?.Dispose();

            _stageEndedDisposable?.Dispose();
        }
    }
}
