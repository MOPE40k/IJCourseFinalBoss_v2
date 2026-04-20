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

        public StateMachineBrain CreateGhostBrain(Entity entity, ITargetSelector targetSelector)
        {
            AIStateMachine movementToTargetState = CreateMovementToTargetStateMachine(entity, targetSelector);
            AttackTriggerState attackState = new AttackTriggerState(entity);

            ICompositeCondition fromMovementToAttackCondition = new CompositeCondition()
                .Add(new FuncCondition(() =>
                {
                    IReadOnlyVariable<Entity> currentTarget = entity.CurrentTarget;

                    if (currentTarget.Value is null)
                        return false;

                    Vector3 position = entity.Transform.position;
                    Vector3 targetPosition = currentTarget.Value.Transform.position;

                    float radiusAttack = entity.RadiusAreaAttack.Value;

                    return Vector3.Distance(position, targetPosition) <= radiusAttack;
                }));

            ICompositeCondition fromAttackToMovementCondition = new CompositeCondition()
                .Add(new FuncCondition(() => entity.IsDead.Value == false));

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine
                .AddState(movementToTargetState)
                .AddState(attackState)
                .AddTransition(movementToTargetState, attackState, fromMovementToAttackCondition)
                .AddTransition(attackState, movementToTargetState, fromAttackToMovementCondition);

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

        private AIStateMachine CreateMovementToTargetStateMachine(Entity entity, ITargetSelector targetSelector)
        {
            FindTargetState findTargetState = new FindTargetState(targetSelector, _entitiesLifeContext, entity);
            MovementToTargetState movementState = new MovementToTargetState(entity);

            IReadOnlyVariable<Entity> currentTarget = entity.CurrentTarget;

            ICompositeCondition fromFindTargetToMovementCondition = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget.Value is not null));

            ICompositeCondition fromMovementToFindTarget = new CompositeCondition()
                .Add(new FuncCondition(() => currentTarget.Value is null));

            AIStateMachine stateMachine = new AIStateMachine();

            stateMachine
                .AddState(findTargetState)
                .AddState(movementState)
                .AddTransition(findTargetState, movementState, fromFindTargetToMovementCondition)
                .AddTransition(movementState, findTargetState, fromMovementToFindTarget);

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
