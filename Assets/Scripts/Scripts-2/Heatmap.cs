using UnityEngine;

public class Heatmap : MonoBehaviour
{
    public int resolution = 100;
    public float size = 10f;
    public float decayRate = 0.9f;
    public GameObject[] agents;

    private Texture2D heatmapTexture;
    private Color[] heatmapColors;

    void Start()
    {
        InitializeHeatmapTexture();
    }

    void Update()
    {
        UpdateHeatmap();
    }

    void InitializeHeatmapTexture()
    {
        heatmapTexture = new Texture2D(resolution, resolution);
        heatmapColors = new Color[resolution * resolution];
        GetComponent<Renderer>().material.mainTexture = heatmapTexture;
    }

    void UpdateHeatmap()
    {
        // Reset heatmap
        for (int i = 0; i < heatmapColors.Length; i++)
        {
            heatmapColors[i] *= decayRate;
        }

        // Update heatmap with agent positions
        foreach (GameObject agent in agents)
        {
            Vector3 position = agent.transform.position;
            int x = Mathf.FloorToInt((position.x / size + 0.5f) * resolution);
            int y = Mathf.FloorToInt((position.z / size + 0.5f) * resolution);

            if (x >= 0 && x < resolution && y >= 0 && y < resolution)
            {
                int index = x + y * resolution;
                heatmapColors[index] += Color.red;
            }
        }

        // Apply colors to texture
        heatmapTexture.SetPixels(heatmapColors);
        heatmapTexture.Apply();
    }
}

