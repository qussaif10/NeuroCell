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
        public LayerMask repellableLayer;

        private Rigidbody2D _targetRigidbody;
        private State _currentState = State.Active;
        private Rigidbody2D _rigidbody2D;
        private Collider2D _collider2D;
        private float currentAngle;
        private Collider2D _targetCollider2D;
        private int tracker;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _targetRigidbody = target.GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _targetCollider2D = target.GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            Wander(_targetRigidbody, ref currentAngle, 10f, 2f);
            RepelObjects(gameObject, 0.1f, 0.3f);
        }

        public override void OnEpisodeBegin()
        {
            transform.position = GetRandomTargetInBox(-8, -4, 8, 4);
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
                    HandleWalls();
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

            // EndEpisode();
        }

        private void HandleWalls()
        {
            AddReward(-1f);
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

        private void Wander(Rigidbody2D rb, ref float currentAnglee, float speed, float angleChangeRange)
        {
            if (rb.IsTouchingLayers() && !rb.IsTouchingLayers(LayerMask.NameToLayer("Organelle")))
            {
                currentAnglee += 180;
            }
            else
            {
                currentAnglee += Random.Range(-angleChangeRange, angleChangeRange);
            }

            var angleInRadians = currentAnglee * Mathf.Deg2Rad;
            var direction = new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians)).normalized;
            rb.velocity = direction * (speed * Time.deltaTime);
        }

        private static Vector2 GetRandomTargetInBox(float xmin, float ymin, float xmax, float ymax)
        {
            var randomX = Random.Range(xmin, xmax);
            var randomY = Random.Range(ymin, ymax);

            return new Vector2(randomX, randomY);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Collider2D>() != _targetCollider2D) return;
            AddReward(1f);
            EndEpisode();
        }

        private void RepelObjects(GameObject source, float repelRange, float repelForce)
        {
            var hitColliders = Physics2D.OverlapCircleAll(source.transform.position, repelRange, repellableLayer);
            ;
            foreach (var hitCollider in hitColliders)
            {
                var rb = hitCollider.GetComponent<Rigidbody2D>();
                if (rb == null) continue;
                var direction = hitCollider.transform.position - source.transform.position;
                rb.AddForce(direction.normalized * repelForce, ForceMode2D.Impulse);
            }
        }
    }

    public enum State
    {
        Idle,
        Active,
    }
}