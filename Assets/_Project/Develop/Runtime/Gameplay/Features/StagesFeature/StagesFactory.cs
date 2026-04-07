using Assets._Project.Develop.Runtime.Configs.Gameplay.Stages;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.Enemies;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.DataManagment.DataProviders;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature
{
    public class StagesFactory
    {
        // References
        private readonly DIContainer _container;

        public StagesFactory(DIContainer container)
            => _container = container;

        public IStage Create(StageConfig stageConfig)
            => stageConfig switch
            {
                ClearAllEnemiesStageConfig clearAllEnemiesStageConfig =>
                    new ClearAllEnemiesStage(
                        clearAllEnemiesStageConfig,
                        _container.Resolve<EnemiesFactory>(),
                        _container.Resolve<EntitiesLifeContext>(),
                        _container.Resolve<WalletService>()),

                _ => throw new ArgumentException($"Not supported {stageConfig.GetType()} type config")
            };
    }
}
