using UnityEngine;

public class CloneAgent : MonoBehaviour
{
    public GameObject agentGameObject; // Referencia al GameObject original que se quiere clonar
    public int numClones = 0;
    public bool ignoreAgentColliders = true; // Opción para determinar si se deben ignorar las colisiones con los agentes
    public string agentLayerName = "agent"; // Nombre del layer de los agentes (por defecto "agent")

    private void Start()
    {
        if (agentGameObject != null)
        {
            for (int i = 0; i < numClones; i++)
            {
                GameObject clonedObject = Instantiate(agentGameObject); // Crea una copia del objeto original
                
                // Ignorar colisiones con otros GameObjects que tengan la capa señalada
                if (ignoreAgentColliders)
                {
                    IgnoreAgentCollisions(clonedObject);
                }
            }

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
        // Obtener todos los colliders en el Layer de los agentes
        LayerMask agentLayer = LayerMask.GetMask(agentLayerName);
        Collider[] agentColliders = Physics.OverlapSphere(clonedObject.transform.position, Mathf.Infinity, agentLayer);

        // Ignorar colisiones con los colliders de los otros agentes
        foreach (Collider collider in agentColliders)
        {
            if (collider != clonedObject.GetComponent<Collider>()) // Evitar ignorar la colisión con uno mismo
            {
                Physics.IgnoreCollision(clonedObject.GetComponent<Collider>(), collider, true);
            }
        }
    }
}



