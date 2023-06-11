using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeleteBulletPointButtonManager : MonoBehaviour
{
    public void DeleteBulletPointButtonPressed()
    {
        UIManager uIManager = transform.parent.parent.parent.GetComponent<UIManager>();
        uIManager.DeleteBulletPointButtonPressed(transform.parent.GetChild(2).GetComponent<TextMeshProUGUI>().text);
    }
}
