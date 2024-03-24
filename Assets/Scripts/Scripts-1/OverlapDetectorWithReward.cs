using UnityEngine;
using System;
using System.Collections.Generic;

public class OverlapDetectorWithReward : MonoBehaviour
{
    public event Action<float> OnCollisionDetected;

    public enum OverlapType { Capsule, Box, Sphere }; // Enum to choose the type of overlap

    public List<string> layersToDetect; // List of layer names we want to detect

    public OverlapType overlapType = OverlapType.Capsule; // Default overlap type
    public float detectionRadius = 0.3f; // Detection radius

    // List to store collision points and their ranges
    private List<(Vector3 point, float range)> collisionPoints = new List<(Vector3, float)>();

    private CSVManager csvManager;

    void Start()
    {
        // Get reference to the CSVManager script of the GameObject containing it
        csvManager = GameObject.FindObjectOfType<CSVManager>();

        // Check if CSVManager is found
        if (csvManager == null)
        {
            Debug.LogError("Could not find the CSVManager component in the scene.");
        }
    }

    void Update()
    {
        Collider[] colliders;
        if (overlapType == OverlapType.Capsule)
        {
            // Use OverlapCapsule if that option is selected
            colliders = Physics.OverlapCapsule(transform.position - Vector3.up * detectionRadius,
                                                transform.position + Vector3.up * detectionRadius,
                                                detectionRadius);
        }
        else if (overlapType == OverlapType.Box)
        {
            // Use OverlapBox if that option is selected
            colliders = Physics.OverlapBox(transform.position, new Vector3(detectionRadius, detectionRadius, detectionRadius));
        }
        else
        {
            // Use OverlapSphere if that option is selected
            colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        }

        foreach (Collider collider in colliders)
        {
            if (layersToDetect.Contains(LayerMask.LayerToName(collider.gameObject.layer)))
            {
                Vector3 collisionPoint = collider.ClosestPoint(transform.position);
                bool foundCollision = false;

                // Check if the collision point is within any stored range
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
                    // If the collision point is not found within any range, add it to the list
                    collisionPoints.Add((collisionPoint, 1.0f));
                    if (csvManager != null && csvManager.enabled) csvManager.SaveVector(collisionPoint, gameObject.name, collider.gameObject.name);
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
            // Draw a capsule in the editor if OverlapCapsule is used
            DrawWireCapsule(transform.position, detectionRadius, transform.up, Color.red);
        }
        else if (overlapType == OverlapType.Box)
        {
            // Draw a box in the editor if OverlapBox is used
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, new Vector3(detectionRadius * 2, detectionRadius * 2, detectionRadius * 2));
        }
        else
        {
            // Draw a sphere in the editor if OverlapSphere is used
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }

    // Function to draw a capsule in the editor using Gizmos
    void DrawWireCapsule(Vector3 center, float radius, Vector3 direction, Color color)
    {
        Vector3 start = center + direction * radius;
        Vector3 end = center - direction * radius;

        // Draw segments of the capsule
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

        // Draw the line traversing through the center
        DrawGizmosLine(start + direction * radius, end - direction * radius, color);
    }

    // Function to draw a line with Gizmos
    void DrawGizmosLine(Vector3 start, Vector3 end, Color color)
    {
        Gizmos.color = color;
        Gizmos.DrawLine(start, end);
    }
}

