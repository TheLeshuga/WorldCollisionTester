using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

public class CSVManager : MonoBehaviour
{
    public string fileName = "default";
    private string filePath;
    private Dictionary<Vector3, List<string>> vectorAgentObjects;
    private Dictionary<Vector3, List<string>> vectorObjectsName;
    private Dictionary<Vector3, int> vectorCounts;

    void Start()
    {
        // Construct file path
        fileName = fileName + ".csv";
        string folderPath = Path.Combine(Application.dataPath, "ResultsCSV"); 
        Directory.CreateDirectory(folderPath); 
        filePath = Path.Combine(folderPath, fileName);

        // Initialize dictionaries
        vectorAgentObjects = new Dictionary<Vector3, List<string>>();
        vectorObjectsName = new Dictionary<Vector3, List<string>>();
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
            string agentNames = string.Join(",", vectorAgentObjects[vector].ToArray());
            string objectNames = string.Join(",", vectorObjectsName[vector].ToArray());

            // Append vector, count, and object names to CSV line
            sb.AppendLine($"({vector.x},{vector.y},{vector.z});{count};{objectNames};{agentNames}");
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
    public void SaveVector(Vector3 vector, string agentName, string objectName)
    {
        // Check if a similar vector already exists
        Vector3 similarVector = vectorCounts.Keys.FirstOrDefault(v => Vector3.Distance(v, vector) <= 1.0f);

        if (similarVector != Vector3.zero)
        {
            // Increment count if similar vector found
            vectorCounts[similarVector]++;
            // Add object name to the list for the similar vector
            if (!vectorAgentObjects[similarVector].Contains(agentName)) vectorAgentObjects[similarVector].Add(agentName);
            if (!vectorObjectsName[similarVector].Contains(objectName)) vectorObjectsName[similarVector].Add(objectName);
        }
        else
        {
            // Add new vector to dictionaries
            vectorCounts[vector] = 1;
            vectorAgentObjects[vector] = new List<string>() { agentName };
            vectorObjectsName[vector] = new List<string>() { objectName };
        }
    }
}



