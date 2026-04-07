using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class RandomMovementState : State, IUpdatableState
    {
        // References
        private ReactiveVariable<Vector3> _movementDirection;
        private ReactiveVariable<Vector3> _rotationDirection;

        // Settings
        private float _cooldownBetweenDirectionGeneration;

        // Runtime
        private float _time;

        public RandomMovementState(
            Entity entity,
            float cooldownBetweenDirectionGeneration)
        {
            _movementDirection = entity.MoveDirection;
            _rotationDirection = entity.RotationDirection;

            _cooldownBetweenDirectionGeneration = cooldownBetweenDirectionGeneration;
        }

        public override void Enter()
        {
            base.Enter();

            Vector3 randomDirection = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
            _movementDirection.Value = randomDirection;
            _rotationDirection.Value = randomDirection;

            _time = 0;
        }

        public void Update(float deltaTime)
        {
            _time += deltaTime;

            if (_time >= _cooldownBetweenDirectionGeneration)
            {
                GenerateNewDirection();

                _time = 0;
            }
        }

        public override void Exit()
        {
            base.Exit();

            _movementDirection.Value = Vector3.zero;
        }

        private void GenerateNewDirection()
        {
            Vector3 inverseDirection = -_movementDirection.Value.normalized;
            Quaternion randomTurn = Quaternion.Euler(0, Random.Range(-30, 30), 0);
            Vector3 newDirection = randomTurn * inverseDirection;

            _movementDirection.Value = newDirection;
            _rotationDirection.Value = newDirection;
        }
    }
}
