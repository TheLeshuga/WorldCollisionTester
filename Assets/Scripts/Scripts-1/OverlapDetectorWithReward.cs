using UnityEngine;
using System;
using System.Collections.Generic;

public class OverlapDetectorWithReward : MonoBehaviour
{
    public event Action<float> OnCollisionDetected;

    public enum OverlapType { Capsule, Box, Sphere }; // Enum para elegir el tipo de overlap

    public List<string> layersToDetect; // Lista de nombres de capas que deseamos detectar

    public OverlapType overlapType = OverlapType.Capsule; // Tipo de overlap por defecto
    public float detectionRadius = 0.3f; // El radio de deteccion

    // Lista para almacenar los puntos de colision y sus rangos
    private List<(Vector3 point, float range)> collisionPoints = new List<(Vector3, float)>();

    private CSVManager csvManager;

    void Start()
    {
        // Obtener la referencia al script CSVManager del GameObject que lo contiene
        csvManager = GameObject.FindObjectOfType<CSVManager>();

        // Verificar si se encontró el CSVManager
        if (csvManager == null)
        {
            Debug.LogError("No CSVManager script found in the scene.");
        }
    }

    void Update()
    {
        Collider[] colliders;
        if (overlapType == OverlapType.Capsule)
        {
            // Utiliza OverlapCapsule si se selecciona esa opcion
            colliders = Physics.OverlapCapsule(transform.position - Vector3.up * detectionRadius,
                                                transform.position + Vector3.up * detectionRadius,
                                                detectionRadius);
        }
        else if (overlapType == OverlapType.Box)
        {
            // Utiliza OverlapBox si se selecciona esa opcion
            colliders = Physics.OverlapBox(transform.position, new Vector3(detectionRadius, detectionRadius, detectionRadius));
        }
        else
        {
            // Utiliza OverlapSphere si se selecciona esa opcion
            colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        }

        foreach (Collider collider in colliders)
        {
            if (layersToDetect.Contains(LayerMask.LayerToName(collider.gameObject.layer)))
            {
                Vector3 collisionPoint = collider.ClosestPoint(transform.position);
                bool foundCollision = false;

                // Verificar si el punto de colision esta dentro de algun rango almacenado
                foreach ((Vector3 point, float range) in collisionPoints)
                {
                    if (Vector3.Distance(point, collisionPoint) <= range)
                    {
                        foundCollision = true;
                        OnCollisionDetected?.Invoke(0.0f);
                        Debug.Log("Already reached position: " + collisionPoint);
                        if (csvManager != null && csvManager.enabled) csvManager.SaveVector(collisionPoint, gameObject.name, collider.gameObject.name);
                        break;
                    }
                }

                if (!foundCollision)
                {
                    // Si no se encuentra el punto de colision en ningun rango, agregarlo a la lista
                    collisionPoints.Add((collisionPoint, 1.0f));
                    if (csvManager != null  && csvManager.enabled) csvManager.SaveVector(collisionPoint, gameObject.name, collider.gameObject.name);
                    OnCollisionDetected?.Invoke(1.0f);
                    Debug.Log("Reached: " + LayerMask.LayerToName(collider.gameObject.layer) + " at position: " + collisionPoint);
                }
            }
        }

    }

    void OnDrawGizmosSelected()
    {
        if (overlapType == OverlapType.Capsule)
        {
            // Dibuja una capsula en el editor si se usa OverlapCapsule
            DrawWireCapsule(transform.position, detectionRadius, transform.up, Color.red);
        }
        else if (overlapType == OverlapType.Box)
        {
            // Dibuja un box en el editor si se usa OverlapBox
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(detectionRadius * 2, detectionRadius * 2, detectionRadius * 2));
        }
        else
        {
            // Dibuja una esfera en el editor si se usa OverlapSphere
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }

    // Funcion para dibujar una capsula en el editor usando Gizmos
    void DrawWireCapsule(Vector3 center, float radius, Vector3 direction, Color color)
    {
        Vector3 start = center + direction * radius;
        Vector3 end = center - direction * radius;

        // Dibuja los segmentos de la capsula
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

        // Dibujar la linea que atraviesa todo el centro
        DrawGizmosLine(start + direction * radius, end - direction * radius, color);
    }

    // Funcion para dibujar una linea con Gizmos
    void DrawGizmosLine(Vector3 start, Vector3 end, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(start, end);
    }
}

