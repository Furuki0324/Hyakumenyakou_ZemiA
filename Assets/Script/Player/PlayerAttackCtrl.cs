using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(AudioSource))]
public class PlayerAttackCtrl : MonoBehaviour
{
    //------------------------Public------------------
    [Header("Set action key")]
    public KeyCode attackKey;

    [Header("SFX")]
    [SerializeField] AudioClip sound;
    private AudioSource audioSource;

    [Header("VFX")]
    [SerializeField] DestroyFXWhenFinishPlaying effect;
    private DestroyFXWhenFinishPlaying _effect;
    private AttackCollisionControl collisionControl;


    //------------------------Private--------------------
    EdgeCollider2D edgeCollider;
    Vector3 scale;

    private void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.enabled = false;

        audioSource = GetComponent<AudioSource>();
        
        _effect = Instantiate(effect, GameObject.Find("EffectCanvas").transform);
        collisionControl = _effect.GetComponent<AttackCollisionControl>();

        scale = _effect.transform.localScale;
        
    }


    void Update()
    {
        _effect.transform.position = transform.position;

        if (Input.GetKeyDown(attackKey)) Attack();

        CollisionCheck();


        Vector3 newScale = scale;
        newScale.x *= -transform.parent.localScale.x;
        _effect.transform.localScale = newScale;
    }


    private void Attack()
    {
        audioSource.PlayOneShot(sound);
        _effect.StartTheCoroutine(DestroyFXWhenFinishPlaying.Pattern.play); 
    }

    private void CollisionCheck()
    {
        edgeCollider.enabled = collisionControl.IsColliderEnabled();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBaseScript enemy = collision.gameObject.GetComponent<EnemyBaseScript>();
        if (enemy)
        {
            enemy.EnemyTakeDamage();
        }
    }
}
