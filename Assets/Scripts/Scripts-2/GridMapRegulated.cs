using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridMapRegulated : BaseGridMap
{
    public float[] floorWidths; // Widths of floors for regulated grid
    public float[] floorHeights; // Heights of floors for regulated grid

    protected override void AwakeSpecific()
    {
        // Check if the specific lists have the same size
        if (!CheckListEquality(floorWidths.Length, floorHeights.Length, cellSizeList.Length))
        {
            Debug.LogError("The floorWidths, floorHeights and the other lists must have the same amount of elements.");
            return;
        }

        // Iterate through specific lists of GridMapRegulated
        for (int i = 0; i < lowerLeftCornerPositions.Count; i++)
        {
            GameObject lowerLeftCornerPos = lowerLeftCornerPositions[i];

            Vector3 bottomLeft = lowerLeftCornerPos.transform.position;
            lowerLeftCornerPos.SetActive(false);

            FindPossiblePositions(Mathf.RoundToInt(floorWidths[i] / cellSizeList[i]), Mathf.RoundToInt(floorHeights[i] / cellSizeList[i]), bottomLeft, cellSizeList[i], gridCubeYSize[i]);
        }
    }

    public override void CreateHeatMapGrid(float Xshift){
        debugMode = true;
        heatMapMode = true;
        
        // Create heatmap grid with X shift
        for (int i = 0; i < lowerLeftCornerPositions.Count; i++)
        {
            GameObject lowerLeftCornerPos = lowerLeftCornerPositions[i];

            Vector3 bottomLeft = lowerLeftCornerPos.transform.position;
            
            bottomLeft += new Vector3(Xshift, 0f, 0f);

            FindPossiblePositions(Mathf.RoundToInt(floorWidths[i] / cellSizeList[i]), Mathf.RoundToInt(floorHeights[i] / cellSizeList[i]), bottomLeft, cellSizeList[i], gridCubeYSize[i]);
        }
    }
}

