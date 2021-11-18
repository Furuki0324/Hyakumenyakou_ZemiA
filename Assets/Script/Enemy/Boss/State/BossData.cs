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
    public int possAttackPowToParts = 1;
    public int possAttackToPartsInterval = 1;
    public float attacksRange = 1.0f;

    [Header("Values of DamageChance state")]
    public float moveUnderEar = 1f;
    public float moveTimeEar = 1f;
    public float moveUnderEye = 1f;
    public float moveTimeEye = 1f;
    public float moveUnderMouth = 1f;
    public float moveTimeMouth = 1f;
    public bool invincible = false;
    
    [Header("Ear Value")]
    public int earAttackPower = 1;
    public float earAttackInterval = 3.0f;
    public float earAttackSpeed = 2.0f;

    [Header("Eye Value")]
    public int eyeAttackPower = 2;
    public float eyeAttackInterval = 5.0f;
    public float eyeAttackSpeed = 2.0f;

    [Header("Mouth Value")]
    public int mouthAttackPower = 5;
    public float mouthAttackInterval = 10.0f;
    public float mouthAttackSpeed = 2.0f;
    public float mouthAttackCurve = 1.0f;

    [Header("Values of HighSpeed state")]
    public float hsSpeed = 1.0f;

    [Header("Values of NoParts state")]
    public int noPAttackPowOfBullet;
    public float noPAttackIntervalOfBullet;
    public float noPAttackSpeed = 2.0f;
    public float noPRandWalkInterval;
    public float noPRandWalkSpeed;

}
