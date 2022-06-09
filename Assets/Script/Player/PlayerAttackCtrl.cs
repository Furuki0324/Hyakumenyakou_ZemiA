using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[RequireComponent(typeof(AudioSource))]
public class PlayerAttackCtrl : MonoBehaviour
{
    //------------------------Public------------------
    [Header("Attack")]
    [SerializeField] KeyCode attackKey;
    [SerializeField] float attackDuration = 0.3f;
    [SerializeField] float heavyAttackCoolTime = 0.5f;
    [SerializeField] Slider coolTimeIndicator;
    [SerializeField] Image coolTimeSliderFillImage;
    [SerializeField] Color heavyAttackRechargingColor;

    [Header("SFX")]
    [SerializeField] AudioClip sound;
    [SerializeField] AudioClip heavyAttackRechargedSound;
    private AudioSource audioSource;

    [Header("VFX")]
    [SerializeField] DestroyFXWhenFinishPlaying effect;
    private DestroyFXWhenFinishPlaying _effect;


    //------------------------Private--------------------
    Vector3 scale;
    Player2DMovement playerMovement;
    Rigidbody2D parentRigidbody;

    Vector3 direction = new Vector3();
    bool isAttacking = false;
    bool isHeavyAttackReady = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        _effect = Instantiate(effect, GameObject.Find("EffectCanvas").transform);

        scale = _effect.transform.localScale;
        playerMovement = GetComponentInParent<Player2DMovement>();
        parentRigidbody = GetComponentInParent<Rigidbody2D>();

        if(coolTimeIndicator)
        {
            coolTimeIndicator.value = 1;
        }
    }


    void Update()
    {
        // [elseを使っていない理由]
        // プレイヤーが移動を止めた瞬間の方向を維持するためにelseを使っていません。
        if(parentRigidbody.velocity.x < 0)
        {
            direction.x = -1;
        }
        if(parentRigidbody.velocity.x > 0)
        {
            direction.x = 1;
        }
        transform.localScale = direction;

        _effect.transform.position = transform.position;

        if (Input.GetKeyDown(attackKey)) Attack();

        Vector3 newScale = scale;
        newScale.x *= -transform.localScale.x;
        _effect.transform.localScale = newScale;
    }


    private void Attack()
    {
        if(!isAttacking && Time.timeScale != 0)
        {
            if(Input.GetKey(KeyCode.LeftShift))
            {
                if (!isHeavyAttackReady)
                {
                    return; 
                }
                if(coolTimeIndicator)
                {
                    coolTimeSliderFillImage.color = heavyAttackRechargingColor;
                    coolTimeIndicator.value = 0.0f;
                }
                isAttacking = true;
                isHeavyAttackReady = false;
                float duration = attackDuration * 1.5f;
                StartCoroutine(FinishAttacking(duration, heavyAttackCoolTime));
                playerMovement.StartAttack(duration, true);
            }
            else
            {
                isAttacking = true;
                float duration = attackDuration;
                StartCoroutine(FinishAttacking(duration, 0.0f));
                playerMovement.StartAttack(duration);
            }

            
            audioSource.PlayOneShot(sound);
            _effect.StartTheCoroutine(DestroyFXWhenFinishPlaying.Pattern.play);
        }
    }

    private IEnumerator FinishAttacking(float duration, float cool)
    {
        yield return new WaitForSeconds(duration);
        if(cool <= 0.0f)
        {
            isAttacking = false;
            isHeavyAttackReady = true;
        }
        else
        {
            isAttacking = false;
            StartCoroutine(CheckCoolTime(cool));
        }
    }

    private IEnumerator CheckCoolTime(float cool)
    {
        isHeavyAttackReady = false;

        for(float passedTime = 0.0f; passedTime < cool; passedTime += Time.deltaTime)
        {
            if (coolTimeIndicator) 
            {
                coolTimeIndicator.value = passedTime / cool;
            }
            yield return null;
        }

        if(heavyAttackRechargedSound)
        {
            audioSource.PlayOneShot(heavyAttackRechargedSound);
        }
        isHeavyAttackReady = true;
        if (coolTimeIndicator)
        {
            coolTimeSliderFillImage.color = new Color(1, 1, 1);
            coolTimeIndicator.value = 1.0f; 
        }
    }
}
