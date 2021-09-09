using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{
    [Header("Status")]
    public int hp = 1;
    public int attackPower = 1;
    public float attackInterval = 1;


    public virtual void EnemyTakeDamage()
    {
        hp--;

        if (hp <= 0) EnemyDie();
    }

    public virtual void EnemyDie()
    {
        EnemyDropItemCtrl dropCtrl;

        Destroy(this.gameObject);

        PhaseManager.RemoveFromEnemyList(this);

        dropCtrl = GetComponent<EnemyDropItemCtrl>();
        if (!dropCtrl) return;

        dropCtrl.DroppingItem();
    }
}
