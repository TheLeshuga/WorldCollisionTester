using UnityEngine;
using System.IO;
using System.Globalization;

public class HeatMapVisualizer : MonoBehaviour
{
    public float Xshift = 0f;   // Shift applied to the X coordinate of each position
    public float cubeSize = 1f; // Size of the cubes representing data points
    public string csvFileName = "heatmap_data"; // Name of the CSV file containing heatmap data

    private string folderPath;  // Path to the folder containing CSV files
    private string csvFilePath; // Path to the CSV file to load

    void Start()
    {
        // Combine folder path with "ResultsCSV-HM" directory in the project's data path
        folderPath = Path.Combine(Application.dataPath, "ResultsCSV-HM");

        // Ensure the CSV file name has the correct extension
        csvFileName = AddCsvExtension(csvFileName);

        // Combine folder path with CSV file name to get the full file path
        csvFilePath = Path.Combine(folderPath, csvFileName);

        // Check if the folder containing CSV files exists
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("Folder not found: " + folderPath);
            return;
        }

        // Check if the CSV file exists
        if (!File.Exists(csvFilePath))
        {
            Debug.LogError("CSV file not found: " + csvFilePath);
            return;
        }

        // Load heatmap data from the CSV file
        LoadHeatMapData(csvFilePath);
    }

    // Add .csv extension to the file name if it's missing
    string AddCsvExtension(string fileName)
    {
        if (!fileName.EndsWith(".csv"))
        {
            return fileName + ".csv";
        }
        return fileName;
    }

    // Load heatmap data from the CSV file
    void LoadHeatMapData(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                string[] values = line.Split(';');
                if (values.Length < 3)
                {
                    Debug.LogError("Invalid CSV format: " + line);
                    continue;
                }

                // Parse position data
                Vector3 position = ParseVector3(values[0]);
                position.x += Xshift;

                // Parse color data
                Color color = ParseColor(values[2]);

                // Create cube at position with color
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = position;
                cube.transform.localScale = new Vector3(cubeSize, cubeSize, cubeSize);
                cube.GetComponent<Renderer>().material.color = color;
            }
        }
    }

    // Parse Vector3 data from the CSV string
    Vector3 ParseVector3(string vectorString)
    {
        string[] components = vectorString.TrimStart('(').TrimEnd(')').Split('/');
        if (components.Length != 3)
        {
            Debug.LogError("Invalid Vector3 format: " + vectorString);
            return Vector3.zero;
        }

        float x = float.Parse(components[0]);
        float y = float.Parse(components[1]);
        float z = float.Parse(components[2]);
        return new Vector3(x, y, z);
    }

    // Parse Color data from the CSV string
    private Color ParseColor(string colorString)
    {
        // Remove parentheses from the color string
        colorString = colorString.Replace("(", "").Replace(")", "");

        // Split the string into RGB components and alpha channel
        string[] components = colorString.Split('/');

        // Convert string components to float values
        float r = float.Parse(components[0]);
        float g = float.Parse(components[1]);
        float b = float.Parse(components[2]);
        float a = float.Parse(components[3]);

        return new Color(r, g, b, a);
    }
}


