using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This should be the only script that runs code.
 */

public class TheOnlyActiveScript : MonoBehaviour
{
    [Header("Refferences:")]
    public AIModelAccess aIModelAccess;
    public SaverAndLoader saverAndLoader;
    public InstructorMenuManager instructorMenuManager;

    void Awake()
    {
        instructorMenuManager.CustomAwake();
    }

    // Start is called before the first frame update
    void Start()
    {
        saverAndLoader.LoadAll();
    }

    // Update is called once per frame
    void Update()
    {
        //aIModelAccess.CustomUpdate();
    }
}
