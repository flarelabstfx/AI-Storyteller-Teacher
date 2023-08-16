//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Threading.Tasks;

//Likely direction: Combine both methods, maybe running socratic first then self refine, likey end up with a more close knit intertwining.
public class PromptEngineer : MonoBehaviour
{
    public AIModelAccess aIModelAccess;

    string lastOutput;

    async Task GetOutputFromModel(string modelName, string prompt)
    {
        Process process = Process.Start(AIModelAccess.GetProcessStartInfo(AIModelAccess.GetCommand(modelName, prompt)));
        System.IO.StreamReader outputStream = process.StandardOutput;

        string outputString = "Did not receive output.";

        while (!process.HasExited)
        {
            UnityEngine.Debug.Log("Waiting...");
            await Task.Delay(1000);
        }

        outputString = outputStream.ReadToEnd();

        //Adapted From: https://stackoverflow.com/questions/20432379/remove-last-line-from-a-string
        outputString = outputString.Remove(outputString.LastIndexOf(System.Environment.NewLine));

        lastOutput = outputString;
    }

    public async Task SelfRefinePromptingAsync(string modelName, string prompt, int refineCycles)
    {

        //string critiqueGuideString = "Critique the following:\n\n";
        string critiqueGuideString = "Give feedback on this J. K. Rowling story:\n\n";

        //Step 1: Generate initial output from starting prompt
        await GetOutputFromModel(modelName, prompt);

        /*Process selfRefineProcess = Process.Start(AIModelAccess.GetProcessStartInfo(AIModelAccess.GetCommand(modelName, prompt)));
        System.IO.StreamReader selfRefineOutputStream = selfRefineProcess.StandardOutput;

        string selfRefineOutputString = "Did not receive output.";

        //Adapted from Marco Concas & David Molnar's answer here: https://stackoverflow.com/questions/470256/process-waitforexit-asynchronously
        while (!selfRefineProcess.HasExited)
        {
            UnityEngine.Debug.Log("Waiting...");
            await Task.Delay(1000);
        }

        selfRefineOutputString = selfRefineOutputStream.ReadToEnd();

        //Adapted From: https://stackoverflow.com/questions/20432379/remove-last-line-from-a-string
        selfRefineOutputString = selfRefineOutputString.Remove(selfRefineOutputString.LastIndexOf(System.Environment.NewLine));*/

        for (int i = 0; i < refineCycles; i++)
        {
            UnityEngine.Debug.Log("i: " + i);
            UnityEngine.Debug.Log("selfRefineOutputString: " + lastOutput);


            //Step 2: Critique this output.
            await GetOutputFromModel(modelName, critiqueGuideString + lastOutput);

            /*Process critiqueProcess = Process.Start(AIModelAccess.GetProcessStartInfo(AIModelAccess.GetCommand(modelName, "Critique the following:\n\n" + selfRefineOutputString)));
            System.IO.StreamReader critiqueOutputStream = critiqueProcess.StandardOutput;

            string critiqueOutputString = "Did not receive output.";

            while (!critiqueProcess.HasExited)
            {
                UnityEngine.Debug.Log("Waiting...");
                await Task.Delay(1000);
            }

            critiqueOutputString = critiqueOutputStream.ReadToEnd();

            //Adapted From: https://stackoverflow.com/questions/20432379/remove-last-line-from-a-string
            critiqueOutputString = critiqueOutputString.Remove(critiqueOutputString.LastIndexOf(System.Environment.NewLine));*/

            UnityEngine.Debug.Log("critiqueOutputString: " + lastOutput);


            //Step 3: Get model to give new refined output based on critique.
            await GetOutputFromModel(modelName, "Address this critique:\n\n" + lastOutput + "\n\nApply the above critique for the following prompt:\n\n" + prompt);

            /*selfRefineProcess = Process.Start(AIModelAccess.GetProcessStartInfo(AIModelAccess.GetCommand(modelName, "Address this critique:\n\n" + critiqueOutputString + "\n\nApply the above critique for the following prompt:\n\n" + prompt)));
            selfRefineOutputStream = selfRefineProcess.StandardOutput;

            selfRefineOutputString = "Did not receive output.";

            while (!selfRefineProcess.HasExited)
            {
                UnityEngine.Debug.Log("Waiting...");
                await Task.Delay(1000);
            }

            selfRefineOutputString = selfRefineOutputStream.ReadToEnd();

            //Adapted From: https://stackoverflow.com/questions/20432379/remove-last-line-from-a-string
            selfRefineOutputString = selfRefineOutputString.Remove(selfRefineOutputString.LastIndexOf(System.Environment.NewLine));*/

            //Step 4: Repeat steps 2-3 as many times as requested (in the refineCycles variable).
        }

        //UnityEngine.Debug.Log("Final selfRefineOutputString (i.e. the final resulting output): " + selfRefineOutputString);
        UnityEngine.Debug.Log("Final selfRefineOutputString (i.e. the final resulting output): " + lastOutput);
        //aIModelAccess.lastCompiledResult = selfRefineOutputString;
        aIModelAccess.lastCompiledResult = lastOutput;

    }

