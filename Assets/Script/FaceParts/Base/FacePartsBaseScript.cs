using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePartsBaseScript : MonoBehaviour
{
    [Header("Scale factor")]
    [Tooltip("素材消費量が1増えた際の耐久値の増加率")]
    [Range(0,1)]
    public float scale;

    [Header("Setting in base script")]
    public int health;
    protected int cacheHealth;
    


    private void Start()
    {
        TakeDamage();
        SetCache();
    }

    public void SetCache()
    {
        health = health + Mathf.FloorToInt(health * scale * (DropItemManager.GetSpendingElementFromHolder() - 1));
        cacheHealth = health;
        Debug.Log("CacheHealth is " + cacheHealth);
    }

    /// <summary>
    /// <para>各顔面パーツが攻撃されたときに使用されます。</para>
    /// <para>目・耳・口の各パーツのスクリプトでoverrideして使用してください。</para>
    /// </summary>
    public virtual void TakeDamage()
    {
        health--;
        //Debug.Log(gameObject.name + " - TakeDamage method has invoked.");
        if (health <= 0) FacePartsDie();
    }


    /// <summary>
    /// <para>ダメージ量を調整する必要がある場合に使用してください</para>
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) FacePartsDie();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EnemyCtrl enemyCtrl = collision.gameObject.GetComponent<EnemyCtrl>();
        if(enemyCtrl != null && enemyCtrl.GetFacePart() == null)
        {
            enemyCtrl.SetFaceScript(this);
        }
    }


    public virtual void Repaired(int amount)
    {
        
        health += amount;
        if (health > cacheHealth) health = cacheHealth;
    }

    /// <summary>
    /// <para>耐久値が減少している場合はtrueを返します。</para>
    /// </summary>
    /// <returns></returns>
    public virtual bool Damaged()
    {
        if (health < cacheHealth)
        {
            return true;
        }
        else
        {
            Debug.Log("Health is full.");
            Debug.Log("cacheHealth is " + cacheHealth);
            return false;
        }
    }


    public virtual void FacePartsDie()
    {
        if (health > 0)
        {
            //Debug.Log("Do not die");
            return;
        }

        Destroy(gameObject);
        MainScript.RemoveFaceObject(this.gameObject);
    }
}
