using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class AgentController : Agent
{
    public GameObject target;
    public float speedFactor;

    public override void OnEpisodeBegin()
    {
        transform.localPosition = Vector3.zero;
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
        if (!other.gameObject.CompareTag("wall"))
        {
            SetReward(1f);
            EndEpisode();
        }
        else
        {
            SetReward(-1f);
            EndEpisode();
        }
    }
}