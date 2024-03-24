using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public abstract class BaseGridMap : MonoBehaviour
{
    public List<GameObject> lowerLeftCornerPositions; // List of lower left corner positions for grid cells
    public float[] cellSizeList; // List of cell sizes for each grid
    public float[] gridCubeYSize; // List of cube Y sizes for each grid

    public LayerMask obstacleLayer; // Layer mask for obstacles
    public LayerMask groundLayer; // Layer mask for ground

    public bool debugMode = false; // Debug mode flag
    protected bool heatMapMode = false; // Heatmap mode flag

    protected List<Vector3> possiblePositions = new List<Vector3>(); // List of possible positions
    protected List<Vector3> allPositions = new List<Vector3>(); // List of all positions

    protected virtual void Awake()
    {
        // Check if lists have the same number of elements
        if (!CheckListEquality(lowerLeftCornerPositions.Count, cellSizeList.Length, gridCubeYSize.Length))
        {
            Debug.LogError("The lowerLeftCornerPositions, cellSizeList, and gridCubeYSize lists must have the same amount of elements.");
            return;
        }

        // Specific logic for Awake
        AwakeSpecific();
    }

    protected virtual void AwakeSpecific(){}

    // Check equality of lengths of lists
    protected bool CheckListEquality(params int[] lengths)
    {
        return lengths.All(x => x == lengths[0]);
    }

    // Find possible positions within the grid
    protected void FindPossiblePositions(int width, int height, Vector3 bottomLeft, float cellSize, float cubeYSize)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 startPos = bottomLeft + new Vector3(x * cellSize, 0, y * cellSize);
                Vector3 endPosRight = startPos + new Vector3(cellSize, 0, 0);
                Vector3 endPosUp = startPos + new Vector3(0, 0, cellSize);
                
                if(debugMode) {
                    Debug.DrawLine(startPos, endPosRight, Color.black, 100f);
                    Debug.DrawLine(startPos, endPosUp, Color.black, 100f);
                }

                // Create a cube at current position
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = startPos + new Vector3(cellSize / 2f, cellSize / 2f, cellSize / 2f);
                cube.transform.localScale = new Vector3(cellSize, cubeYSize, cellSize);

                allPositions.Add(cube.transform.position);

                // Check if cube collides with objects in the specified layer
                Collider[] colliders = Physics.OverlapBox(cube.transform.position, cube.transform.localScale / 2f, Quaternion.identity, obstacleLayer);
                if (colliders.Length > 0)
                {
                    Renderer cubeRenderer = cube.GetComponent<Renderer>();
                    cubeRenderer.material.color = Color.red;
                }
                else
                {
                    // If no collision with obstacle layer, check collision with ground layer
                    Collider[] groundColliders = Physics.OverlapBox(cube.transform.position, cube.transform.localScale / 2f, Quaternion.identity, groundLayer);
                    if (groundColliders.Length > 0)
                    {
                        Renderer cubeRenderer = cube.GetComponent<Renderer>();
                        cubeRenderer.material.color = Color.green;
                        possiblePositions.Add(cube.transform.position);
                    }
                }

                // Destroy cube if not in debug mode
                if(!debugMode) Destroy(cube);
                // Set cube tag for heatmap mode
                if(heatMapMode) cube.tag = "heatBox";
            }
        }
    }

    // Return possible positions
    public List<Vector3> ReceivePositions()
    {
        return possiblePositions;
    }

    // Return all positions sorted
    public List<Vector3> ReceiveAllPositions()
    {
        List<Vector3> sortedPositions = allPositions.OrderBy(v => v.z).ThenBy(v => v.x).ToList();
        return sortedPositions;
    }

    // Abstract method to create heatmap grid
    public abstract void CreateHeatMapGrid(float Xshift);
}


