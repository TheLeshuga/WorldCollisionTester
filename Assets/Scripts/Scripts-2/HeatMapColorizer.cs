using UnityEngine;
using System.Collections.Generic;

public class HeatMapColorizer : MonoBehaviour
{
    public float allowableDistance = 2f; 
    public float colorAdjuster = 0.2f;  

    private Color color = Color.blue;

    private bool setColorsDone = false;
    private float Xshift;
    private GameObject[] heatBoxes;

    private Dictionary<Vector3, GameObject> cubeDictionary = new Dictionary<Vector3, GameObject>();

    private CSVManagerHM csvManagerHM;

    void Start() {
    heatBoxes = GameObject.FindGameObjectsWithTag("heatBox");

        foreach (GameObject item in heatBoxes)
        {
            Renderer renderer = item.GetComponent<Renderer>();
            renderer.material.color = color;

            cubeDictionary.Add(item.transform.position, item);
        }

        HeatMapReader heatMapReader = FindObjectOfType<HeatMapReader>();
        if (heatMapReader != null)
        {
            Xshift = heatMapReader.Xshift;
        }
        else
        {
            Debug.LogError("Could not find the HeatMapReader component in the scene.");
        }

        csvManagerHM = GameObject.FindObjectOfType<CSVManagerHM>();

        if (csvManagerHM == null)
        {
            Debug.LogError("Could not find the CSVManagerHM component in the scene.");
        }
    }


    void Update()
    {
        if (HeatMapReader.finished && !setColorsDone)
            HeatMapReader.finished = false;
            SetColor();
    }

    void SetColor()
    {
        List<Vector3> positions = HeatMapReader.nearestPositions;
        HeatMapReader.nearestPositions = new List<Vector3>();

        if(positions != null) {
            foreach (Vector3 position in positions)
            {
                Vector3 adjustedPosition = position;
                adjustedPosition.x += Xshift;

                if (cubeDictionary.ContainsKey(adjustedPosition))
                {
                    GameObject cube = cubeDictionary[adjustedPosition];
                    Renderer renderer = cube.GetComponent<Renderer>();
                    Color currentColor = renderer.material.color;

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

                    currentColor.r = Mathf.Clamp01(currentColor.r);
                    currentColor.g = Mathf.Clamp01(currentColor.g);
                    currentColor.b = Mathf.Clamp01(currentColor.b);

                    renderer.material.color = currentColor;
                    if(csvManagerHM != null  && csvManagerHM.enabled) csvManagerHM.SaveVector(position, currentColor);
                }
            }
            setColorsDone = true;
        }
    }


}


