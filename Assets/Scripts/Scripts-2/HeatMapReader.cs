using UnityEngine;
using System.Collections.Generic;

public class HeatMapReader : MonoBehaviour
{
    public float readFrequency = 20f;
    public float Xshift = 50f;

    public static bool finished = false;

    private PositionData[] positionDatas;
    private List<Vector3> allPositions;
    
    public static List<Vector3> nearestPositions;

    void Start()
    {
        InvokeRepeating("ReadData", readFrequency, readFrequency);
        
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
        positionDatas = FindObjectsOfType<PositionData>();

        Vector3[] combinedPositions = CombinePositionArrays(positionDatas);

        FindNearestPositions(combinedPositions);

        ResetPositionDataArrays();
    }

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

    void FindNearestPositions(Vector3[] combinedPositions)
    {
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

        finished = true;
    }

}
