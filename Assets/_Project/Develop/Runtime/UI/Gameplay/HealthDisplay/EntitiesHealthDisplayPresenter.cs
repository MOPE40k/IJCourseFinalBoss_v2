using System;
using System.Collections.Generic;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.UI.CommonViews;
using Assets._Project.Develop.Runtime.UI.Core;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.UI.Gameplay.HealthDisplay
{
    public class EntitiesHealthDisplayPresenter : IPresenter
    {
        // References
        private readonly EntitiesLifeContext _entitiesLifeContext = null;
        private readonly EnititiesHealthDisplay _entitiesHealthDisplay = null;

        private readonly GameplayPresentersFactory _gameplayPresentersFactory = null;

        private readonly ViewsFactory _viewsFactory = null;

        private readonly Dictionary<Entity, EntityHealthBarInfo> _entityToHealthBarInfo = new();

        public EntitiesHealthDisplayPresenter(
            EntitiesLifeContext entitiesLifeContext,
            EnititiesHealthDisplay enititiesHealthDisplay,
            GameplayPresentersFactory gameplayPresentersFactory,
            ViewsFactory viewsFactory)
        {
            _entitiesLifeContext = entitiesLifeContext;
            _entitiesHealthDisplay = enititiesHealthDisplay;
            _gameplayPresentersFactory = gameplayPresentersFactory;
            _viewsFactory = viewsFactory;
        }

        public void Initialize()
        {
            _entitiesLifeContext.Added += OnEntityAdded;
            _entitiesLifeContext.Released += OnEntityReleased;

            foreach (Entity entity in _entitiesLifeContext.Entities)
                OnEntityAdded(entity);
        }

        public void LateUpdateTick()
        {
            foreach (KeyValuePair<Entity, EntityHealthBarInfo> info in _entityToHealthBarInfo)
            {
                BarWithText bar = info.Value.HealthPresenter.Bar;

                Vector3 position = info.Value.HealthBarPoint.position;

                _entitiesHealthDisplay.UpdatePositionFor(bar, position);
            }
        }

        public void Dispose()
        {
            _entitiesLifeContext.Added -= OnEntityAdded;
            _entitiesLifeContext.Released -= OnEntityReleased;

            foreach (EntityHealthBarInfo info in _entityToHealthBarInfo.Values)
                DisposeFor(info);

            _entityToHealthBarInfo.Clear();
        }

        private void OnEntityAdded(Entity entity)
        {
            if (entity.TryGetHealthBarPoint(out Transform healthBarPoint))
            {
                BarWithText healthBarView = null;

                if (entity.HasComponent<IsMainHero>())
                    healthBarView = _viewsFactory.Create<BarWithText>(ViewIDs.HealthBarWithoutText);
                else
                    healthBarView = _viewsFactory.Create<BarWithText>(ViewIDs.HealthBarWithText);

                _entitiesHealthDisplay.Add(healthBarView);

                EntityHealthPresenter entityHealthPresenter
                    = _gameplayPresentersFactory.CreateEntityHealthPresenter(entity, healthBarView);

                entityHealthPresenter.Initialize();

                IDisposable removeReason = entity.IsDead.Subscribe((oldValue, isDead) =>
                {
                    if (isDead)
                        RemoveHealthBarFor(entity);
                });

                _entityToHealthBarInfo.Add(
                    entity,
                    new EntityHealthBarInfo(healthBarPoint, removeReason, entityHealthPresenter));
            }
        }

        private void OnEntityReleased(Entity entity)
        {
            if (_entityToHealthBarInfo.ContainsKey(entity))
                RemoveHealthBarFor(entity);
        }

        private void RemoveHealthBarFor(Entity entity)
        {
            EntityHealthBarInfo info = _entityToHealthBarInfo[entity];

            DisposeFor(info);

            _entityToHealthBarInfo.Remove(entity);
        }

        private void DisposeFor(EntityHealthBarInfo info)
        {
            info.RemoveReason.Dispose();

            _entitiesHealthDisplay.Remove(info.HealthPresenter.Bar);

            _viewsFactory.Release(info.HealthPresenter.Bar);

            info.HealthPresenter.Dispose();
        }

        private class EntityHealthBarInfo
        {
            public EntityHealthBarInfo(
                Transform healthBarPoint,
                IDisposable removeReason,
                EntityHealthPresenter haelthPresenter)
            {
                HealthBarPoint = healthBarPoint;
                RemoveReason = removeReason;
                HealthPresenter = haelthPresenter;
            }

            // References
            public Transform HealthBarPoint { get; } = null;
            public IDisposable RemoveReason { get; } = null;
            public EntityHealthPresenter HealthPresenter { get; } = null;
        }
    }
}