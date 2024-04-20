using UnityEngine;
using System.Collections.Generic;

public class HeatMapColorizer : MonoBehaviour
{
    public float colorAdjuster = 0.2f;    // Amount to adjust color by

    private Color color = Color.blue;     // Base color for heat map visualization

    private bool setColorsDone = false;   // Flag to check if color adjustment is done
    private float Xshift;                  // Shift in X-coordinate for positioning

    private GameObject[] heatBoxes;       // Array to store heat boxes

    private Dictionary<Vector3, GameObject> cubeDictionary = new Dictionary<Vector3, GameObject>(); // Dictionary to map positions to cubes

    private CSVManagerHM csvManagerHM;     // Reference to CSVManagerHM for saving data

    void Start() {
        heatBoxes = GameObject.FindGameObjectsWithTag("heatBox");

        // Assign color and populate cube dictionary
        foreach (GameObject item in heatBoxes)
        {
            Renderer renderer = item.GetComponent<Renderer>();
            renderer.material.color = color;

            cubeDictionary.Add(item.transform.position, item);
        }

        // Get Xshift from HeatMapReader if available
        HeatMapReader heatMapReader = FindObjectOfType<HeatMapReader>();
        if (heatMapReader != null)
        {
            Xshift = heatMapReader.Xshift;
        }
        else
        {
            Debug.LogError("Could not find the HeatMapReader component in the scene.");
        }

        // Find CSVManagerHM component
        csvManagerHM = GameObject.FindObjectOfType<CSVManagerHM>();

        if (csvManagerHM == null)
        {
            Debug.LogWarning("Could not find the CSVManagerHM component in the scene.");
        }
    }


    void Update()
    {
        // Check if heatmap data processing is finished and colors are not set yet
        if (HeatMapReader.finished)
        {
            HeatMapReader.finished = false; // Reset finished flag
            SetColor(); // Set colors based on heatmap data
        }
    }

    // Method to adjust colors based on heatmap data
    void SetColor()
    {
        List<Vector3> positions = HeatMapReader.nearestPositions;
        HeatMapReader.nearestPositions = new List<Vector3>(); // Reset nearest positions

        if(positions != null) {
            foreach (Vector3 position in positions)
            {
                Vector3 adjustedPosition = position;
                adjustedPosition.x += Xshift; // Adjust position based on Xshift

                if (cubeDictionary.ContainsKey(adjustedPosition))
                {
                    GameObject cube = cubeDictionary[adjustedPosition];
                    Renderer renderer = cube.GetComponent<Renderer>();
                    Color currentColor = renderer.material.color;

                    // Adjust color based on current values
                    if (currentColor.g < 1f && currentColor.b != 0f)
                    {
                        currentColor.g += colorAdjuster;
                        currentColor.b -= colorAdjuster;
                    }
                    else
                    {
                        currentColor.r += colorAdjuster;
                        currentColor.g -= colorAdjuster;
                    }

                    // Clamp color values between 0 and 1
                    currentColor.r = Mathf.Clamp01(currentColor.r);
                    currentColor.g = Mathf.Clamp01(currentColor.g);
                    currentColor.b = Mathf.Clamp01(currentColor.b);

                    renderer.material.color = currentColor;

                    // Save position and color data to CSV
                    if(csvManagerHM != null  && csvManagerHM.enabled) csvManagerHM.SaveVector(position, currentColor);
                }
            }
        }
    }
}



