using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MainHero
{
    public class MainHeroHolderService : IInitializable, IDisposable
    {
        // Delegates
        private ReactiveEvent<Entity> _heroRegistred = new();

        // References
        private EntitiesLifeContext _entitiesLifeContext;

        // Runtime
        private Entity _mainHero;

        public MainHeroHolderService(EntitiesLifeContext entitiesLifeContext)
            => _entitiesLifeContext = entitiesLifeContext;

        public IReadOnlyEvent<Entity> HeroRegistred => _heroRegistred;

        // Runtime
        public Entity MainHero => _mainHero;

        public void Initialize()
            => _entitiesLifeContext.Added += OnEntityAdded;

        private void OnEntityAdded(Entity entity)
        {
            if (entity.HasComponent<IsMainHero>())
            {
                _entitiesLifeContext.Added -= OnEntityAdded;

                _mainHero = entity;

                _heroRegistred?.Invoke(_mainHero);
            }
        }

        public void Dispose()
            => _entitiesLifeContext.Added -= OnEntityAdded;
    }
}
