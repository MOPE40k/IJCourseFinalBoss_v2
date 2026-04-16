using Assets._Project.Develop.Runtime.Configs.Gameplay.Stages;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature
{
    public class ClearAllEnemiesStage : IStage
    {
        // Delegates
        private ReactiveEvent _completed = new();

        // References
        private readonly ClearAllEnemiesStageConfig _config = null;
        private readonly EnemiesFactory _enemiesFactory = null;
        private readonly EntitiesLifeContext _entitiesLifeContext = null;
        private readonly WalletService _walletService = null;

        // Runtime
        private bool _inProcess = false;

        private Dictionary<Entity, IDisposable> _spawnedEnemiesToRemoveReason = new();

        public ClearAllEnemiesStage(
            ClearAllEnemiesStageConfig config,
            EnemiesFactory enemiesFactory,
            EntitiesLifeContext entitiesLifeContext,
            WalletService walletService)
        {
            _config = config;
            _enemiesFactory = enemiesFactory;
            _entitiesLifeContext = entitiesLifeContext;
            _walletService = walletService;
        }

        // Runtime
        public IReadOnlyEvent Completed => _completed;

        public void Start()
        {
            if (_inProcess)
                throw new InvalidOperationException("Game mode alread started");

            SpawnEnemies();

            _inProcess = true;
        }

        public void Update(float deltaTime)
        {
            if (_inProcess == false)
                return;

            if (_spawnedEnemiesToRemoveReason.Count == 0)
                ProcessEnd();
        }

        public void Cleanup()
        {
            foreach (KeyValuePair<Entity, IDisposable> item in _spawnedEnemiesToRemoveReason)
            {
                item.Value.Dispose();

                _entitiesLifeContext.Release(item.Key);
            }

            _spawnedEnemiesToRemoveReason.Clear();

            _inProcess = false;
        }

        public void Dispose()
        {
            foreach (KeyValuePair<Entity, IDisposable> item in _spawnedEnemiesToRemoveReason)
                item.Value.Dispose();

            _spawnedEnemiesToRemoveReason.Clear();

            _inProcess = false;
        }

        private void ProcessEnd()
        {
            _inProcess = false;

            int reward = _config.EnemyItems.Count * _config.RewardPerEnemy;

            _walletService.Add(CurrencyTypes.Gold, reward);

            _completed.Invoke();
        }

        private void SpawnEnemies()
        {
            foreach (EnemyItemConfig enemyItemConfig in _config.EnemyItems)
                SpawnEnemy(enemyItemConfig);
        }

        private void SpawnEnemy(EnemyItemConfig enemyItemConfig)
        {
            Entity spawnedEnemy = _enemiesFactory.Create(
                GetRandomSpawnPosition(),
                enemyItemConfig.EnemyConfig);

            IDisposable removeReason = spawnedEnemy.IsDead.Subscribe((oldValue, isDead) =>
            {
                if (isDead)
                {
                    IDisposable disposable = _spawnedEnemiesToRemoveReason[spawnedEnemy];

                    disposable.Dispose();

                    _spawnedEnemiesToRemoveReason.Remove(spawnedEnemy);
                }
            });

            _spawnedEnemiesToRemoveReason.Add(spawnedEnemy, removeReason);
        }

        private Vector3 GetRandomSpawnPosition()
        {
            Vector3 rDirection = Random.insideUnitCircle.normalized;

            rDirection *= Random.Range(_config.MinRadius, _config.MaxRadius);

            return new Vector3(rDirection.x, 0f, rDirection.y);
        }
    }
}