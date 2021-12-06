using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class FacePartsBaseScript : MonoBehaviour
{
    int usedElement = 0;


    #region Public variables

    [Header("Scale factor")]
    [Tooltip("素材消費量が1増えた際の耐久値の増加率")]
    [Range(0,1)]
    public float scale;

    [Header("HP")]
    public int health;
    protected int cacheHealth;
    protected bool hasBeenDead;

    [Header("Image")]
    [Tooltip("複数用意する場合は必ず番号の若いほうに耐久値が高いスプライトをセットしてください。")]
    public List<Sprite> sprites;

    [Header("Sound")]
    public AudioClip deadSound;
    [SerializeField] AudioMixerGroup deadMixer;
    [SerializeField] protected AudioSource deadSource;

    [Header("Other")]
    public bool immortal;

    #endregion

    #region Private variables

    protected SpriteRenderer spriteRenderer;
    /// <summary>
    /// <para>パーツが作り出されたときに値が決定します。</para>
    /// <para>用意されたスプライトの量に応じて「1 / 枚数」の式が実行されます。</para>
    /// </summary>
    protected float stepRatio;

    #endregion

    public void Initialize(int consumption)
    {
        usedElement = consumption;

        //複数のAudioSourceを設定する必要がある場合に備えてdeadSourceをSerializeにしてあります。
        //AudioSourceが単体の場合は自動的にセットされます。
        if (!deadSource)
        {
            deadSource = GetComponent<AudioSource>();

            //AudioSourceを一つも持っていない場合は追加します。
            if (!deadSource)
            {
                deadSource = gameObject.AddComponent<AudioSource>();
            }
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
        if(usedElement >= 0) { health = health + Mathf.FloorToInt(health * scale * usedElement); }
        else { Debug.LogError("No used element was passed."); }
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
        if (health <= 0 && !immortal && !hasBeenDead) FacePartsDie();
        else SetSpriteImage();
    }


    /// <summary>
    /// <para>ダメージ量を調整する必要がある場合に使用してください</para>
    /// </summary>
    /// <param name="damage">ダメージ量</param>
    public virtual void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0 && !immortal && !hasBeenDead) FacePartsDie();
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
        hasBeenDead = true;

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

        deadSource.outputAudioMixerGroup = deadMixer;
        deadSource.PlayOneShot(deadSound);
        yield return new WaitForSeconds(wait * 1.1f);
        
        Destroy(gameObject);
        yield return null;
    }
}
