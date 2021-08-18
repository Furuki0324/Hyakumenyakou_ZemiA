using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EveryTester : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("GMTester", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GMTester()
    {
        //Debug.Log("Invoked.");
    }
}
