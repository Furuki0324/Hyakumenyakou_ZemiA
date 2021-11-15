using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class FacePartsBaseScript : MonoBehaviour
{
    private void Awake()
    {
        Initialize();
    }

    #region Public variables

    [Header("Scale factor")]
    [Tooltip("素材消費量が1増えた際の耐久値の増加率")]
    [Range(0,1)]
    public float scale;

    [Header("HP")]
    public int health;
    protected int cacheHealth;

    [Header("Image")]
    public List<Sprite> sprites;

    [Header("Sound")]
    protected AudioSource audioSource;
    public AudioClip deadSound;
    [SerializeField] AudioMixerGroup deadMixer;

    [Header("Other")]
    public bool immortal;

    #endregion

    #region Private variables

    protected SpriteRenderer spriteRenderer;
    protected float stepRatio;

    #endregion

    private void Initialize()
    {
        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (sprites.Count > 0) stepRatio = 1.0f / sprites.Count;
        else Debug.LogWarning("No sprites is set to " + gameObject.name);


        SetCache();
        SetSpriteImage();
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
        else SetSpriteImage();
    }


    /// <summary>
    /// <para>ダメージ量を調整する必要がある場合に使用してください</para>
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !immortal) FacePartsDie();
        else SetSpriteImage();
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
        //Fail safe
        if (health > cacheHealth) return;

        health += amount;
        SetSpriteImage();
    }

    /// <summary>
    /// 割合回復
    /// </summary>
    /// <param name="percent">0~100の間で入力</param>
    public virtual void Repaired(float percent)
    {
        //Fail safe
        if (health > cacheHealth || percent < 0 || percent > 100) return;

        float ratio = percent / 100;

        health += (int)(cacheHealth * ratio);
        if (health > cacheHealth) health = cacheHealth;
        SetSpriteImage();
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

    private void SetSpriteImage()
    {
        Sprite newSprite = null;
        float lifePercent = (float)health / cacheHealth;


        for(int i = 0; i < sprites.Count; i++)
        {

            if(lifePercent <= 1 - stepRatio * i)
            {
                newSprite = sprites[i];
            }
        }

        spriteRenderer.sprite = newSprite;
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

        audioSource.outputAudioMixerGroup = deadMixer;
        audioSource.PlayOneShot(deadSound);
        yield return new WaitForSeconds(wait * 1.1f);
        
        Destroy(gameObject);
        yield return null;
    }
}
