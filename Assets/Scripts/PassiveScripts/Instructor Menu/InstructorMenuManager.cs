//using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Welcome to this annoyingly complicated script :P.

[System.Serializable]
public class ContentBulletPoint
{
    public ContentBulletPoint parentBulletPoint;

    public string contents;

    public List<ContentBulletPoint> subBulletPoints;

    /// <summary>
    /// Leave parent as null to indicate bullet point is not in a subgroup.
    /// </summary>
    /// <param name="parentBulletPoint"></param>
    public ContentBulletPoint(ContentBulletPoint parentBulletPoint)
    {
        subBulletPoints = new List<ContentBulletPoint>();
        this.parentBulletPoint = parentBulletPoint;
    }
}

public class InstructorMenuManager : MonoBehaviour
{

    [Header("Settings:")]
    public float indentationSpaceSize;

    [Header("Refferences:")]
    public Transform addBulletPointButton;
    public GameObject bulletPointPrefab;
    public GameObject addBulletPointButtonPrefab;
    public SaverAndLoader saverAndLoader;

    [HideInInspector]
    public string totalPrompt = "";

    public List<ContentBulletPoint> educationContent = new List<ContentBulletPoint>();
    List<GameObject> rowObjects = new List<GameObject>();
    float addBulletPointButtonStartingY;

    // Start is called before the first frame update
    public void CustomAwake()
    {
        addBulletPointButtonStartingY = addBulletPointButton.localPosition.y;

        Debug.Log("Clean script since total probmpt was added (global declaration and usage in updatebulletpointdisplay function)");
    }

    List<int> GetBulletPointIDList(string bulletPointIDString)
    {
        List<int> resultingList = new List<int>();
        string currentNumber = "";
        foreach (char c in bulletPointIDString)
        {
            if (c == '.')
            {
                if (currentNumber != "")
                {
                    resultingList.Add(int.Parse(currentNumber));
                    currentNumber = "";
                }
            }
            else
            {
                currentNumber += c;
            }
        }

        return resultingList;
    }

    string GetBulletPointIDstring(List<int> bulletPointIDList)
    {
        string resultingString = "";
        
        foreach(int number in bulletPointIDList)
        {
            resultingString += number + ".";
        }

        return resultingString;
    }

    ContentBulletPoint GetContentBulletPoint(string bulletPointIDString)
    {
        return GetContentBulletPoint(GetBulletPointIDList(bulletPointIDString));
    }

    ContentBulletPoint GetContentBulletPoint(List<int> bulletPointIDList)
    {
        ContentBulletPoint currentBulletPoint = null;
        for (int i = 0; i < bulletPointIDList.Count; i++)
        {
            if (i == 0)
            {
                currentBulletPoint = educationContent[bulletPointIDList[i] - 1];
            }
            else
            {
                currentBulletPoint = currentBulletPoint.subBulletPoints[bulletPointIDList[i] - 1];
            }
        }

        return currentBulletPoint;
    }

    public void UpdateBulletPointContents(string bulletPointIDString, string newContents)
    {

        Debug.Log("Clean this!");

        ContentBulletPoint tmp = GetContentBulletPoint(bulletPointIDString);
        tmp.contents = newContents;

        Debug.Log("test");
        UpdateBulletPointDisplay();
    }

    public void AddBulletPoint(string parentBulletPointID, string contents = "")
    {
        ContentBulletPoint parentBulletPointObject = GetContentBulletPoint(parentBulletPointID);
        ContentBulletPoint newBulletPoint = new ContentBulletPoint(parentBulletPointObject);

        newBulletPoint.contents = contents;

        List<ContentBulletPoint> bulletPointListToAddTo;
        if (parentBulletPointID == ".")
            bulletPointListToAddTo = educationContent;
        else
            bulletPointListToAddTo = parentBulletPointObject.subBulletPoints;

        bulletPointListToAddTo.Add(newBulletPoint);

        UpdateBulletPointDisplay();
    }

    public void RemoveBulletPoint(string bulletPointIDString)
    {
        List<int> bulletPointIDList = GetBulletPointIDList(bulletPointIDString);
        ContentBulletPoint contentBulletPoint = GetContentBulletPoint(bulletPointIDList);
        List<ContentBulletPoint> parentList;

        if (contentBulletPoint.parentBulletPoint == null)
        {
            parentList = educationContent;
        } else
        {
            parentList = contentBulletPoint.parentBulletPoint.subBulletPoints;
        }

        parentList.RemoveAt(bulletPointIDList[bulletPointIDList.Count - 1] - 1);

        UpdateBulletPointDisplay();
    }


