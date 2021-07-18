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
        Debug.Log(gameObject.name + " - TakeDamage method has invoked.");
    }

    private float cacheTime = 0;
    private void Update()
    {
        if (health < 0) Dead();

        if(enemiesNearby.Count > 0)
        {
            if(Time.time > cacheTime + interval)
            {
                TakeDamage();
                cacheTime = Time.time;
            }
        }
    }


    /// <summary>
    /// <para>ダメージ量を調整する必要がある場合に使用してください</para>
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public virtual void TakeDamage(int damage) { }

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
        if(enemiesNearby.Count > 0)
        {
            for(int i = 0; i < enemiesNearby.Count; i++)
            {
                Destroy(this.gameObject);
                enemiesNearby[i].ResetTarget();
            }
        }
    }
}
