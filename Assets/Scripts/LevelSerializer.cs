using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LevelSerializer : MonoBehaviour
{
    private static string FILES_PATH = "{0}/SerializedLevel/";
    private static string FILE_PREFX = "Level_{0}.json";

    public void Serialize(List<GameObject> ingredientsList)
    {
        SerializableLevel serializableLevel = new SerializableLevel();
        serializableLevel.Ingredients.AddRange(ingredientsList);

        string json = JsonUtility.ToJson(serializableLevel);
        var appDataPath = string.Format(FILES_PATH, Application.persistentDataPath);
        List<string> items = Directory.GetFileSystemEntries(appDataPath).ToList();

        var fileName = string.Format(FILE_PREFX, items.Count);
        var filePath = appDataPath + fileName;
        Debug.Log(filePath);
        File.WriteAllText(filePath, json);
    }

    public List<GameObject> Deserialize(string fileName)
    {
        var appDataPath = string.Format(FILES_PATH, Application.persistentDataPath);
        var filePath = appDataPath + fileName;
        string json = File.ReadAllText(filePath);

        SerializableLevel serializableLevel = new SerializableLevel();
        JsonUtility.FromJsonOverwrite(json, serializableLevel);

        Debug.Log(json);

        return serializableLevel.Ingredients;
    }

    public List<string> GetFilesInFolder()
    {
        var appDataPath = string.Format(FILES_PATH, Application.persistentDataPath);

        if (!Directory.Exists(appDataPath))
        {
            Directory.CreateDirectory(appDataPath);
        }

        var fileNames = new List<string>();
        foreach (var path in Directory.GetFiles(appDataPath))
        {
            fileNames.Add(Path.GetFileName(path));
        }

        return fileNames;
    }
}
