using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This should be the only script that runs code.
 */

public class TheOnlyActiveScript : MonoBehaviour
{

    public AIModelAccess aIModelAccess;

    // Start is called before the first frame update
    void Start()
    {
        //aIModelAccess.RunTest();
    }

    // Update is called once per frame
    void Update()
    {
        aIModelAccess.CustomUpdate();
    }
}
