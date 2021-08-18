using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePartsBaseScript : MonoBehaviour
{
    [Header("Setting in base script")]
    public int health;
    protected int cacheHealth;
    public float interval;
    public List<EnemyCtrl> enemiesNearby = new List<EnemyCtrl>();


    private void Start()
    {
        TakeDamage();
        cacheHealth = health;
    }

    /// <summary>
    /// <para>各顔面パーツが攻撃されたときに使用されます。</para>
    /// <para>目・耳・口の各パーツのスクリプトでoverrideして使用してください。</para>
    /// </summary>
    public virtual void TakeDamage()
    {
        //Debug.Log(gameObject.name + " - TakeDamage method has invoked.");
        if (health <= 0) Dead();
    }

    private float cacheTime = 0;



    /// <summary>
    /// <para>ダメージ量を調整する必要がある場合に使用してください</para>
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public virtual void TakeDamage(int damage)
    {
        if (health <= 0) Dead();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyCtrl enemyCtrl = collision.gameObject.GetComponent<EnemyCtrl>();
        if(enemyCtrl != null && enemyCtrl.GetFacePart() == null)
        {
            enemyCtrl.SetFaceScript(this);
            enemiesNearby.Add(enemyCtrl);
        }
    }

    public void RemoveEnemyFromList(EnemyCtrl enemy)
    {
        enemiesNearby.Remove(enemy);
    }


    public virtual void Dead()
    {
        if (health > 0)
        {
            Debug.Log("Do not die");
            return;
        }

        Destroy(gameObject);
        MainScript.RemoveFaceObject(this.gameObject);

        /*
        if (enemiesNearby.Count > 0)
        {
            for(int i = 0; i < enemiesNearby.Count; i++)
            {
                Destroy(this.gameObject);
                MainScript.RemoveFaceObject(this.gameObject);
                enemiesNearby[i].ResetTarget();
            }
        }
        */
    }
}
