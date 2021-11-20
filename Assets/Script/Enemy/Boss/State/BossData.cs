using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossData", menuName = "Zemi_Group_A/BossData", order = 0)]
public class BossData : ScriptableObject
{
    //現在状態
    public State nowState = State.highS;
    public enum State
    {
        pos,
        damC,
        highS,
        noP
    }

    public const string PATH = "BossData";
    private static BossData _bossData;
    public static BossData bossData
    {
        get
        {
            if (_bossData == null)
            {
                _bossData = Resources.Load<BossData>(PATH);
            }
            if (_bossData == null)
            {
                Debug.LogError(PATH + " not found");
            }
            return _bossData;
        }
    }

    [Header("Values of Possession state")]
    [Tooltip("憑依対象へのダメージ")]public int possAttackPowToParts = 1;
    [Tooltip("憑依対象へのダメージ間隔")]public int possAttackToPartsInterval = 1;
    [Tooltip("憑依時の攻撃がどこまで飛んだら消えるか")]public float attacksRange = 1.0f;
    
    [Header("Ear Value")]
    [Tooltip("耳憑依時の攻撃力")]public int earAttackPower = 1;
    [Tooltip("耳憑依時の弾の発射間隔")]public float earAttackInterval = 3.0f;
    [Tooltip("耳憑依時の弾の速さ")]public float earAttackSpeed = 2.0f;

    [Header("Eye Value")]
    [Tooltip("目憑依時の攻撃力")]public int eyeAttackPower = 2;
    [Tooltip("目憑依時の弾の発射間隔")]public float eyeAttackInterval = 5.0f;
    [Tooltip("目憑依時の弾の速さ")]public float eyeAttackSpeed = 2.0f;

    [Header("Mouth Value")]
    [Tooltip("口憑依時の攻撃力")]public int mouthAttackPower = 5;
    [Tooltip("口憑依時の弾の発射間隔")]public float mouthAttackInterval = 10.0f;
    [Tooltip("口憑依時の弾の速さ")]public float mouthAttackSpeed = 2.0f;
    [Tooltip("口憑依時の弾の曲率")]public float mouthAttackCurve = 1.0f;
    
    [Header("Values of DamageChance state")]
    [Tooltip("耳から憑依解除されたときの下移動、距離")]public float moveUnderEar = 1f;
    [Tooltip("耳から憑依解除されたときの下移動、時間")]public float moveTimeEar = 1f;
    [Tooltip("目から憑依解除されたときの下移動、距離")]public float moveUnderEye = 1f;
    [Tooltip("目から憑依解除されたときの下移動、時間")]public float moveTimeEye = 1f;
    [Tooltip("口から憑依解除されたときの下移動、距離")]public float moveUnderMouth = 1f;
    [Tooltip("口から憑依解除されたときの下移動、時間")]public float moveTimeMouth = 1f;
    [Tooltip("憑依解除時、移動する際の無敵判定（デバッグ用）")]public bool invincible = false;


    [Header("Values of HighSpeed state")]
    [Tooltip("パーツへ憑依しに行く時のボスの速さ")]public float hsSpeed = 1.0f;

    [Header("Values of NoParts state")]
    [Tooltip("パーツが無い時の弾の攻撃力")]public int noPAttackPowOfBullet;
    [Tooltip("パーツが無い時の弾の発射間隔")]public float noPAttackIntervalOfBullet;
    [Tooltip("パーツが無い時の弾の速度")]public float noPAttackSpeed = 2.0f;
    [Tooltip("パーツが無い時のボスが移動方向を変えるまでの時間")]public float noPRandWalkInterval;
    [Tooltip("パーツが無い時のボスの移動速度")]public float noPRandWalkSpeed;

}
