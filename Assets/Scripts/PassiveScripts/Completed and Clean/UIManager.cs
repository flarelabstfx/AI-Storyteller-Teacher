using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Refferences:")]
    public GameObject mainMenuGroup;
    public GameObject instructorMenuGroup;
    public InstructorMenuManager instructorMenuManager;
    public GameObject storyGroup;
    //public TextMeshProUGUI storyText;
    public AIModelAccess aIModelAccess;

    public void StartButtonPressed()
    {
        mainMenuGroup.SetActive(false);
        StartCoroutine(aIModelAccess.RunModelCoroutine("Dolly"));
        //aIModelAccess.RunModel("Dolly");
        //aIModelAccess.RunTest();
        storyGroup.SetActive(true);
    }

    public void InstructorButtonPressed()
    {
        mainMenuGroup.SetActive(false);
        instructorMenuGroup.SetActive(true);
    }

    public void BackButtonPressed()
    {
        instructorMenuGroup.SetActive(false);
        mainMenuGroup.SetActive(true);
    }

    public void DeleteBulletPointButtonPressed(TextMeshProUGUI bulletPointIDTextComponant)
    {
        DeleteBulletPointButtonPressed(bulletPointIDTextComponant.text);
    }

    public void DeleteBulletPointButtonPressed(string bulletPointIDString)
    {
        instructorMenuManager.RemoveBulletPoint(bulletPointIDString);
    }

    public void AddBulletPointButtonPressed(string parentBulletPointIDString)
    {
        instructorMenuManager.AddBulletPoint(parentBulletPointIDString);
    }

    public void OnBulletPointInputFieldContentsUpdated(string bulletPointIDString, string newContents)
    {
        instructorMenuManager.UpdateBulletPointContents(bulletPointIDString, newContents);
    }

    public void MakeSubGroupButtonPressed(string bulletPointIDString)
    {
        instructorMenuManager.AddBulletPoint(bulletPointIDString);
    }
}
