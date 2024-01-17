using System;
using Collidables;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;

namespace Managers
{
    public class AgentController : Agent
    {
        public GameObject target;
        public float speedFactor;

        public override void OnEpisodeBegin()
        {
            transform.localPosition = Vector3.zero;
        }

        private void Update()
        {
            Debug.Log(MoleculeManager.Instance.GetMoleculeRegion(gameObject));
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

            GetComponent<Rigidbody2D>().AddForce(new Vector2(forceX, forceY) * Time.deltaTime * speedFactor);
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
                    break;
                case CollidableType.GolgiIn:
                    HandleMitochondrion();
                    break;
                case CollidableType.GolgiOut:
                    break;
                case CollidableType.Phospholipid:
                    break;
                case CollidableType.SmoothEr:
                    break;
                case CollidableType.RoughEr:
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
                    MoleculeManager.Instance.ConvertMolecule(gameObject,
                        MoleculeManager._moleculeTemplatesDictionary["ATP"], 1f);
                    break;
            }
        }
    }
}