//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class AddBulletPointButtonManager : MonoBehaviour
{

    public string parentBulletPointID;

    public void OnAddBulletPointButtonPressed()
    {
        //Pass to ui manager with id info.
        UIManager uIManager = transform.parent.parent.GetComponent<UIManager>();
        uIManager.AddBulletPointButtonPressed(parentBulletPointID);
    }
}
