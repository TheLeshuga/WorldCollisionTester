using UnityEngine;

public class CloneAgent : MonoBehaviour
{
    public GameObject agentGameObject; // Reference to the original agent to be cloned
    public int numClones = 0;
    public bool ignoreAgentColliders = true; // Option to determine whether to ignore collisions with agents
    public string agentLayerName = "agent"; // Name of the layer for agents (default "agent")

    private void Start()
    {
        if (agentGameObject != null)
        {
            // Create specified number of clones
            for (int i = 1; i <= numClones; i++)
            {
                GameObject clonedObject = Instantiate(agentGameObject); // Create a copy of the original object
                clonedObject.name = agentGameObject.name + "_" + i; // Assign a unique name based on the original object's name and instance number
                
                // Ignore collisions with other GameObjects that have the specified layer
                if (ignoreAgentColliders)
                {
                    IgnoreAgentCollisions(clonedObject);
                }
            }

            // Ignore collisions for the original agent GameObject as well
            if (ignoreAgentColliders)
            {
                IgnoreAgentCollisions(agentGameObject);
            }
        }
        else
        {
            Debug.LogError("A GameObject has not been assigned to clone.");
        }
    }

    private void IgnoreAgentCollisions(GameObject clonedObject)
    {
        // Get all colliders in the agent Layer
        LayerMask agentLayer = LayerMask.GetMask(agentLayerName);
        Collider[] agentColliders = Physics.OverlapSphere(clonedObject.transform.position, Mathf.Infinity, agentLayer);

        // Ignore collisions with colliders of other agents
        foreach (Collider collider in agentColliders)
        {
            if (collider != clonedObject.GetComponent<Collider>()) // Avoid ignoring collision with self
            {
                Physics.IgnoreCollision(clonedObject.GetComponent<Collider>(), collider, true);
            }
        }
    }
}





