using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCtrl : EnemyBaseScript
{
    #region PrivateField
    private IBossStateRoot bp, bdc, bhs, bnp;

    //PossessEffect
    [SerializeField]
    private GameObject pe;
    private GameObject nowPe;

    private PhaseManager time;
    private Renderer bRenderer;
    private enum tags
    {
        Face_Eye,
        Face_Mouth,
        Face_Ear
    }

    private bool takeDamage;

    [SerializeField]
    private Transform formerPossess;
    private bool throughFlag;
    private int formerLayerNum;

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
                    if (takeDamage)
                    {
                        bdc.stopHavingAllCoroutine();
                        if (formerPossess != null) formerPossess.gameObject.layer = LayerMask.NameToLayer("BossThroughFormer");
                        takeDamage = false;
                        bhs.First = true;
                        BossData.bossData.nowState = BossData.State.highS;
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
        transform.position = BossDeepData.GetBDpData.toPossessParts.position;
        nowPe = Instantiate(pe, BossDeepData.GetBDpData.toPossessParts.position, Quaternion.identity);

        if (formerPossess != null) formerPossess.gameObject.layer = LayerMask.NameToLayer("Face");
    }

    void unPossession()
    {
        bRenderer.enabled = true;
        BossDeepData.GetBDpData.bRigid.bodyType = RigidbodyType2D.Dynamic;
        BossDeepData.GetBDpData.bRigid.velocity = Vector3.zero;
        formerPossess = BossDeepData.GetBDpData.toPossessParts;

        //formerPossess.gameObject.layer = LayerMask.NameToLayer("BossThroughFormer");

        BossDeepData.GetBDpData.toPossessParts = null;
        //ランダムな方向に弾かれて出てくる
        transform.position += new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), 0);
        Destroy(nowPe.gameObject);
    }

    //とりあえずボスが倒されたらゲームクリアのメソッドを呼ぶ記述をしていますが、必要に応じて変更してください。
    //ボスが撃破されたらゲームクリア
    public override void EnemyDie()
    {
        base.EnemyDie();
        MainScript.GameClear(); //まだデバッグ出力がされるのみです。
    }

    public override void EnemyTakeDamage()
    {
        hp--;
        takeDamage = true;
        if (hp <= 0) EnemyDie();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (BossData.bossData.nowState == BossData.State.highS)
        {
            if (formerPossess != other.transform)
            {
                foreach (string i in Enum.GetNames(typeof(tags)))
                {
                    if (other.gameObject.CompareTag(i))
                    {
                        BossDeepData.GetBDpData.toPossessParts = other.transform;
                    }
                }
            }
            // else if (throughFlag = false)
            // {
            //     // gameObject.layer = LayerMask.NameToLayer("BossThroughFormer");
            //     BossDeepData.GetBDpData.bRigid.bodyType = RigidbodyType2D.Kinematic;
            //     throughFlag = true;
            // }
        }
    }

    // private void OnCollisionStay2D(Collision2D other)
    // {
    //     if (BossData.bossData.nowState == BossData.State.highS &&
    //         formerPossess == other.transform &&
    //         throughFlag == false)
    //     {
    //         // gameObject.layer = LayerMask.NameToLayer("BossThroughFormer");
    //         BossDeepData.GetBDpData.bRigid.bodyType = RigidbodyType2D.Kinematic;
    //         throughFlag = true;
    //     }
    // }

    // private void OnCollisionExit2D(Collision2D other)
    // {

    //     if (BossData.bossData.nowState == BossData.State.highS && throughFlag == true)
    //         {
    //         // gameObject.layer = LayerMask.NameToLayer("Enemy");
    //         BossDeepData.GetBDpData.bRigid.bodyType = RigidbodyType2D.Dynamic;
    //         throughFlag = false;
    //     }

    // }

    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     // if (BossData.bossData.nowState == BossData.State.highS &&
    //     //     formerPossess == other.transform &&
    //     //     throughFlag == false)
    //     // {
    //     //     BossDeepData.GetBDpData.bRigid.bodyType = RigidbodyType2D.Kinematic;
    //     //     throughFlag = true;
    //     // }
    //     if (BossData.bossData.nowState == BossData.State.highS &&
    //         formerPossess == other.transform) //&&
    //                                           //throughFlag == false)
    //     {
    //         //formerLayerNum = formerPossess.gameObject.layer;
    //         formerPossess.gameObject.layer = LayerMask.NameToLayer("BossThroughFormer");
    //         throughFlag = true;
    //     }
    // }

    // private void OnTriggerExit2D(Collider2D other)
    // {
    //     //どうもレイヤーを変えると出ていった判定になるらしい。
    //     if (BossData.bossData.nowState == BossData.State.highS &&
    //         formerPossess == other.transform)// &&
    //                                          // throughFlag == true)
    //     {
    //         // BossDeepData.GetBDpData.bRigid.bodyType = RigidbodyType2D.Dynamic;
    //         formerPossess.gameObject.layer = LayerMask.NameToLayer("Face");
    //         throughFlag = false;
    //     }
    // }
}