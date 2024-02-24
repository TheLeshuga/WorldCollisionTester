using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;

public class MoveToGoalWithCollision : Agent
{   
    private float moveSpeed = 1f;
    private List<Vector3> possiblePositions = new List<Vector3>();

    public override void OnEpisodeBegin() {
        if (possiblePositions.Count == 0)
        {
            Debug.LogError("possiblePositions list is empty.");
        }

        int randomIndex = Random.Range(0, possiblePositions.Count);
        //Vector3 randomPosition = possiblePositions[randomIndex];

        transform.localPosition = possiblePositions[randomIndex];
        //new Vector3(randomPosition.x, 0, randomPosition.z);   //CAMBIAR Y
        transform.localRotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
    }

    public override void OnActionReceived(ActionBuffers actions) {
        //Debug.Log(actions.DiscreteActions[0]);
        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        transform.localPosition += new Vector3(moveX*2, 0, moveZ*2) * Time.deltaTime * moveSpeed;

        if (StepCount >= MaxStep)
        {
            Debug.Log("Reached MAX STEP");
            SetReward(-1f);
        }
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        continuousActions[0] = Input.GetAxisRaw("Horizontal");
        continuousActions[1] = Input.GetAxisRaw("Vertical");
    }

    private void OnTriggerEnter(Collider other) {

        if(other.TryGetComponent<Wall>(out Wall wall)) {
            SetReward(-0.5f);
            Debug.Log("Reached WALL");
            EndEpisode();
        }
        
    }

    public void AddRewardFromDetector(float rewardValue)
    {
        SetReward(rewardValue);
        EndEpisode();
    }

    public void ReceivePositions(List<Vector3> positions)
    {
       possiblePositions = positions;
    }

}
