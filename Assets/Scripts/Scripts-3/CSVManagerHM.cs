using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Text;

public class CSVManagerHM : MonoBehaviour
{
    public string fileName = "default";
    private string filePath;
    private Dictionary<Vector3, int> vectorCounts;
    private Dictionary<Vector3, Color> vectorColors;

    void Awake()
    {
        if(this.enabled) {
            // Construct file path
            fileName = fileName + ".csv";
            string folderPath = Path.Combine(Application.dataPath, "ResultsCSV-HM"); 
            Directory.CreateDirectory(folderPath); 
            filePath = Path.Combine(folderPath, fileName);

            // Initialize dictionaries to store vector counts and colors
            vectorCounts = new Dictionary<Vector3, int>();
            vectorColors = new Dictionary<Vector3, Color>();

            // Register SaveDataHM method to be called when the application quits
            Application.quitting += SaveDataHM;
        }
    }

    void SaveDataHM()
    {
        // Get a unique file path to avoid overwriting existing files
        string uniqueFilePath = GetUniqueFilePath(filePath);

        StringBuilder sb = new StringBuilder();

        // Iterate through each vector and its count
        foreach (var kvp in vectorCounts)
        {
            Vector3 vector = kvp.Key;
            int count = kvp.Value;
            Color color = vectorColors[vector];

            Color32 color32 = color;
            string hexColor = "#" + color32.r.ToString("X2") + color32.g.ToString("X2") + color32.b.ToString("X2");

            // Append vector, count, and color components to CSV line
            sb.AppendLine($"({vector.x},{vector.y},{vector.z});{count};({color.r},{color.g},{color.b}),{color.a};{hexColor}");
        }

        // Write the CSV content to the file
        File.WriteAllText(uniqueFilePath, sb.ToString());
    }

    // Method to find a unique file path to avoid overwriting existing files
    private string GetUniqueFilePath(string originalPath)
    {
        int index = 1;
        string newPath = originalPath;

        // Check if the file exists, if so, add an index to the file name
        while (File.Exists(newPath))
        {
            index++;
            newPath = Path.Combine(
                Path.GetDirectoryName(originalPath),
                $"{Path.GetFileNameWithoutExtension(originalPath)}_{index}{Path.GetExtension(originalPath)}"
            );
        }

        return newPath;
    }

    // Method to save a Vector3 and its corresponding color
    public void SaveVector(Vector3 vector, Color positionColor)
    {

        if (vectorCounts.ContainsKey(vector))
        {
            // Increment count if similar vector found
            vectorCounts[vector]++;
        }
        else
        {
            // Add new vector to dictionaries
            vectorCounts[vector] = 1;
        }

        // Update or add color associated with the vector
        vectorColors[vector] = positionColor;
    }
}


