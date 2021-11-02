using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveGameManager : MonoBehaviour
{
    private static SaveFile saveFile;
    private static string filePath;

    static SaveGameManager()
    {
        saveFile = new SaveFile();

        filePath = Application.dataPath + "/HyakumenYakou.save";
        Debug.Log(filePath);
        //
    }


    public static void Save()
    {
        saveFile.achievements = SaveData.GetAchievements();
        saveFile.stepRecords = SaveData.GetStepRecords();

        string jsonFile = JsonUtility.ToJson(saveFile, true);

        File.WriteAllText(filePath, jsonFile);
    }

    public static void Load()
    {

        if (File.Exists(filePath))
        {
            string jsonFile = File.ReadAllText(filePath);

            saveFile = JsonUtility.FromJson<SaveFile>(jsonFile);
        }
    }
}

public class SaveFile
{
    public int highScore = 100;
    public StepRecord[] stepRecords;
    public Achievement[] achievements;
}