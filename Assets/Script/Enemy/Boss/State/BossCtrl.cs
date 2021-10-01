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

    private float time;
    private Renderer bRenderer;
    private enum tags
    {
        Face_Eye,
        Face_Mouth,
        Face_Ear
    }

    private bool takeDamage;

    #endregion

    void Start()
    {
        bp = gameObject.GetComponent<BossPossesion>();
        bdc = gameObject.GetComponent<BossDamChance>();
        bhs = gameObject.GetComponent<BossHighSpeed>();
        bnp = gameObject.GetComponent<BossNoParts>();
        time = Camera.main.GetComponent<PhaseManager>().time;
        BossData.bossData.nowState = BossData.State.highS;
        bRenderer = GetComponent<Renderer>();
        BossDeepData.GetBDpData.bRigid = GetComponent<Rigidbody2D>();
        takeDamage = false;
    }

    void Update()
    {
        if (time <= 0)
        {
            switch (BossData.bossData.nowState)
            {
                case BossData.State.pos:
                    bp.attack();
                    if (takeDamage)
                    {
                        takeDamage = false;
                        unPossession();
                        bdc.First = true;
                        BossData.bossData.nowState = BossData.State.damC;
                    }
                    if (BossDeepData.GetBDpData.toPossessParts == null)
                    {
                        unPossession();
                        bhs.First = true;
                        BossData.bossData.nowState = BossData.State.highS;
                    }
                    break;

                case BossData.State.damC:
                    bdc.move();
                    if (takeDamage)
                    {
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
                        possession();
                        bp.First = true;
                        BossData.bossData.nowState = BossData.State.pos;
                    }
                    //もしぶつかる候補が無かったらパーツ無し状態へ
                    if (BossDeepData.GetBDpData.transforms.Count <= 0)
                    {
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
    }

    void unPossession()
    {
        bRenderer.enabled = true;
        BossDeepData.GetBDpData.bRigid.bodyType = RigidbodyType2D.Dynamic;
        BossDeepData.GetBDpData.bRigid.velocity = Vector3.zero;
        //BossDeepData.GetBDpData.toPossessParts = null;
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
            foreach (string i in Enum.GetNames(typeof(tags)))
            {
                if (other.gameObject.CompareTag(i))
                {
                    BossDeepData.GetBDpData.toPossessParts = other.transform;
                }
            }
        }
    }
}