using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GridMapRegulated : MonoBehaviour
{
    public List<GameObject> lowerLeftCornerPositions;
    public float[] floorWidths;
    public float[] floorHeights;

    public float[] cellSizeList;
    public float[] gridCubeYSize;

    public LayerMask obstacleLayer; // Layer para obstaculos
    public LayerMask groundLayer; // Layer para el suelo

    public bool debugMode = false;
    private bool heatMapMode = false;

    private List<Vector3> possiblePositions = new List<Vector3>();
    private List<Vector3> allPositions = new List<Vector3>();

    private void Awake()
    {
        // Verificar si las listas tienen el mismo número de elementos
        if (lowerLeftCornerPositions.Count != floorWidths.Length || lowerLeftCornerPositions.Count != floorHeights.Length)
        {
            Debug.LogError("The lowerLeftCornerPositions, floorWidths, and floorHeights lists must have the same amount of elements.");
            return;
        }

        if (lowerLeftCornerPositions.Count != gridCubeYSize.Length || lowerLeftCornerPositions.Count != cellSizeList.Length)
        {
            Debug.LogError("The lowerLeftCornerPositions, gridCubeYSize, and cellSizeList lists must have the same amount of elements.");
            return;
        }

        // Recorrer las listas
        for (int i = 0; i < lowerLeftCornerPositions.Count; i++)
        {
            GameObject lowerLeftCornerPos = lowerLeftCornerPositions[i];

            Vector3 bottomLeft = lowerLeftCornerPos.transform.position;
            lowerLeftCornerPos.SetActive(false);

            FindPossiblePositions(Mathf.RoundToInt(floorWidths[i] / cellSizeList[i]), Mathf.RoundToInt(floorHeights[i] / cellSizeList[i]), bottomLeft, cellSizeList[i], gridCubeYSize[i]);
        }

    }

    private void FindPossiblePositions(int width, int height, Vector3 bottomLeft, float cellSize, float cubeYSize)
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

                // Crear un cubo en la posición actual
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = startPos + new Vector3(cellSize / 2f, cellSize / 2f, cellSize / 2f);
                //Debug.Log("POS Y: " + cube.transform.position.y);
                cube.transform.localScale = new Vector3(cellSize, cubeYSize, cellSize);

                allPositions.Add(cube.transform.position);

                // Comprobar si el cubo colisiona con objetos en el layer deseado
                Collider[] colliders = Physics.OverlapBox(cube.transform.position, cube.transform.localScale / 2f, Quaternion.identity, obstacleLayer);
                if (colliders.Length > 0)
                {
                    Renderer cubeRenderer = cube.GetComponent<Renderer>();
                    cubeRenderer.material.color = Color.red;
                }
                else
                {
                    // Si no hay colisión con el layer de obstáculos, comprobar si hay colisión con el layer de suelo
                    Collider[] groundColliders = Physics.OverlapBox(cube.transform.position, cube.transform.localScale / 2f, Quaternion.identity, groundLayer);
                    if (groundColliders.Length > 0)
                    {
                        Renderer cubeRenderer = cube.GetComponent<Renderer>();
                        cubeRenderer.material.color = Color.green;
                        possiblePositions.Add(cube.transform.position);
                    }
                }

                if(!debugMode) Destroy(cube);
                if(heatMapMode) cube.tag = "heatBox";

            }
        }

        if(debugMode) {
            // Draw the remaining border lines
            Debug.DrawLine(bottomLeft, bottomLeft + new Vector3(width * cellSize, 0, 0), Color.black, 100f); // bottom
            Debug.DrawLine(bottomLeft, bottomLeft + new Vector3(0, 0, height * cellSize), Color.black, 100f); // left
            Debug.DrawLine(bottomLeft + new Vector3(width * cellSize, 0, 0), bottomLeft + new Vector3(width * cellSize, 0, height * cellSize), Color.black, 100f); // top
            Debug.DrawLine(bottomLeft + new Vector3(0, 0, height * cellSize), bottomLeft + new Vector3(width * cellSize, 0, height * cellSize), Color.black, 100f); // right
        }
    }

    public List<Vector3> ReceivePositions() {
        return possiblePositions;
    }

    public List<Vector3> ReceiveAllPositions() {
        List<Vector3> sortedPositions = allPositions.OrderBy(v => v.z).ThenBy(v => v.x).ToList();
        return sortedPositions;
    }

    public void CreateHeatMapGrid(float Xshift) {
        debugMode = true;
        heatMapMode = true;
        
        for (int i = 0; i < lowerLeftCornerPositions.Count; i++)
        {
            GameObject lowerLeftCornerPos = lowerLeftCornerPositions[i];

            Vector3 bottomLeft = lowerLeftCornerPos.transform.position;
            
            bottomLeft += new Vector3(Xshift, 0f, 0f);

            FindPossiblePositions(Mathf.RoundToInt(floorWidths[i] / cellSizeList[i]), Mathf.RoundToInt(floorHeights[i] / cellSizeList[i]), bottomLeft, cellSizeList[i], gridCubeYSize[i]);
        }
    }
}
