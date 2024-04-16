using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoal : Agent
{   
    [SerializeField] private Transform targetTransform;
    private float moveSpeed = 1f;

    public override void OnEpisodeBegin() {
        transform.localPosition = new Vector3(Random.Range(-11.2f, -6.6f), 0, Random.Range(12.5f, 16.5f));
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        //Debug.Log(actions.DiscreteActions[0]);
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX*2, 0, moveZ*2) * Time.deltaTime * moveSpeed;

        if (StepCount >= MaxStep)
        {
            Debug.Log("Reached MAX STEP");
            SetReward(-0.5f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent<Goal>(out Goal goal)) {
            SetReward(1f);
            Debug.Log("Reached GOAL");
            targetTransform.localPosition = new Vector3(Random.Range(-1.7f, -5.1f), 0, Random.Range(12.2f, 15.5f));
            EndEpisode();
        }

        if(other.TryGetComponent<Wall>(out Wall wall)) {
            SetReward(-1f);
            Debug.Log("Reached WALL");
            EndEpisode();
        }
        
    }
    
}
