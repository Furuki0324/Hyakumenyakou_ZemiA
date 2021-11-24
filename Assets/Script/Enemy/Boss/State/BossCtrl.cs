using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BossCtrl : EnemyBaseScript
{
    #region PrivateField

    private IBossStateRoot bp, bdc, bhs, bnp;

    //PossessEffect
    [SerializeField] private GameObject pe;
    private GameObject nowPe;

    private PhaseManager time;
    private Renderer bRenderer;

    private enum Tags
    {
        Face_Eye,
        Face_Mouth,
        Face_Ear
    }

    private bool takeDamage;

    public static Transform formerPossess;
    private bool throughFlag;
    private int formerLayerNum;

    private SpriteRenderer bossSprite;
    private float bossColliderYPer2;

    #endregion

    void Start()
    {
        bp = gameObject.GetComponent<BossPossesion>();
        bdc = gameObject.GetComponent<BossDamChance>();
        bhs = gameObject.GetComponent<BossHighSpeed>();
        bnp = gameObject.GetComponent<BossNoParts>();
        time = Camera.main.GetComponent<PhaseManager>();
        BossData.bossData.nowState = BossData.State.highS;
        bRenderer = GetComponent<Renderer>();
        BossDeepData.GetBDpData.bRigid = GetComponent<Rigidbody2D>();
        takeDamage = false;
        throughFlag = false;
        bossSprite = gameObject.GetComponent<SpriteRenderer>();
        bossColliderYPer2 = GetComponent<BoxCollider2D>().size.y / 2f;
    }

    void Update()
    {
        if (time.time <= 0)
        {
            switch (BossData.bossData.nowState)
            {
                case BossData.State.pos:
                    bp.attack();
                    if (BossDeepData.GetBDpData.toPossessParts == null)
                    {
                        bp.stopHavingAllCoroutine();
                        unPossession();
                        bhs.First = true;
                        BossData.bossData.nowState = BossData.State.highS;
                    }

                    //ここをいじって下にフェード
                    if (takeDamage)
                    {
                        bp.stopHavingAllCoroutine();
                        takeDamage = false;
                        unPossession();
                        bdc.First = true;
                        BossData.bossData.nowState = BossData.State.damC;
                    }

                    break;

                case BossData.State.damC:
                    bdc.move();
                    if (!BossData.bossData.invincible)
                    {
                        if (takeDamage)
                        {
                            bdc.stopHavingAllCoroutine();

                           
                            // if (formerPossess != null)
                            //     formerPossess.gameObject.layer = LayerMask.NameToLayer("BossThroughFormer");
                            //憑依してたパーツにぶつからないようレイヤー変更
                            if (formerPossess != null) formerPossess.gameObject.layer = LayerMask.NameToLayer("Face");

                            takeDamage = false;
                            bhs.First = true;
                            BossDeepData.GetBDpData.bRigid.bodyType = RigidbodyType2D.Dynamic;
                            BossData.bossData.nowState = BossData.State.highS;
                        }
                    }

                    break;
                case BossData.State.highS:
                    bhs.move();
                    //もしぶつかったら憑依、憑依状態へ
                    if (BossDeepData.GetBDpData.toPossessParts != null)
                    {
                        bhs.stopHavingAllCoroutine();
                        possession();
                        bp.First = true;
                        BossData.bossData.nowState = BossData.State.pos;
                    }

                    //もしぶつかる候補が無かったらパーツ無し状態へ
                    if (BossDeepData.GetBDpData.Transforms.Count <= 0)
                    {
                        bhs.stopHavingAllCoroutine();
                        bnp.First = true;
                        BossData.bossData.nowState = BossData.State.noP;
                    }

                    break;
                case BossData.State.noP:
                    //NoParts処理
                    bnp.move();
                    break;
            }
        }
    }

    //憑依メソッド、レンダラ消すだけ
    void possession()
    {
        bRenderer.enabled = false;
        BossDeepData.GetBDpData.bRigid.bodyType = RigidbodyType2D.Kinematic;
        BossDeepData.GetBDpData.bRigid.velocity = Vector3.zero;
        var position = BossDeepData.GetBDpData.toPossessParts.position;
        transform.position = position;
        //憑依エフェクトを生産
        nowPe = Instantiate(pe, position, Quaternion.identity);

        
    }

    void unPossession()
    {
        bRenderer.enabled = true;

        BossDeepData.GetBDpData.bRigid.velocity = Vector3.zero;
        formerPossess = BossDeepData.GetBDpData.toPossessParts;
        BossDeepData.GetBDpData.toPossessParts = null;
        
        var bossTempColor = bossSprite.color;
        var moveValue = 0f;
        var moveTime = 0f;

        switch (formerPossess.tag)
        {
            case "Face_Ear":
                moveValue = BossData.bossData.moveUnderEar;
                moveTime = BossData.bossData.moveTimeEar;
                break;
            case "Face_Eye":
                moveValue = BossData.bossData.moveUnderEye;
                moveTime = BossData.bossData.moveTimeEye;
                break;
            case "Face_Mouth":
                moveValue = BossData.bossData.moveUnderMouth;
                moveTime = BossData.bossData.moveUnderMouth;
                break;
            default:
                moveValue = BossData.bossData.moveUnderEar;
                moveTime = BossData.bossData.moveTimeEar;
                break;
        }

        ; //formerPossess.GetComponent<BoxCollider2D>().size.y / 2f;
        transform.DOMoveY(transform.position.y - moveValue, moveTime)
            .OnStart(() =>
            {
                BossData.bossData.invincible = true;
                bossSprite.color = new Color(bossTempColor.r, bossTempColor.g, bossTempColor.b, 0f);
            })
            .OnUpdate(() => bossSprite.color += new Color(0, 0, 0, 1f / moveTime * Time.deltaTime))
            .OnComplete(() =>
            {
                BossData.bossData.invincible = false;
                bossSprite.color = new Color(bossTempColor.r, bossTempColor.g, bossTempColor.b, 1f);
            });
        Destroy(nowPe.gameObject);
    }

    //とりあえずボスが倒されたらゲームクリアのメソッドを呼ぶ記述をしていますが、必要に応じて変更してください。
    //ボスが撃破されたらゲームクリア
    public override void EnemyDie()
    {
        base.EnemyDie();
        _ = MainScript.GameClear(); //まだデバッグ出力がされるのみです。
    }

    public override void EnemyTakeDamage()
    {
        takeDamage = true;
        base.EnemyTakeDamage();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (BossData.bossData.nowState != BossData.State.highS || formerPossess == other.transform) return;
        foreach (string i in Enum.GetNames(typeof(Tags)))
        {
            if (other.gameObject.CompareTag(i))
            {
                BossDeepData.GetBDpData.toPossessParts = other.transform;
            }
        }
    }
}