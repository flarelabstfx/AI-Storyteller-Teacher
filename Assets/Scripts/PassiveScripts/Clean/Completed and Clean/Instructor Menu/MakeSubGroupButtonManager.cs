using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MakeSubGroupButtonManager : MonoBehaviour
{
    public void MakeSubGroupButtonPressed()
    {
        UIManager uIManager = transform.parent.parent.parent.GetComponent<UIManager>();
        uIManager.MakeSubGroupButtonPressed(transform.parent.GetChild(2).GetComponent<TextMeshProUGUI>().text);
    }
}
