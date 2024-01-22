using System;
using Collidables;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Managers
{
    public class AgentController : Agent
    {
        public float speedFactor = 170f;
        public LayerMask membraneLayers;
        public GameObject target;
        public LayerMask repellableLayer;

        private Rigidbody2D _rigidbody2D;
        private Collider2D _collider2D;
        private Collider2D _targetCollider2D;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _targetCollider2D = target.GetComponent<Collider2D>();
        }

        private void FixedUpdate()
        {
            RepelObjects(gameObject, 0.1f, 0.3f);
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

            var boost = _collider2D.IsTouchingLayers(membraneLayers) ? 6 : 1;

            _rigidbody2D.AddForce(new Vector2(forceX, forceY) * Time.deltaTime * speedFactor * boost);
        }

        public override void Heuristic(in ActionBuffers actionsOut)
        {
            var continuousActionsOut = actionsOut.ContinuousActions;
            continuousActionsOut[0] = Input.GetAxisRaw("Horizontal");
            continuousActionsOut[1] = Input.GetAxisRaw("Vertical");
        }
        
        private static void Wander(Rigidbody2D rb, ref float currentAnglee, float speed, float angleChangeRange)
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

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.GetComponent<Collider2D>() != _targetCollider2D) return;
            AddReward(1f);
            EndEpisode();
        } 

        private void RepelObjects(GameObject source, float repelRange, float repelForce)
        {
            var hitColliders = Physics2D.OverlapCircleAll(source.transform.position, repelRange, repellableLayer);
            
            foreach (var hitCollider in hitColliders)
            {
                var rb = hitCollider.GetComponent<Rigidbody2D>();
                if (rb == null) continue;
                var direction = hitCollider.transform.position - source.transform.position;
                rb.AddForce(direction.normalized * repelForce, ForceMode2D.Impulse);
            }
        }
    }
}