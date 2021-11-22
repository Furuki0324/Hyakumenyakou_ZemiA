//#define SAVE

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
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
    public AudioClip damageSound;
    public AudioClip deadSound;
    public ParticleSystem deadParticle;

    #region private variables

    protected Rigidbody2D rigid2D;

    #endregion

    private void Awake()
    {
        rigid2D = GetComponent<Rigidbody2D>();
        if (attackEffect) _effect = Instantiate(attackEffect, GameObject.Find("EffectCanvas").transform);
    }

    public virtual void EnemyTakeDamage()
    {
        hp--;
        if(damageSound) EnemySoundPlayer.PlayEnemySFX(damageSound);

        if (hp <= 0) EnemyDie();
    }

    public virtual async Task EnemyDie()
    {
        await KnockBack();

        if (deadSound) EnemySoundPlayer.PlayEnemySFX(deadSound);
        if (deadParticle) Instantiate(deadParticle, transform.position, Quaternion.identity);

        if (_effect)
        {
            DestroyFXWhenFinishPlaying fx = _effect.GetComponent<DestroyFXWhenFinishPlaying>();
            if (attackEffect && fx) fx.StartTheCoroutine(DestroyFXWhenFinishPlaying.Pattern.destroy);
        }

#if SAVE
        SaveData.AchievementStep(Achievement.StepType.killEnemy_ResetForEachPlay);
        SaveData.AchievementStep(Achievement.StepType.killEnemy_Cumulative);
#endif

        EnemyDropItemCtrl dropCtrl;

        Destroy(this.gameObject);

        PhaseManager.RemoveFromEnemyList(this);

        dropCtrl = GetComponent<EnemyDropItemCtrl>();
        if (!dropCtrl) return;

        dropCtrl.DroppingItem();
    }

    public virtual async Task KnockBack()
    {
        Vector3 playerPosition = GameObject.FindWithTag("Player").transform.position;

        Vector2 direction = (transform.position - playerPosition).normalized;

        rigid2D.velocity = Vector2.zero;

        Vector2 currentPosition;
        Vector2 nextPosition;
        Vector2 move;
        float a;
        for(float i = 0; i < 0.1f; i += Time.unscaledDeltaTime)
        {
            a = Mathf.Lerp(1, 0, i / 0.3f);
            move = direction * a * 0.2f;

            currentPosition = transform.position;
            nextPosition = currentPosition + move;

            transform.position = nextPosition;
            await Task.Delay(10);
        }
    }
}
