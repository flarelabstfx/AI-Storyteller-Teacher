using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using TMPro;

public class AIModelAccess : MonoBehaviour
{
    public TextMeshProUGUI tmpTextDisplay;
    [HideInInspector]
    public string lastCompiledResult;

    Process commandPromptProcess;

    public void CustomUpdate()
    {
        CheckIfCommandPromptProcessIsDone();
    }

    public void RunTest()
    {
        UnityEngine.Debug.Log("Clean this script");

        UnityEngine.Debug.Log("Go!");
        //Adapted From: https://stackoverflow.com/questions/9679375/how-can-i-run-an-exe-file-from-my-c-sharp-code
        //Process.Start("-Dolly\\main.py");
        //Adapted From: https://stackoverflow.com/questions/15878810/how-to-execute-command-on-cmd-from-c-sharp
        string dollyFolderPath = "-Dolly";
        string commandText = "cd " + "\"" + dollyFolderPath + "\"";
        //commandText = "cd -Dolly";
        commandText += " & venv\\Scripts\\activate.bat";
        //commandText += " & cd ../";
        commandText += " & python \"-Dolly\\main.py\"";
        //string strCmdText = "/K cd \"../\"";
        Process.Start("CMD.exe", "/K" + commandText);
    }

    void CheckIfCommandPromptProcessIsDone()
    {
        //If there is no process, or there is one but it's not done running yet, then never mind.
        if(commandPromptProcess == null || !commandPromptProcess.HasExited)
        {
            return;
        }

        //Get command prompt output results.
        lastCompiledResult = commandPromptProcess.StandardOutput.ReadToEnd();
        commandPromptProcess = null;

        /*if (process.HasExited)
        {
            output = myOutput.ReadToEnd();
        }*/

        //return output;

        //lastCompiledResult = output;

        //Do anything else you are meant to once compiling has ended, for example sending the results off to the correct places needed or showing some indication that it's done.
        OnCompileFinished();
    }

    public void RunModel(string modelName)
    {
        UnityEngine.Debug.Log("Compile Started");

        string commandText = "";
        if (modelName == "Dolly")
        {
            UnityEngine.Debug.Log("Dolly");
            UnityEngine.Debug.Log("Clean this script");

            UnityEngine.Debug.Log("Go!");
            //Adapted From: https://stackoverflow.com/questions/9679375/how-can-i-run-an-exe-file-from-my-c-sharp-code
            //Process.Start("-Dolly\\main.py");
            //Adapted From: https://stackoverflow.com/questions/15878810/how-to-execute-command-on-cmd-from-c-sharp
            string dollyFolderPath = "-Dolly";
            commandText = "cd " + "\"" + dollyFolderPath + "\"";
            //commandText = "cd -Dolly";
            commandText += " & venv\\Scripts\\activate.bat";
            //commandText += " & cd ../";
            commandText += " & python \"-Dolly\\main.py\"";
            //string strCmdText = "/K cd \"../\"";
        }

        //Adapted from Jeff Mc's answer here: https://stackoverflow.com/questions/206323/how-to-execute-command-line-in-c-get-std-out-results
        ProcessStartInfo processStartInfo = new ProcessStartInfo("CMD.exe", "/K" + commandText);
        processStartInfo.RedirectStandardOutput = true;

        //processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processStartInfo.UseShellExecute = false;

        commandPromptProcess = Process.Start(processStartInfo);
        
        
        
        
        
        /*System.IO.StreamReader myOutput = commandPromptProcess.StandardOutput;

        //process.WaitForExit(2000);

        string output = "Did not receive output.";

        int i = 0;
        while (!commandPromptProcess.HasExited)
        {
            yield return null;
            UnityEngine.Debug.Log(i++);
        }

        output = myOutput.ReadToEnd();*/

        /*if (process.HasExited)
        {
            output = myOutput.ReadToEnd();
        }*/

        //return output;

        /*lastCompiledResult = output;

        OnCompileFinished();*/
    }

    public IEnumerator RunModelCoroutine(string modelName)
    {
        UnityEngine.Debug.Log("Compile Started");

        string commandText = "";
        if(modelName == "Dolly")
        {
            UnityEngine.Debug.Log("Clean this script");

            UnityEngine.Debug.Log("Go!");
            //Adapted From: https://stackoverflow.com/questions/9679375/how-can-i-run-an-exe-file-from-my-c-sharp-code
            //Process.Start("-Dolly\\main.py");
            //Adapted From: https://stackoverflow.com/questions/15878810/how-to-execute-command-on-cmd-from-c-sharp
            string dollyFolderPath = "-Dolly";
            commandText = "cd " + "\"" + dollyFolderPath + "\"";
            //commandText = "cd -Dolly";
            commandText += " & venv\\Scripts\\activate.bat";
            //commandText += " & cd ../";
            commandText += " & python \"-Dolly\\main.py\" outputOnly";
            //string strCmdText = "/K cd \"../\"";
        } else if(modelName == "test")
        {
            commandText = "echo test";
        } else
        {
            UnityEngine.Debug.LogWarning("Model name does not exist! Stuff won't work correctly! Please Fix!");
        }

        //Adapted from Jeff Mc's answer here: https://stackoverflow.com/questions/206323/how-to-execute-command-line-in-c-get-std-out-results
        ProcessStartInfo processStartInfo = new ProcessStartInfo("CMD.exe", "/K" + commandText);
        processStartInfo.RedirectStandardOutput = true;

        //processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        processStartInfo.UseShellExecute = false;

        Process process = Process.Start(processStartInfo);
        System.IO.StreamReader myOutput = process.StandardOutput;

        //process.WaitForExit(2000);

        string output = "Did not receive output.";

        int i = 0;
        while (!process.HasExited)
        {
            yield return null;
            UnityEngine.Debug.Log(i++);
        }

        output = myOutput.ReadToEnd();

        //Adapted From: https://stackoverflow.com/questions/20432379/remove-last-line-from-a-string
        output = output.Remove(output.LastIndexOf(System.Environment.NewLine));

        /*if (process.HasExited)
        {
            output = myOutput.ReadToEnd();
        }*/

        //return output;

        lastCompiledResult = output;

        OnCompileFinished();
    }

    void OnCompileFinished()
    {
        tmpTextDisplay.text = lastCompiledResult;
        UnityEngine.Debug.Log("Compile Done");
    }
}
