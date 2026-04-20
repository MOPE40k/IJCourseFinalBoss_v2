using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.StagesFeature;
using Assets._Project.Develop.Runtime.Gameplay.Infrastructure;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay;
using Assets._Project.Develop.Runtime.UI.Gameplay.ResultsPopups;
using Assets._Project.Develop.Runtime.UI.Gameplay.Stages;
using Assets._Project.Develop.Runtime.Utilities.CoroutinesManagment;
using Assets._Project.Develop.Runtime.Utilities.SceneManagment;

namespace Assets._Project.Develop.Runtime.UI.Gameplay
{
    public class GameplayPresentersFactory
    {
        private readonly DIContainer _container = null;
        private readonly GameplayInputArgs _gameplayInputArgs = null;

        public GameplayPresentersFactory(
            DIContainer container,
            GameplayInputArgs gameplayInputArgs)
        {
            _container = container;
            _gameplayInputArgs = gameplayInputArgs;
        }

        public GameplayScreenPresenter CreateGameplayScreenPresenter(GameplayScreenView view)
            => new GameplayScreenPresenter(
                view,
                _container.Resolve<ProjectPresentersFactory>(),
                _container.Resolve<GameplayPresentersFactory>());

        public WinPopupPresenter CreateWinPopupPresenter(WinPopupView view)
            => new WinPopupPresenter(
                view,
                _container.Resolve<SceneSwitcherService>(),
                _container.Resolve<ICoroutinesPerformer>());

        public DefeatPopupPresenter CreateDefeatPopupPresenter(DefeatPopupView view)
            => new DefeatPopupPresenter(
                view,
                _container.Resolve<SceneSwitcherService>(),
                _gameplayInputArgs,
                _container.Resolve<ICoroutinesPerformer>());

        public StagePresenter CreateStagePresenter(IconTextView view)
            => new StagePresenter(
                view,
                _container.Resolve<StageProviderService>());

        public EntityHealthPresenter CreateEntityHealthPresenter(Entity entity, BarWithText bar)
            => new EntityHealthPresenter(entity, bar);

        public EntitiesHealthDisplayPresenter CreateEntitiesHealthDisplayPresenter(EnititiesHealthDisplay view)
            => new EntitiesHealthDisplayPresenter(
                _container.Resolve<EntitiesLifeContext>(),
                view,
                this,
                _container.Resolve<ViewsFactory>());
    }
}