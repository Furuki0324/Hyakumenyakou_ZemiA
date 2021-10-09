using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePartsBaseScript : MonoBehaviour
{
    private void Awake()
    {
        Initialize();
    }


    [Header("Scale factor")]
    [Tooltip("素材消費量が1増えた際の耐久値の増加率")]
    [Range(0,1)]
    public float scale;

    [Header("HP")]
    public int health;
    protected int cacheHealth;

    [Header("Sound")]
    protected AudioSource audioSource;
    public AudioClip deadSound;

    [Header("Other")]
    public bool immortal;

    private void Initialize()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        SetCache();
        Debug.Log("Initialized.");
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
        if (health <= 0 && !immortal) FacePartsDie();
    }


    /// <summary>
    /// <para>ダメージ量を調整する必要がある場合に使用してください</para>
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !immortal) FacePartsDie();
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
        if (deadSound)
        {
            StartCoroutine(FacePartsDestroyAfterSound(deadSound.length));
            return;
        }

        if (health > 0)
        {
            //Debug.Log("Do not die");
            return;
        }

        Destroy(gameObject);
        MainScript.RemoveFaceObject(this.gameObject);
    }

    public virtual IEnumerator FacePartsDestroyAfterSound(float wait)
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;


        audioSource.PlayOneShot(deadSound);
        yield return new WaitForSeconds(wait * 1.1f);
        
        Destroy(gameObject);
        yield return null;
    }
}
