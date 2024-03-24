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

            // Initialize dictionaries
            vectorCounts = new Dictionary<Vector3, int>();
            vectorColors = new Dictionary<Vector3, Color>();

            // Register SaveDataHM method to be called when the application quits
            Application.quitting += SaveDataHM;
        }
    }

    void SaveDataHM()
    {
        string uniqueFilePath = GetUniqueFilePath(filePath);

        StringBuilder sb = new StringBuilder();

        foreach (var kvp in vectorCounts)
        {
            Vector3 vector = kvp.Key;
            int count = kvp.Value;
            Color color = vectorColors[vector];

            // Append vector, count, and color components to CSV line
            sb.AppendLine($"({vector.x},{vector.y},{vector.z});{count};({color.r},{color.g},{color.b}),{color.a}");
        }

        File.WriteAllText(uniqueFilePath, sb.ToString());
    }

    // Method to find a unique file path
    private string GetUniqueFilePath(string originalPath)
    {
        int index = 1;
        string newPath = originalPath;

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

    // Method to save a Vector3 
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

        // Update or add color
        vectorColors[vector] = positionColor;
    }
}

