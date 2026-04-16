using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Gameplay.Features.Attack.AreaAttack;
using Assets._Project.Develop.Runtime.Gameplay.Features.Sensors;
using Assets._Project.Develop.Runtime.Gameplay.Features.TeamsFeature;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Meta.Features.Wallet;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AbilityFeatures
{
    public class AbilitiesFactory
    {
        // Consts
        private const string AirStrikeBombPrefabPath = "Entities/AirStrikeBomb";
        private const string MinePrefabPath = "Entities/Mine";

        // References
        private readonly DIContainer _container = null;
        private readonly EntitiesFactory _entitiesFactory = null;
        private readonly MonoEntitiesFactory _monoEntitiesFactory = null;
        private readonly CollidersRegistryService _collidersRegistryService = null;
        private readonly EntitiesLifeContext _entitiesLifeContext = null;
        private readonly WalletService _walletService = null;

        public AbilitiesFactory(DIContainer container)
        {
            _container = container;

            _entitiesFactory = _container.Resolve<EntitiesFactory>();
            _monoEntitiesFactory = _container.Resolve<MonoEntitiesFactory>();
            _collidersRegistryService = _container.Resolve<CollidersRegistryService>();
            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();
            _walletService = _container.Resolve<WalletService>();
        }

        public Entity UseAbility(Entity owner)
            => owner.CurrentAbility.Value switch
            {
                Abilities.AirStrike => CreateAirStrike(owner),
                Abilities.Mine => CreateMine(owner),
                _ => throw new InvalidOperationException($"Ability: {owner.CurrentAbility.Value.ToString()} is unknown!")
            };

        public Entity CreateAirStrike(Entity owner)
        {
            Entity entity = _entitiesFactory.CreateAreaDamageEntity();

            IReadOnlyVariable<Vector3> position = owner.MousePositionOnPlane;
            IReadOnlyVariable<float> radius = owner.RadiusAreaAttack;
            IReadOnlyVariable<float> damage = owner.DamageAreaAttack;
            IReadOnlyVariable<Teams> team = owner.Team;

            _monoEntitiesFactory.Create(entity, position.Value, AirStrikeBombPrefabPath);

            entity
                .AddRadiusAreaAttack(new ReactiveVariable<float>(radius.Value))
                .AddDamageAreaAttack(new ReactiveVariable<float>(damage.Value))
                .AddTeam(new ReactiveVariable<Teams>(team.Value));

            entity
                .AddSystem(new AreaContactsDetectingSystem())
                .AddSystem(new AreaContactsEntitiesFilterSystem(_collidersRegistryService))
                .AddSystem(new AreaDealDamageOnContactAndSelfReleaseSystem());

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        public Entity CreateMine(Entity owner)
        {
            Entity entity = CreateAreaDamageEntity();

            IReadOnlyVariable<Vector3> position = owner.MousePositionOnPlane;
            IReadOnlyVariable<float> radius = owner.RadiusAreaAttack;
            IReadOnlyVariable<float> damage = owner.DamageAreaAttack;
            IReadOnlyVariable<Teams> team = owner.Team;

            _monoEntitiesFactory.Create(entity, position.Value, MinePrefabPath);

            entity
                .AddBodyContactDamage(new ReactiveVariable<float>(damage.Value))
                .AddTeam(new ReactiveVariable<Teams>(team.Value));

            entity
                .AddSystem(new BodyContactsDetectingSystem(radius.Value))
                .AddSystem(new BodyContactsEntitiesFilterSystem(_collidersRegistryService));

            _entitiesLifeContext.Add(entity);

            return entity;
        }

        private Entity CreateAreaDamageEntity()
            => _entitiesFactory.CreateAreaDamageEntity();
    }
}