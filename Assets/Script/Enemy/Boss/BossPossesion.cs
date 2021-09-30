using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPossesion : MonoBehaviour, IBossStateRoot
{
    private Transform coreParts;
    [SerializeField]
    private GameObject eyeTearBullet;
    [SerializeField]
    private GameObject mouseVoiceBullet;
    [SerializeField]
    private GameObject earNormalBullet;
    private GameObject temp;

    private float theta;
    private const int earBulletWay = 4;
    public bool First {get; set;}

    private void Start()
    {
        coreParts = GameObject.FindGameObjectWithTag("Face_Nose").transform;
        First = true;
    }

    public void attack()
    {
        attackWay(BossDeepData.GetBDpData.toPossessParts.gameObject);
    }
    public void defend() { }
    public void move() { }

    void attackWay(GameObject possParts)
    {
        if (First)
        {
            if (possParts.CompareTag("Face_Eye"))
            {
                StartCoroutine(TearGenerator());
            }
            if (possParts.CompareTag("Face_Mouth"))
            {
                StartCoroutine(VoiceGenerator());
            }
            if (possParts.CompareTag("Face_Ear"))
            {
                StartCoroutine(CrossBulletGenerator());
            }
        }
        First = false;
    }
    void effect()
    {
        if (BossDeepData.GetBDpData.toPossessParts.gameObject.CompareTag("Face_Eye"))
        {

        }
        if (BossDeepData.GetBDpData.toPossessParts.gameObject.CompareTag("Face_Mouth"))
        {

        }
        if (BossDeepData.GetBDpData.toPossessParts.gameObject.CompareTag("Face_Ear"))
        {

        }
    }

    IEnumerator TearGenerator()
    {
        if (BossData.bossData.nowState != BossData.State.pos) yield break;
        Instantiate(eyeTearBullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(BossData.bossData.eyeAttackInterval);
        StartCoroutine(TearGenerator());
    }
    IEnumerator VoiceGenerator()
    {
        if (BossData.bossData.nowState != BossData.State.pos) yield break;
        Instantiate(mouseVoiceBullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(BossData.bossData.mouthAttackInterval);
        StartCoroutine(VoiceGenerator());
    }

    IEnumerator CrossBulletGenerator()
    {
        if (BossData.bossData.nowState != BossData.State.pos) yield break;
        temp = Instantiate(earNormalBullet, transform.position, transform.rotation);
        temp.GetComponent<EarBulletCtrl>().force =
            (coreParts.position - transform.position).normalized * BossData.bossData.earAttackSpeed;
        for (int i = 0; i < earBulletWay; i++)
        {
            temp = Instantiate(earNormalBullet, transform.position, transform.rotation);
            theta = 360.0f / earBulletWay * (i + 1) * Mathf.Deg2Rad;
            temp.GetComponent<EarBulletCtrl>().force =
                new Vector2(Mathf.Cos(theta), Mathf.Sin(theta));
        }
        yield return new WaitForSeconds(BossData.bossData.earAttackInterval);
        StartCoroutine(CrossBulletGenerator());
    }
}