    /*public async Task SelfRefinePromptingAsync(string modelName, string prompt, int refineCycles)
    {
        //Step 1: Generate initial output from starting prompt
        Process selfRefineProcess = Process.Start(AIModelAccess.GetProcessStartInfo(AIModelAccess.GetCommand(modelName, prompt)));
        System.IO.StreamReader selfRefineOutputStream = selfRefineProcess.StandardOutput;

        string selfRefineOutputString = "Did not receive output.";

        //Adapted from Marco Concas & David Molnar's answer here: https://stackoverflow.com/questions/470256/process-waitforexit-asynchronously
        while (!selfRefineProcess.HasExited)
        {
            UnityEngine.Debug.Log("Waiting...");
            await Task.Delay(1000);
        }

        selfRefineOutputString = selfRefineOutputStream.ReadToEnd();

        //Adapted From: https://stackoverflow.com/questions/20432379/remove-last-line-from-a-string
        selfRefineOutputString = selfRefineOutputString.Remove(selfRefineOutputString.LastIndexOf(System.Environment.NewLine));

        for (int i = 0; i < refineCycles; i++)
        {
            UnityEngine.Debug.Log("i: " + i);
            UnityEngine.Debug.Log("selfRefineOutputString: " + selfRefineOutputString);


            //Step 2: Critique this output.
            Process critiqueProcess = Process.Start(AIModelAccess.GetProcessStartInfo(AIModelAccess.GetCommand(modelName, "Critique the following:\n\n" + selfRefineOutputString)));
            System.IO.StreamReader critiqueOutputStream = critiqueProcess.StandardOutput;

            string critiqueOutputString = "Did not receive output.";

            while (!critiqueProcess.HasExited)
            {
                UnityEngine.Debug.Log("Waiting...");
                await Task.Delay(1000);
            }

            critiqueOutputString = critiqueOutputStream.ReadToEnd();

            //Adapted From: https://stackoverflow.com/questions/20432379/remove-last-line-from-a-string
            critiqueOutputString = critiqueOutputString.Remove(critiqueOutputString.LastIndexOf(System.Environment.NewLine));

            UnityEngine.Debug.Log("critiqueOutputString: " + critiqueOutputString);


            //Step 3: Get model to give new refined output based on critique.
            selfRefineProcess = Process.Start(AIModelAccess.GetProcessStartInfo(AIModelAccess.GetCommand(modelName, "Address this critique:\n\n" + critiqueOutputString + "\n\nApply the above critique for the following prompt:\n\n" + prompt)));
            selfRefineOutputStream = selfRefineProcess.StandardOutput;

            selfRefineOutputString = "Did not receive output.";

            while (!selfRefineProcess.HasExited)
            {
                UnityEngine.Debug.Log("Waiting...");
                await Task.Delay(1000);
            }

            selfRefineOutputString = selfRefineOutputStream.ReadToEnd();

            //Adapted From: https://stackoverflow.com/questions/20432379/remove-last-line-from-a-string
            selfRefineOutputString = selfRefineOutputString.Remove(selfRefineOutputString.LastIndexOf(System.Environment.NewLine));

            //Step 4: Repeat steps 2-3 as many times as requested (in the refineCycles variable).
        }
        
        UnityEngine.Debug.Log("Final selfRefineOutputString (i.e. the final resulting output): " + selfRefineOutputString);
        aIModelAccess.lastCompiledResult = selfRefineOutputString;

    }*/

    /// <summary>
    /// From: https://en.wikipedia.org/wiki/Prompt_engineering
    /// Maieutic (Socratic) prompting first generates an explanation of the phenomena related to
    /// a problem to be solved, then - after receiving the explanation - prompts again to generate
    /// additional explanation for the least understood parts of explanation. Inconsistent
    /// explanation paths (trees) are pruned or discarded, improving complex commonsense reasoning.
    /// </summary>
    /// <returns></returns>
    
    //For now, this will just generate explanation of the phnomena.
    public async Task SocraticPromptingAsync(string modelName, string prompt)
    {
        //string refinedPrompt = "";

        //await GetOutputFromModel("GBT", "Generate an explanation about the phenomina surrounding how to tell a fantasy story.");
        //await GetOutputFromModel("GBT", "Give me a paragraph about algebra as if it were written by J. K. Rowling.");
        await GetOutputFromModel(modelName, "Give me a paragraph of stortelling by J. K. Rowling.");
        //await GetOutputFromModel("GBT", "Give me a story as if written by J. K. Rowling about the following content:\n\n");
        UnityEngine.Debug.Log("Socratic Prompting Result: " + lastOutput);

        //return refinedPrompt;

        //return lastOutput;

        aIModelAccess.lastCompiledResult = lastOutput;
    }
}
