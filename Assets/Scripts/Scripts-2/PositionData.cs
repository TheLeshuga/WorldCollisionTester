using UnityEngine;
using System.Collections;

public class PositionData : MonoBehaviour
{
    public int trackingFreq = 1;    // Tracking frequency in seconds
    private float timer = 0f;       // Timer to track time elapsed

    public Vector3[] posArray;      // Array to store positions

    private int arrayIt = -1;       // Iterator for position array
    private bool resettingArray = false; // Flag to indicate array reset

    void Update()
    {
        timer += Time.deltaTime;

        // Check if it's time to track position
        if (!resettingArray && timer >= trackingFreq)
        {
            TrackPosition();
        }
    }

    // Method to track position
    void TrackPosition()
    {
        timer = 0;  // Reset timer

        ArrayList auxArray = new ArrayList();   // Create auxiliary ArrayList

        if (posArray != null)
            auxArray.AddRange(posArray);    // Add existing positions to auxiliary array

        auxArray.Add(transform.position);  // Add current position to auxiliary array

        // Convert auxiliary array to Vector3 array and assign to posArray
        posArray = auxArray.ToArray(typeof(Vector3)) as Vector3[];
        arrayIt++;
        //Debug.Log(posArray[arrayIt] + "  Iteration =  " + arrayIt);
    }

    // Method to reset position array
    public void ResetPosArray()
    {
        resettingArray = true;  // Set flag to indicate array reset
        posArray = null;        // Clear position array
        arrayIt = -1;  
        resettingArray = false;
    }
}


