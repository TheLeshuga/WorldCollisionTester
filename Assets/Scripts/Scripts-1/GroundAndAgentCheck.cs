using UnityEngine;

public class GroundAndAgentCheck : MonoBehaviour
{
    public LayerMask groundLayer;
    //public LayerMask playerLayer; // Layer de los personajes
    public bool isGrounded;

    private CapsuleCollider capsuleCollider;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();

        // Inicializar playerLayer en el método Start
        //playerLayer = LayerMask.GetMask("agent");

        // Ignorar colisiones con otros personajes
        //IgnorePlayerCollisions();
    }

    private void FixedUpdate()
    {
        // Verificar si el personaje está en el suelo
        isGrounded = Physics.CheckCapsule(
            capsuleCollider.bounds.center,
            new Vector3(capsuleCollider.bounds.center.x, capsuleCollider.bounds.min.y, capsuleCollider.bounds.center.z),
            capsuleCollider.radius * 0.9f,
            groundLayer
        );
    }
    /*
    private void IgnorePlayerCollisions()
    {
        // Obtener todos los colliders en el Layer de los personajes
        Collider[] playerColliders = Physics.OverlapSphere(transform.position, Mathf.Infinity, playerLayer);

        // Ignorar colisiones con los colliders de los otros personajes
        foreach (Collider collider in playerColliders)
        {   
            if (collider != gameObject.GetComponent<Collider>()) // Evitar ignorar la colisión con uno mismo
            {
                Physics.IgnoreCollision(GetComponent<Collider>(), collider, true);
            }
        }
    }
    */
}