    /// <summary>
    /// An element row is a bullet point or a add button.
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="rowElementCountBeforeInstantiating"></param>
    /// <param name="depthLevel"></param>
    /// <returns>The instantiated GameObject.</returns>
    GameObject InstantiateElementRow(GameObject prefab, int rowElementCountBeforeInstantiating, int depthLevel)
    {
        GameObject newObject = Instantiate(prefab, transform);

        newObject.GetComponent<RectTransform>().localPosition += new Vector3(indentationSpaceSize * depthLevel, -40 * rowElementCountBeforeInstantiating, 0);
        rowObjects.Add(newObject);

        return newObject;
    }

    GameObject InstantiateBulletPoint(int rowElementCountBeforeInstantiating, List<int> ID, string newBulletPointContents)
    {
        GameObject rowObject = InstantiateElementRow(bulletPointPrefab, rowElementCountBeforeInstantiating, ID.Count - 1);

        string res = "";
        foreach (int number in ID)
        {
            res += number + ".";
        }

        rowObject.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = res;
        rowObject.transform.GetChild(3).GetComponent<TMP_InputField>().text = newBulletPointContents;

        return rowObject;
    }

    GameObject InstantiateAddButton(int rowElementCountBeforeInstantiating, List<int> ID)
    {
        GameObject newObject = InstantiateElementRow(addBulletPointButtonPrefab, rowElementCountBeforeInstantiating, ID.Count - 1);

        List<int> parentID = new List<int>(ID);
        parentID.RemoveAt(parentID.Count - 1);
        newObject.GetComponent<AddBulletPointButtonManager>().parentBulletPointID = GetBulletPointIDstring(parentID);

        return newObject;
    }

    /// <summary>
    /// Checks the educationContent list and displays the according UI in the instructor menu. Also makes the 'totalPrompt' string and also saves the contents to a file.
    /// </summary>
    public void UpdateBulletPointDisplay()
    {
        saverAndLoader.SaveAll();

        //Remove all previously instantiated bullet point and addButton GameObjects (this doesn't include the bottom addButton).
        //We will be recreating them based on the updated contents of educationContent.
        for(int i = rowObjects.Count - 1; i >= 0; i--)
        {
            Destroy(rowObjects[i]);
        }

        //Clear the deleted GameObjects out of the List.
        rowObjects.Clear();

        totalPrompt = "";

        //This list contains everything yet to be instantiated, bullet points will be swapped out for their sub bullet points during the process.
        List<ContentBulletPoint> currentBulletPoints = new List<ContentBulletPoint>(educationContent);

        int rowElementCount = 0;
        List<int> currentID = new List<int>();

        currentID.Add(1);

        //One loop = One instantiated row. Loop ends when there's nothing left to instantiate.
        while(currentBulletPoints.Count > 0)
        {
            //How we are using null is explained in 2 below, but it basically means that a subgroup has ended.
            if(currentBulletPoints[0] == null)
            {
                //No addButton for a subgroup that has no bullet points.
                if (currentID[currentID.Count - 1] != 1)
                {
                    InstantiateAddButton(rowElementCount, currentID);
                    rowElementCount++;
                }

                //Remove the null.
                currentBulletPoints.RemoveAt(0);
                //Update ID of bullet point we are focused on.
                currentID.RemoveAt(currentID.Count - 1);
                currentID[currentID.Count - 1] += 1;

                continue;
            }

            //1 Instantite first bullet point in currentBulletPoints list.

            //2 If this first bullet point has subbullet points, then add them to the list at the start of the list.
            //  We will also add a null bulletpoint at the end of the subbullet points to indicate the end of the subgroup,
            //  even if there were no subbulletpoints, it will work just the same.

            //3 Delete the "parent" bullet point from the list.

            //Note: Remember indentation.

            totalPrompt += currentBulletPoints[0].contents + "\n";
            GameObject instantiatedObject = InstantiateBulletPoint(rowElementCount, currentID, currentBulletPoints[0].contents); //1
            rowElementCount++;

            //2 START
            currentID.Add(1);

            List<ContentBulletPoint> subBulletPoints = currentBulletPoints[0].subBulletPoints;

            for (int k = 0; k < subBulletPoints.Count; k++)
            {
                currentBulletPoints.Insert(k, subBulletPoints[k]);               
            }

            currentBulletPoints.Insert(subBulletPoints.Count, null); //2 END

            currentBulletPoints.RemoveAt(subBulletPoints.Count + 1); //3
        }

        addBulletPointButton.localPosition = new Vector3(addBulletPointButton.localPosition.x, addBulletPointButtonStartingY - 40 * rowElementCount);
    }
}
