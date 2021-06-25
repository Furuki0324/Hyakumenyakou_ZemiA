using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCtrl : MonoBehaviour
{
    //------------------------Public------------------
    [Header("Set action key")]
    public KeyCode attackKey;


    //------------------------Private--------------------



    void Update()
    {
        if (Input.GetKeyDown(attackKey)) Attack();
    }

    private void Attack()
    {
        Debug.Log("Attack.");
    }
}
