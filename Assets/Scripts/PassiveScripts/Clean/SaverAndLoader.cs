using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class SaverAndLoader : MonoBehaviour
{
    public InstructorMenuManager instructorMenuManager;

    string path = Application.streamingAssetsPath + "/SaveData.json";

    class SaveData
    {
        public List<ContentBulletPoint> educationContent;

        public SaveData(InstructorMenuManager instructorMenuManager)
        {
            educationContent = new List<ContentBulletPoint>(instructorMenuManager.educationContent);
        }
    }

    public void SaveAll()
    {
        SaveData saveData = new SaveData(instructorMenuManager);

        File.WriteAllText(path, JsonUtility.ToJson(saveData));
    }

    public void LoadAll()
    {
        SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
        if (saveData != null)
        {
            instructorMenuManager.educationContent = saveData.educationContent;
            instructorMenuManager.UpdateBulletPointDisplay();
        }
    }
}
