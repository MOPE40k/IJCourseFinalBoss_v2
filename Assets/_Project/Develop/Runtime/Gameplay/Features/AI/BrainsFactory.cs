using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.AI.States;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Infrastructure.DI;
using Assets._Project.Develop.Runtime.Utilities.Conditions;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.Timer;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI
{
    public class BrainsFactory
    {
        private readonly DIContainer _container;
        private readonly TimerServiceFactory _timerServiceFactory;
        private readonly AIBrainsContext _brainsContext;
        private readonly EntitiesLifeContext _entitiesLifeContext;

        public BrainsFactory(DIContainer container)
        {
            _container = container;
            _timerServiceFactory = _container.Resolve<TimerServiceFactory>();
            _brainsContext = _container.Resolve<AIBrainsContext>();
            _entitiesLifeContext = _container.Resolve<EntitiesLifeContext>();
        }

        public StateMachineBrain CreateMainHeroBrain(Entity entity)
        {
            IInputService inputService = _container.Resolve<IInputService>();

            PlayerMouseInputRotationState rotateToMousePointerState
                = new PlayerMouseInputRotationState(entity);

            AttackTriggerState attackTriggerState = new AttackTriggerState(entity);

            ICondition canAttack = entity.CanStartAttack;

            ICompositeCondition fromRotateToAttackCondition = new CompositeCondition()
                .Add(canAttack)
                .Add(new FuncCondition(() => inputService.IsFire));

            ReactiveVariable<bool> inAttackProcess = entity.InAttackProcess;

            ICompositeCondition fromAttackToRotateStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => inAttackProcess.Value == false));

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine
                .AddState(rotateToMousePointerState)
                .AddState(attackTriggerState)
                .AddTransition(rotateToMousePointerState, attackTriggerState, fromRotateToAttackCondition)
                .AddTransition(attackTriggerState, rotateToMousePointerState, fromAttackToRotateStateCondition);

            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        public StateMachineBrain CreateGhostBrain(Entity entity)
        {
            AIStateMachine stateMachine = CreateMovementToTargetStateMachine(entity);
            StateMachineBrain brain = new StateMachineBrain(stateMachine);

            _brainsContext.SetFor(entity, brain);

            return brain;
        }

        private AIStateMachine CreateRandomMovementStateMachine(Entity entity)
        {
            List<IDisposable> disposables = new List<IDisposable>();

            RandomMovementState randomMovementState = new RandomMovementState(entity, 0.5f);

            EmptyState emptyState = new EmptyState();

            TimerService movementTimer = _timerServiceFactory.Create(2f);
            disposables.Add(movementTimer);
            disposables.Add(randomMovementState.Entered.Subscribe(movementTimer.Restart));

            TimerService idleTimer = _timerServiceFactory.Create(3f);
            disposables.Add(idleTimer);
            disposables.Add(emptyState.Entered.Subscribe(idleTimer.Restart));

            FuncCondition movementTimerEndedCondition = new FuncCondition(() => movementTimer.IsOver);
            FuncCondition idleTimerEndedCondition = new FuncCondition(() => idleTimer.IsOver);

            AIStateMachine stateMachine = new AIStateMachine(disposables);

            stateMachine.AddState(randomMovementState);
            stateMachine.AddState(emptyState);

            stateMachine.AddTransition(randomMovementState, emptyState, movementTimerEndedCondition);
            stateMachine.AddTransition(emptyState, randomMovementState, idleTimerEndedCondition);

            return stateMachine;
        }

        private AIStateMachine CreateMovementToTargetStateMachine(Entity entity)
        {
            MovementToTargetState movementToTargetState = new MovementToTargetState(
                entity,
                _container.Resolve<MainHeroHolderService>());

            SelfDestroyState selfDestroyState = new SelfDestroyState(entity);

            ICompositeCondition fromMovementToSelfDestroyCondition = new CompositeCondition()
                .Add(new FuncCondition(() =>
                {
                    if (entity.CurrentTarget is null)
                        return false;

                    float distanceToTarget = Vector3.Distance(entity.CurrentTarget.Value.Transform.position, entity.Transform.position);

                    return distanceToTarget <= entity.RadiusAreaAttack.Value;
                }));

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine.AddState(movementToTargetState);
            stateMachine.AddState(selfDestroyState);

            stateMachine.AddTransition(movementToTargetState, selfDestroyState, fromMovementToSelfDestroyCondition);

            return stateMachine;
        }

        private AIStateMachine CreateAutoAttackStateMachine(Entity entity)
        {
            RotateToTargetState rotateToTargetState = new RotateToTargetState(entity);

            AttackTriggerState attackTriggerState = new AttackTriggerState(entity);

            ICondition canAttack = entity.CanStartAttack;
            Transform transform = entity.Transform;
            ReactiveVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromRotateToAttackCondition = new CompositeCondition()
                .Add(canAttack)
                .Add(new FuncCondition(() =>
                {
                    Entity target = currentTarget.Value;

                    if (target == null)
                        return false;

                    float angleToTarget = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(target.Transform.position - transform.position));
                    return angleToTarget < 3f;
                }));

            ReactiveVariable<bool> inAttackProcess = entity.InAttackProcess;

            ICompositeCondition fromAttackToRotateStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => inAttackProcess.Value == false));

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine
                .AddState(rotateToTargetState)
                .AddState(attackTriggerState)
                .AddTransition(rotateToTargetState, attackTriggerState, fromRotateToAttackCondition)
                .AddTransition(attackTriggerState, rotateToTargetState, fromAttackToRotateStateCondition);

            return stateMachine;
        }

        private AIStateMachine CreateAirStrikeAttackStateMachine(Entity entity)
        {
            IInputService inputService = _container.Resolve<IInputService>();

            PlayerMouseInputRotationState rotateToMousePointerState
                = new PlayerMouseInputRotationState(entity);

            AttackTriggerState attackTriggerState = new AttackTriggerState(entity);

            ICondition canAttack = entity.CanStartAttack;

            ICompositeCondition fromRotateToAttackCondition = new CompositeCondition()
                .Add(canAttack)
                .Add(new FuncCondition(() => inputService.IsFire));

            ReactiveVariable<bool> inAttackProcess = entity.InAttackProcess;

            ICompositeCondition fromAttackToRotateStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => inAttackProcess.Value == false));

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine
                .AddState(rotateToMousePointerState)
                .AddState(attackTriggerState)
                .AddTransition(rotateToMousePointerState, attackTriggerState, fromRotateToAttackCondition)
                .AddTransition(attackTriggerState, rotateToMousePointerState, fromAttackToRotateStateCondition);

            return stateMachine;
        }

        private AIStateMachine CreateMiningStateMachine(Entity entity)
        {
            IInputService inputService = _container.Resolve<IInputService>();

            PlayerMouseInputRotationState rotateToMousePointerState
                = new PlayerMouseInputRotationState(entity);

            AttackTriggerState mouseAttackTriggerState
                = new AttackTriggerState(
                    entity);

            ICondition canAttack = entity.CanStartAttack;

            ICompositeCondition fromRotateToAttackCondition = new CompositeCondition()
                .Add(new FuncCondition(() => inputService.IsFire))
                .Add(canAttack);

            ReactiveVariable<bool> inAttackProcess = entity.InAttackProcess;

            ICompositeCondition fromAttackToRotateStateCondition = new CompositeCondition()
                .Add(new FuncCondition(() => inAttackProcess.Value == false))
                .Add(new FuncCondition(() => inputService.IsFire == false));

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine
                .AddState(rotateToMousePointerState)
                .AddState(mouseAttackTriggerState)
                .AddTransition(rotateToMousePointerState, mouseAttackTriggerState, fromRotateToAttackCondition)
                .AddTransition(mouseAttackTriggerState, rotateToMousePointerState, fromAttackToRotateStateCondition);

            return stateMachine;
        }
    }
}
