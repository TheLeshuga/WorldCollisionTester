using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridMap : BaseGridMap
{
    public List<GameObject> floorObjects; // List of floor objects for grid mapping

    protected override void AwakeSpecific()
    {
        // Iterate through specific lists of GridMap
        for (int i = 0; i < floorObjects.Count; i++)
        {
            GameObject floorObject = floorObjects[i];
            GameObject lowerLeftCornerPos = lowerLeftCornerPositions[i];

            Renderer renderer = floorObject.GetComponent<Renderer>();
            int width = Mathf.RoundToInt(renderer.bounds.size.x / cellSizeList[i]);
            int height = Mathf.RoundToInt(renderer.bounds.size.z / cellSizeList[i]);

            Vector3 bottomLeft = lowerLeftCornerPos.transform.position;
            lowerLeftCornerPos.SetActive(false);

            FindPossiblePositions(width, height, bottomLeft, cellSizeList[i], gridCubeYSize[i]);
        }
    }

    public override void CreateHeatMapGrid(float Xshift) {
        debugMode = true;
        heatMapMode = true;

        // Create heatmap grid with X shift
        for (int i = 0; i < floorObjects.Count; i++)
        {
            GameObject floorObject = floorObjects[i];
            GameObject lowerLeftCornerPos = lowerLeftCornerPositions[i];

            Renderer renderer = floorObject.GetComponent<Renderer>();
            int width = Mathf.RoundToInt(renderer.bounds.size.x / cellSizeList[i]);
            int height = Mathf.RoundToInt(renderer.bounds.size.z / cellSizeList[i]);

            Vector3 bottomLeft = lowerLeftCornerPos.transform.position;

            bottomLeft += new Vector3(Xshift, 0f, 0f);

            FindPossiblePositions(width, height, bottomLeft, cellSizeList[i], gridCubeYSize[i]);
        }
    }

}

