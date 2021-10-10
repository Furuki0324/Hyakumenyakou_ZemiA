using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PlayerAttackCtrl : MonoBehaviour
{
    //------------------------Public------------------
    [Header("Set action key")]
    public KeyCode attackKey;

    [Header("Effect")]
    [SerializeField] DestroyFXWhenFinishPlaying effect;
    private DestroyFXWhenFinishPlaying _effect;
    private VideoPlayer videoPlayer;


    //------------------------Private--------------------
    EdgeCollider2D edgeCollider;
    Vector3 scale;

    private void Start()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        edgeCollider.enabled = false;

        
        _effect = Instantiate(effect, GameObject.Find("EffectCanvas").transform);
        videoPlayer = _effect.GetComponent<VideoPlayer>();

        scale = _effect.transform.localScale;
        
    }


    void Update()
    {
        _effect.transform.position = transform.position;

        if (Input.GetKeyDown(attackKey)) Attack();

        edgeCollider.enabled = videoPlayer.isPlaying;

        Vector3 newScale = scale;
        newScale.x *= -transform.parent.localScale.x;
        _effect.transform.localScale = newScale;
    }

    private void Attack()
    {
        _effect.StartTheCoroutine(DestroyFXWhenFinishPlaying.Pattern.play); 
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
