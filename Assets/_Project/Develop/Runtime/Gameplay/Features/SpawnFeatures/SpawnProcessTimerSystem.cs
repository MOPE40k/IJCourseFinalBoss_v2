using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpawnFeatures
{
    public class SpawnProcessTimerSystem : IInitializableSystem, IUpdatableSystem
    {
        // Runtime
        private ReactiveVariable<float> _spawnInitialTime = null;
        private ReactiveVariable<float> _spawnCurrentTime = null;

        private ReactiveVariable<bool> _inSpawnProcess = null;

        public void OnInit(Entity entity)
        {
            _spawnInitialTime = entity.SpawnInitialTime;
            _spawnCurrentTime = entity.SpawnCurrentTime;
            _inSpawnProcess = entity.InSpawnProcess;

            _spawnCurrentTime.Value = 0;

            _inSpawnProcess.Value = true;
        }

        public void OnUpdate(float deltaTime)
        {
            if (_inSpawnProcess.Value == false)
                return;

            _spawnCurrentTime.Value += deltaTime;

            if (_spawnCurrentTime.Value >= _spawnInitialTime.Value)
                _inSpawnProcess.Value = false;
        }
    }
}