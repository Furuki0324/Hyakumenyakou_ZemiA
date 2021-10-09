using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{
    [Header("Status")]
    public int hp = 1;
    public int attackPower = 1;
    public float attackInterval = 1;

    [Header("Attack effect")]
    public UnityEngine.Video.VideoPlayer attackEffect;
    protected UnityEngine.Video.VideoPlayer _effect;

    [Header("Option")]
    public AudioClip deadSound;
    public ParticleSystem deadParticle;


    public virtual void EnemyTakeDamage()
    {
        hp--;

        if (hp <= 0) EnemyDie();
    }

    public virtual void EnemyDie()
    {
        //if(deadSound)
        if (deadParticle) Instantiate(deadParticle, transform.position, Quaternion.identity);
        

        EnemyDropItemCtrl dropCtrl;

        Destroy(this.gameObject);

        PhaseManager.RemoveFromEnemyList(this);

        dropCtrl = GetComponent<EnemyDropItemCtrl>();
        if (!dropCtrl) return;

        dropCtrl.DroppingItem();
    }
}
