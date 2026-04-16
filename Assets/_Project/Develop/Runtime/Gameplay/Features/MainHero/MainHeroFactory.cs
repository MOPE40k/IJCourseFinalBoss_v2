using System;
using System.Collections.Generic;
using Assets._Project.Develop.Runtime.Configs.Gameplay.Entities;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.AreaAttack;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.ConfigsManagment;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Gameplay.Features.AbilityFeatures;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.MainHero
{
    public class MainHeroFactory
    {
        // References
        private readonly DIContainer _container;

        private readonly EntitiesFactory _entitiesFactory;
        private readonly AbilitiesFactory _abilitiesFactory = null;
        private readonly BrainsFactory _brainsFactory;
        private readonly ConfigsProviderService _configsProviderService;
        private readonly EntitiesLifeContext _entitiesLifeContext;

        public MainHeroFactory(DIContainer container)
        {
            _container = container;

            _entitiesFactory = _container.Resolve<EntitiesFactory>();
            _abilitiesFactory = _container.Resolve<AbilitiesFactory>();
            _brainsFactory = _container.Resolve<BrainsFactory>();
            _configsProviderService = _container.Resolve<ConfigsProviderService>();
            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();
        }

        public Entity Create(Vector3 position)
        {
            HeroConfig config = _configsProviderService.GetConfig<HeroConfig>();

            Entity entity = _entitiesFactory.CreateHero(position, config);

            entity
                .AddIsMainHero()
                .AddTeam(new ReactiveVariable<Teams>(Teams.MainHero))
                .AddAirStrikeAttackRequest()
                .AddMiningRequest()
                .AddAbilitiesList(new ReactiveVariable<Dictionary<Abilities, ReactiveEvent>>(AbilitiesListFiller(entity)))
                .AddCurrentAbility(new ReactiveVariable<Abilities>(Abilities.AirStrike));

            entity
                .AddSystem(new AreaAttackAbilitiesSystem())
                .AddSystem(new AirStrikeAttackSystem(_abilitiesFactory))
                .AddSystem(new MiningSystem(_abilitiesFactory, _container.Resolve<WalletService>(), config));

            _brainsFactory.CreateMainHeroBrain(entity);

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        private Dictionary<Abilities, ReactiveEvent> AbilitiesListFiller(Entity owner)
        {
            Dictionary<Abilities, ReactiveEvent> abilityToEvent = new();

            foreach (Abilities ability in Enum.GetValues(typeof(Abilities)))
            {
                if (ability is Abilities.AirStrike)
                    abilityToEvent.Add(ability, owner.AirStrikeAttackRequest);
                else if (ability is Abilities.Mine)
                    abilityToEvent.Add(ability, owner.MiningRequest);
                else
                    throw new InvalidOperationException($"Ability: {owner.CurrentAbility.Value.ToString()} is unknown!");
            }

            return abilityToEvent;
        }
    }
}
