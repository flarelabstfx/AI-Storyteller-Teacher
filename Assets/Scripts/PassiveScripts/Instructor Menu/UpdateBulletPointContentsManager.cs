//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateBulletPointContentsManager : MonoBehaviour
{
    public void OnBulletPointContentsUpdated(string newContents)
    {
        UIManager uIManager = transform.parent.parent.parent.GetComponent<UIManager>();
        uIManager.OnBulletPointInputFieldContentsUpdated(transform.parent.GetChild(2).GetComponent<TextMeshProUGUI>().text, newContents);
    }
}
