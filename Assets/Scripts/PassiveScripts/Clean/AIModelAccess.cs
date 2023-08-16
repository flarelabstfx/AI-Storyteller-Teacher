using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using TMPro;
using System.Threading.Tasks;

public class AIModelAccess : MonoBehaviour
{
    [Header("Settings:")]
    //How many times we gonna get the prompt to be critiqued and then re-made based on the critique?
    public int refineCycles;

    [Header("Refferences:")]
    public PromptEngineer promptEngineer;
    public TextMeshProUGUI tmpTextDisplay;

    [HideInInspector]
    public string lastCompiledResult;

    public static string pythonFilesFolderPath = Application.streamingAssetsPath;

    public async Task GenerateVirtualEnvironment()
    {
        //Step 1: Navigate to directory
        string commandText = "cd \"" + pythonFilesFolderPath + "\"";

        //Step 2: Delete old virtual envirnment
        commandText += " & rmdir /s venv";

        //Step 3: Create virtual envirnment
        commandText += " & python -m venv venv";

        //Step 4: Install everything from requirements.txt
        commandText += " & venv\\Scripts\\activate.bat";
        commandText += " & pip install -r \"requirements\\dolly requirements.txt\"";
        commandText += " & pip install -r \"requirements\\gbt requirements.txt\"";

        //Run "-m pip install --upgrade" ?


        //Step 5: Run all commands.
        Process process = Process.Start("CMD.exe", "/K" + commandText);

        while (!process.HasExited)
        {
            UnityEngine.Debug.Log("Waiting...");
            await Task.Delay(1000);
        }
    }

    public async void RunModelAsync(string modelName, string prompt)
    {
        if(UIManager.algebraEnabled)
            prompt = "Algebra is the study of variables and the rules for manipulating these variables in formulas it is a unifying thread of almost all of mathematics. Elementary algebra deals with the manipulation of variables (commonly represented by Roman letters) as if they were numbers and is therefore essential in all applications of mathematics. Abstract algebra is the name given, mostly in education, to the study of algebraic structures such as groups, rings, and fields. Linear algebra, which deals with linear equations and linear mappings, is used for modern presentations of geometry, and has many practical applications (in weather forecasting, for example). There are many areas of mathematics that belong to algebra, some having 'algebra' in their name, such as commutative algebra, and some not, such as Galois theory. The word algebra is not only used for naming an area of mathematics and some subareas; it is also used for naming some sorts of algebraic structures, such as an algebra over a field, commonly called an algebra. Sometimes, the same phrase is used for a subarea and its main algebraic structures; for example, Boolean algebra and a Boolean algebra. A mathematician specialized in algebra is called an algebraist.";
        //prompt = "The dog rushed past on the wind. The little boy turned to look back at the dog, and it was there no more. Life was full of mysteries indeed, the world outside of one's own self contained view of the world is an unknown and sometimes scary place, however, the smiling cat, would always be watching to see how it all played out..." + prompt;

        //prompt = "Write me a fantasy story that teaches these concepts:\n\n" + prompt;

        await promptEngineer.SocraticPromptingAsync(modelName, prompt);
        await promptEngineer.SelfRefinePromptingAsync(modelName, lastCompiledResult + "\n\n" + prompt, refineCycles);

        OnCompileFinished();
    }

    public static string GetCommand(string modelName, string prompt)
    {
        string commandText = "";
        //if (modelName == "Dolly")
        //{
            //Adapted From: https://stackoverflow.com/questions/15878810/how-to-execute-command-on-cmd-from-c-sharp
            commandText = "cd " + "\"" + pythonFilesFolderPath + "\"";
            commandText += " & venv\\Scripts\\activate.bat";
            commandText += " & python \"" + pythonFilesFolderPath + "\\main.py\" \"" + modelName + "\" \"" + prompt;
        //}
        //else if (modelName == "test")
        //{
        //    commandText = "echo test";
        //}
        //else
        //{
        //    UnityEngine.Debug.LogWarning("Model name does not exist! Stuff won't work correctly! Please Fix!");
        //}

        return commandText;
    }

    public static ProcessStartInfo GetProcessStartInfo(string commandText)
    {
        //Adapted from Jeff Mc's answer here: https://stackoverflow.com/questions/206323/how-to-execute-command-line-in-c-get-std-out-results
        ProcessStartInfo processStartInfo = new ProcessStartInfo("CMD.exe", "/K" + commandText);
        processStartInfo.RedirectStandardOutput = true;

        //processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processStartInfo.UseShellExecute = false;

        return processStartInfo;
    }

    void OnCompileFinished()
    {
        tmpTextDisplay.text = lastCompiledResult;
    }

    public void RunTest(string modelName, string prompt)
    {
        //prompt = "Gimme the name of a superpower.";

        //Adapted From: https://stackoverflow.com/questions/15878810/how-to-execute-command-on-cmd-from-c-sharp
        string commandText = "cd " + "\"" + pythonFilesFolderPath + "\"" + " & venv\\Scripts\\activate.bat" + " & python \"" + pythonFilesFolderPath + "\\main.py\" \"" + modelName + "\" \""+ prompt +"\"";
        Process.Start("CMD.exe", "/K" + commandText);
    }
}
