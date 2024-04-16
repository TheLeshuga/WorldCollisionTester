using UnityEngine;

public class OverlapDetector : MonoBehaviour
{
    public GameObject[] objectsToDetect; // Lista de GameObjects que deseamos detectar
    public float detectionRadius = 0.3f; // El radio de detección

    void Update()
    {
        // Encuentra todos los colliders dentro del radio de detección
        Collider[] colliders = Physics.OverlapCapsule(transform.position - Vector3.up * detectionRadius, 
                                                       transform.position + Vector3.up * detectionRadius, 
                                                       detectionRadius);

        foreach (Collider collider in colliders)
        {
            foreach (GameObject objectToDetect in objectsToDetect)
            {
                if (collider.gameObject == objectToDetect)
                {
                    Debug.Log("Colisión detectada con: " + objectToDetect.name);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Dibuja una cápsula en el editor para visualizar el rango de detección
        DrawWireCapsule(transform.position, detectionRadius, transform.up, Color.red);
    }

    // Función para dibujar una cápsula en el editor usando Gizmos
    void DrawWireCapsule(Vector3 center, float radius, Vector3 direction, Color color)
    {
        Vector3 start = center + direction * radius;
        Vector3 end = center - direction * radius;

        // Dibuja los segmentos de la cápsula
        DrawGizmosLine(start + Vector3.right * radius, end + Vector3.right * radius, color);
        DrawGizmosLine(start - Vector3.right * radius, end - Vector3.right * radius, color);
        DrawGizmosLine(start + Vector3.forward * radius, end + Vector3.forward * radius, color);
        DrawGizmosLine(start - Vector3.forward * radius, end - Vector3.forward * radius, color);

        Vector3 up = Quaternion.AngleAxis(90, Vector3.forward) * direction * radius;
        Vector3 forward = Quaternion.AngleAxis(90, Vector3.right) * direction * radius;

        DrawGizmosLine(start + up, end + up, color);
        DrawGizmosLine(start - up, end - up, color);
        DrawGizmosLine(start + forward, end + forward, color);
        DrawGizmosLine(start - forward, end - forward, color);

        // Dibujar la línea que atraviesa todo el centro
        DrawGizmosLine(start + direction * radius, end - direction * radius, color);
    }

    // Función para dibujar una línea con Gizmos
    void DrawGizmosLine(Vector3 start, Vector3 end, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(start, end);
    }

}






