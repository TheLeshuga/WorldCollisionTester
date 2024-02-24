using UnityEngine;
using System.Collections.Generic;

public class GridMap : MonoBehaviour
{
    public List<GameObject> floorObjects;
    public List<GameObject> lowerLeftCornerPositions;

    public float cellSize;

    public LayerMask obstacleLayer; // Layer para obstaculos
    public LayerMask groundLayer; // Layer para el suelo

    public bool debugMode = false;

    private List<MoveToGoalWithCollision> moveToGoal = new List<MoveToGoalWithCollision>();
    private List<Vector3> possiblePositions = new List<Vector3>();

    private void Awake()
    {
        // Verificar si las listas tienen el mismo número de elementos
        if (floorObjects.Count != lowerLeftCornerPositions.Count)
        {
            Debug.LogError("The floorObjects and lowerLeftCornerPositions lists must have the same amount of elements.");
            return;
        }

        // Recorrer las listas
        for (int i = 0; i < floorObjects.Count; i++)
        {
            GameObject floorObject = floorObjects[i];
            GameObject lowerLeftCornerPos = lowerLeftCornerPositions[i];

            Renderer renderer = floorObject.GetComponent<Renderer>();
            int width = Mathf.RoundToInt(renderer.bounds.size.x / cellSize);
            int height = Mathf.RoundToInt(renderer.bounds.size.z / cellSize);

            Vector3 bottomLeft = lowerLeftCornerPos.transform.position;
            lowerLeftCornerPos.SetActive(false);

            FindPossiblePositions(width, height, bottomLeft);
        }

    }

    private void FindPossiblePositions(int width, int height, Vector3 bottomLeft)
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
                Debug.Log("POS Y: " + cube.transform.position.y);
                cube.transform.localScale = new Vector3(cellSize, cellSize, cellSize);

                // Comprobar si el cubo colisiona con objetos en el layer deseado
                Collider[] colliders = Physics.OverlapBox(cube.transform.position, cube.transform.localScale / 2f, Quaternion.identity, obstacleLayer);
                if (colliders.Length > 0)
                {
                    // Si hay colisión con el layer de obstáculos, cambiar el color del cubo a rojo
                    Renderer cubeRenderer = cube.GetComponent<Renderer>();
                    cubeRenderer.material.color = Color.red;
                }
                else
                {
                    // Si no hay colisión con el layer de obstáculos, comprobar si hay colisión con el layer de suelo
                    Collider[] groundColliders = Physics.OverlapBox(cube.transform.position, cube.transform.localScale / 2f, Quaternion.identity, groundLayer);
                    if (groundColliders.Length > 0)
                    {
                        // Si hay colisión con el layer de suelo, cambiar el color del cubo a verde
                        Renderer cubeRenderer = cube.GetComponent<Renderer>();
                        cubeRenderer.material.color = Color.green;
                        //print(cube.transform.position);
                        possiblePositions.Add(cube.transform.position);
                    }
                }

                if(!debugMode) Destroy(cube);
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
}
