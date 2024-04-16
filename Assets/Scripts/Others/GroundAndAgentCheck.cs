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
    
}


