using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPossesion : MonoBehaviour, IBossStateRoot
{
    private Transform coreParts;
    [SerializeField]
    private GameObject eyeTearBullet;
    [SerializeField]
    private GameObject mouthVoiceBullet;
    [SerializeField]
    private GameObject earBullet;
    //CrossBulletコルーチン内で使う
    private GameObject temp;
    private FacePartsBaseScript faceScript;

    private float theta;
    private const int earBulletWay = 4;
    public bool First { get; set; }

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
            faceScript = BossDeepData.GetBDpData.toPossessParts.GetComponent<FacePartsBaseScript>();
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

    public void stopAllCoroutine(){
        StopCoroutine(PossPartsDamage());
        StopCoroutine(TearGenerator());
        StopCoroutine(VoiceGenerator());
        StopCoroutine(CrossBulletGenerator());
    }

    IEnumerator PossPartsDamage()
    {
        if (BossData.bossData.nowState != BossData.State.pos) yield break;
        faceScript.TakeDamage(BossData.bossData.possAttackPowToParts);
        yield return new WaitForSeconds(BossData.bossData.possAttackToPartsInterval);
        StartCoroutine(PossPartsDamage());
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
        temp = Instantiate(mouthVoiceBullet, Vector3.zero, Quaternion.identity);
        temp.GetComponent<VoiceCtrl>().force = 
            (coreParts.position - transform.position).normalized * BossData.bossData.mouthAttackSpeed;
        
        yield return new WaitForSeconds(BossData.bossData.mouthAttackInterval);
        StartCoroutine(VoiceGenerator());
    }

    IEnumerator CrossBulletGenerator()
    {
        if (BossData.bossData.nowState != BossData.State.pos) yield break;
        temp = Instantiate(earBullet, transform.position, transform.rotation);
        temp.GetComponent<EarBulletCtrl>().force =
            (coreParts.position - transform.position).normalized * BossData.bossData.earAttackSpeed;
        for (int i = 0; i < earBulletWay; i++)
        {
            temp = Instantiate(earBullet, transform.position, transform.rotation);
            theta = 360.0f / earBulletWay * (i + 1) * Mathf.Deg2Rad;
            temp.GetComponent<EarBulletCtrl>().force =
                new Vector2(Mathf.Cos(theta), Mathf.Sin(theta)) * BossData.bossData.earAttackSpeed;
        }
        yield return new WaitForSeconds(BossData.bossData.earAttackInterval);
        StartCoroutine(CrossBulletGenerator());
    }
}
