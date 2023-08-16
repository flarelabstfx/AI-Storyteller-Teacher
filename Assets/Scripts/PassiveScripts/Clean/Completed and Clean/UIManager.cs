//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Refferences:")]
    public GameObject mainMenuGroup;
    public GameObject instructorMenuGroup;
    public InstructorMenuManager instructorMenuManager;
    public GameObject storyGroup;
    public AIModelAccess aIModelAccess;
    public GameObject loadingScreen;

    [HideInInspector]
    bool dollyToggleEnabled;
    bool gbtToggleEnabled;
    public string chosenModel;
    public static bool algebraEnabled = true;

    public void StartButtonPressed()
    {
        mainMenuGroup.SetActive(false);
        Debug.Log("Total Prompt:\n" + instructorMenuManager.totalPrompt);
        //aIModelAccess.RunModelAsync("Dolly", instructorMenuManager.totalPrompt);
        aIModelAccess.RunModelAsync(chosenModel, instructorMenuManager.totalPrompt);
        //aIModelAccess.RunTest("Dolly", "Name an island in a pirate movie.");
        //aIModelAccess.RunTest("GBT", "Name an island in a pirate movie.");
        storyGroup.SetActive(true);
    }

    public void InstructorButtonPressed()
    {
        mainMenuGroup.SetActive(false);
        instructorMenuGroup.SetActive(true);
    }

    public async void InitialSetupButtonPressed()
    {
        loadingScreen.SetActive(true);
        await aIModelAccess.GenerateVirtualEnvironment();
        loadingScreen.SetActive(false);
    }

    public void DollyCheckboxToggled(bool input)
    {
        Debug.Log(input);

        dollyToggleEnabled = input;

        if(input) {

            chosenModel = "Dolly";

        } else if(gbtToggleEnabled)
        {
            chosenModel = "GBT";
        } else
        {
            chosenModel = "";
        }

    }

    public void GBTCheckboxToggled(bool input)
    {
        Debug.Log(input);

        gbtToggleEnabled = input;

        if (input)
        {

            chosenModel = "GBT";

        }
        else if (dollyToggleEnabled)
        {
            chosenModel = "Dolly";
        }
        else
        {
            chosenModel = "";
        }

    }

    public void DefaultAlgebraPromptCheckboxToggled(bool input)
    {
        algebraEnabled = input;
    }

    //-------------------- \/ Instructor Menu \/ --------------------

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

    public void OnRefineCyclesInputFieldContentsUpdated(string newCycleCount)
    {
        aIModelAccess.refineCycles = int.Parse(newCycleCount);

        Debug.Log(aIModelAccess.refineCycles);
    }
}
