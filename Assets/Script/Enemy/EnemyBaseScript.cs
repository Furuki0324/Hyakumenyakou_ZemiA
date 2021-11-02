using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    

    #endregion

    private void Awake()
    {
        if (attackEffect) _effect = Instantiate(attackEffect, GameObject.Find("EffectCanvas").transform);
    }

    public virtual void EnemyTakeDamage()
    {
        hp--;
        if(damageSound) EnemySoundPlayer.PlayEnemySFX(damageSound);

        if (hp <= 0) EnemyDie();
    }

    public virtual void EnemyDie()
    {
        if (deadSound) EnemySoundPlayer.PlayEnemySFX(deadSound);
        if (deadParticle) Instantiate(deadParticle, transform.position, Quaternion.identity);

        if (_effect)
        {
            DestroyFXWhenFinishPlaying fx = _effect.GetComponent<DestroyFXWhenFinishPlaying>();
            if (attackEffect && fx) fx.StartTheCoroutine(DestroyFXWhenFinishPlaying.Pattern.destroy);
        }

        SaveData.AchievementStep(Achievement.StepType.killEnemy_ResetForEachPlay);
        SaveData.AchievementStep(Achievement.StepType.killEnemy_Cumulative);

        EnemyDropItemCtrl dropCtrl;

        Destroy(this.gameObject);

        PhaseManager.RemoveFromEnemyList(this);

        dropCtrl = GetComponent<EnemyDropItemCtrl>();
        if (!dropCtrl) return;

        dropCtrl.DroppingItem();
    }
}
