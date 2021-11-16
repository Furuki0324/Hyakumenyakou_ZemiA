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
    [SerializeField]
    private GameObject pe;
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

    [SerializeField]
    private Transform formerPossess;
    private bool throughFlag;
    private int formerLayerNum;

    private Color bossColor;

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
        bossColor = gameObject.GetComponent<SpriteRenderer>().color;
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
                    if (takeDamage)
                    {
                        bdc.stopHavingAllCoroutine();

                        //憑依してたパーツにぶつからないようレイヤー変更
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
        //憑依エフェクトを生産
        nowPe = Instantiate(pe, BossDeepData.GetBDpData.toPossessParts.position, Quaternion.identity);

        //前に憑依してたパーツのレイヤーを念のため元に戻す
        if (formerPossess != null) formerPossess.gameObject.layer = LayerMask.NameToLayer("Face");
    }

    void unPossession()
    {
        bRenderer.enabled = true;
        //BossDeepData.GetBDpData.bRigid.bodyType = RigidbodyType2D.Dynamic;
        BossDeepData.GetBDpData.bRigid.velocity = Vector3.zero;
        formerPossess = BossDeepData.GetBDpData.toPossessParts;
        BossDeepData.GetBDpData.toPossessParts = null;

        //ランダムな方向に弾かれて出てくる
        // transform.position += new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f), UnityEngine.Random.Range(-1.0f, 1.0f), 0);
        transform.DOMoveY(5f, 5f)
            .OnStart(() => bossColor.a = 0f)
            .OnUpdate(() => bossColor.a += 1f/5f * Time.deltaTime)
            .OnComplete(() => bossColor.a = 1f);
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
        if (BossData.bossData.nowState == BossData.State.highS &&
            formerPossess != other.transform)
        {
            foreach (string i in Enum.GetNames(typeof(Tags)))
            {
                if (other.gameObject.CompareTag(i))
                {
                    BossDeepData.GetBDpData.toPossessParts = other.transform;
                }
            }
        }
    }
}