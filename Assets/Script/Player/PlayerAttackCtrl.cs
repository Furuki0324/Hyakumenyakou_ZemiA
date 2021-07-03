using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCtrl : MonoBehaviour
{
    //------------------------Public------------------
    [Header("Set action key")]
    public KeyCode attackKey;

    [Header("Attack Component")]
    public GameObject attackParticle;


    //------------------------Private--------------------
    


    void Update()
    {
        if (Input.GetKeyDown(attackKey)) Attack();
    }

    private void Attack()
    {
        attackParticle.SetActive(true);

        Invoke("DeactivateAttack", 0.2f);
    }

    private void DeactivateAttack()
    {
        attackParticle.SetActive(false);
    }
}
