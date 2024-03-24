using UnityEngine;
using System.Collections.Generic;

public class HeatMapReader : MonoBehaviour
{
    public float readFrequency = 20f;   // Frequency of data reading
    public float Xshift = 50f;          // X-axis shift for positioning

    public static bool finished = false;    // Flag indicating if reading is finished

    private PositionData[] positionDatas;   // Array to store PositionData components
    private List<Vector3> allPositions;     // List to store all positions
    
    public static List<Vector3> nearestPositions; // List to store nearest positions

    void Start()
    {
        // Start periodic data reading
        InvokeRepeating("ReadData", readFrequency, readFrequency);
        
        // Get GridMap or GridMapRegulated component and initialize positions
        GridMap gridMap = GetComponent<GridMap>();
        GridMapRegulated gridMapRegulated = GetComponent<GridMapRegulated>();
        
        if (gridMap != null)
        {
            allPositions = gridMap.ReceiveAllPositions();
            gridMap.CreateHeatMapGrid(Xshift);
        }
        else if (gridMapRegulated != null)
        {
            allPositions = gridMapRegulated.ReceiveAllPositions();
            gridMapRegulated.CreateHeatMapGrid(Xshift);
        }
    }

    void ReadData()
    {
        // Find all PositionData components in the scene
        positionDatas = FindObjectsOfType<PositionData>();

        // Combine positions from all PositionData components
        Vector3[] combinedPositions = CombinePositionArrays(positionDatas);

        // Find nearest positions for combined positions
        FindNearestPositions(combinedPositions);

        // Reset position data arrays
        ResetPositionDataArrays();
    }

    // Combine position arrays from PositionData components
    Vector3[] CombinePositionArrays(PositionData[] positionDatas)
    {
        List<Vector3> combinedPositions = new List<Vector3>();

        foreach (PositionData positionData in positionDatas)
        {
            if (positionData != null && positionData.posArray != null)
            {
                combinedPositions.AddRange(positionData.posArray);
            }
        }

        return combinedPositions.ToArray();
    }

    // Reset position data arrays
    void ResetPositionDataArrays()
    {
        foreach (PositionData positionData in positionDatas)
        {
            if (positionData != null)
            {
                positionData.ResetPosArray();
            }
        }
    }

    // Find nearest positions for combined positions
    void FindNearestPositions(Vector3[] combinedPositions)
    {
        nearestPositions = new List<Vector3>();

        foreach (Vector3 combinedPos in combinedPositions)
        {
            float minDistance = Mathf.Infinity;
            Vector3 nearestPos = Vector3.zero;

            foreach (Vector3 pos in allPositions)
            {
                float distance = Vector3.Distance(combinedPos, pos);
                if (distance < minDistance)
                {
                    if (distance <= 0.5f)
                    {
                        nearestPos = pos;
                        break;
                    }
                    minDistance = distance;
                    nearestPos = pos;
                }
            }

            nearestPositions.Add(nearestPos);
        }

        finished = true; // Mark reading as finished
    }
}

