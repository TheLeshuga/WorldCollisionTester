using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class CSVManager : MonoBehaviour
{
    public string fileName = "default";
    private string filePath;
    private Dictionary<Vector3, List<string>> vectorObjectNames; // Diccionario para almacenar los nombres de objetos por vector
    private Dictionary<Vector3, int> vectorCounts;

    void Start()
    {
        // Construct file path
        fileName = fileName + ".csv";
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        // Initialize dictionaries
        vectorObjectNames = new Dictionary<Vector3, List<string>>();
        vectorCounts = new Dictionary<Vector3, int>();

        // Register SaveData method to be called when the application quits
        Application.quitting += SaveData;
    }

    // Method to save data to CSV
    void SaveData()
    {
        string uniqueFilePath = GetUniqueFilePath(filePath);
        Debug.Log(filePath);

        StringBuilder sb = new StringBuilder();

        foreach (var kvp in vectorCounts)
        {
            Vector3 vector = kvp.Key;
            int count = kvp.Value;

            // Convertir la lista de nombres de objetos a una cadena separada por comas
            string objectNames = string.Join(",", vectorObjectNames[vector].ToArray());

            // Append vector, count, and object names to CSV line
            sb.AppendLine($"({vector.x},{vector.y},{vector.z});{count};{objectNames}");
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

    // Method to save a Vector3 and object name to the data dictionaries
    public void SaveVector(Vector3 vector, string objectName)
    {
        // Check if a similar vector already exists
        Vector3 similarVector = vectorCounts.Keys.FirstOrDefault(v => Vector3.Distance(v, vector) <= 1.0f);

        if (similarVector != Vector3.zero)
        {
            // Increment count if similar vector found
            vectorCounts[similarVector]++;
            // Add object name to the list for the similar vector
            vectorObjectNames[similarVector].Add(objectName);
        }
        else
        {
            // Add new vector to dictionaries
            vectorCounts[vector] = 1;
            vectorObjectNames[vector] = new List<string>() { objectName };
        }
    }
}


