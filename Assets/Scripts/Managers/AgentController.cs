using System;
using Collidables;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class AgentController : Agent
    {
        public float speedFactor;
        public LayerMask layers;
        public GameObject target;

        private Rigidbody2D _targetRigidbody;
        // private State _currentState = State.Active;
        private Rigidbody2D _rigidbody2D;
        private Collider2D _collider2D;
        private Vector2 _randomDirection;
        private const float ChangeDirectionInterval = 0.5f;
        private float _timeSinceLastDirectionChange;
        private const float Speed = 0.5f;
        private float _movementRadius = 4.0f;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _targetRigidbody = target.GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            ChangeDirection();
        }

        private void Update()
        {
            _timeSinceLastDirectionChange += Time.deltaTime;
    
            if (_timeSinceLastDirectionChange >= ChangeDirectionInterval) {
                ChangeDirection();
                _timeSinceLastDirectionChange = 0.0f;
            }

            MoveMolecule();
        }

        public override void OnEpisodeBegin()
        {
            transform.position = GetRandomTargetInCircle(-3.86f);
            target.transform.position = GetRandomTargetInCircle(-3.86f);
        }
        
        public override void CollectObservations(VectorSensor sensor)
        {
            var position = transform.localPosition;
            var targetPosition = target.transform.localPosition;
            sensor.AddObservation(new Vector2(position.x, position.y));
            sensor.AddObservation(new Vector2(targetPosition.x, targetPosition.y));
        }

        public override void OnActionReceived(ActionBuffers actions)
        {
            var forceX = actions.ContinuousActions[0];
            var forceY = actions.ContinuousActions[1];

            var boost = _collider2D.IsTouchingLayers(layers) ? 6 : 1;
            
            _rigidbody2D.AddForce(new Vector2(forceX, forceY) * Time.deltaTime * speedFactor * boost);
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;
            continuousActionsOut[0] = Input.GetAxisRaw("Horizontal");
            continuousActionsOut[1] = Input.GetAxisRaw("Vertical");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var collidable = other.gameObject.GetComponent<ICollidable>().Type;

            switch (collidable)
            {
                case CollidableType.Nucleus:
                    break;
                case CollidableType.Nucleolus:
                    break;
                case CollidableType.Mitochondrion:
                    HandleMitochondrion();
                    break;
                case CollidableType.GolgiIn:
                    break;
                case CollidableType.GolgiOut:
                    break;
                case CollidableType.Phospholipid:
                    break;
                case CollidableType.SmoothEr:
                    break;
                case CollidableType.RoughEr:
                    break;
                case CollidableType.Wall:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void HandleMitochondrion()
        {
            switch (gameObject.tag)
            {
                case "Glucose":
                    // MoleculeManager.Instance.ConvertMolecule(gameObject,
                    //     MoleculeManager._moleculeTemplatesDictionary["ATP"], 0.1f);
                    break;
            }

            EndEpisode();
        }

        private static Vector2 GetRandomTargetInCircle(float r)
        {
            const int maxAttempts = 500;
            for (var i = 0; i < maxAttempts; i++)
            {
                var randomPoint = Random.insideUnitCircle * r;
                var collider = Physics2D.OverlapPoint(randomPoint);

                if (collider == null)
                {
                    return randomPoint;
                }
            }

            Debug.LogWarning("Could not find a non-overlapping point within 500 attempts, defaulting to origin.");
            return Vector2.zero;
            
        }
        
        private void ChangeDirection() {
            _randomDirection = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            ValidateDirection();
        }

        private void MoveMolecule() {
            Vector2 newPosition = _rigidbody2D.position + _randomDirection * Speed * Time.deltaTime;
            if (IsWithinBounds(newPosition)) {
                _rigidbody2D.MovePosition(newPosition);
            } else {
                ChangeDirection();
            }
        }

        private bool IsWithinBounds(Vector2 position) {
            return position.sqrMagnitude <= _movementRadius * _movementRadius;
        }

        private void ValidateDirection() {
            Vector2 testPosition = _rigidbody2D.position + _randomDirection * Speed;
            if (!IsWithinBounds(testPosition)) {
                _randomDirection = -_randomDirection;
            }
        }
    }

    public enum State
    {
        Idle,
        Active,
    }
}