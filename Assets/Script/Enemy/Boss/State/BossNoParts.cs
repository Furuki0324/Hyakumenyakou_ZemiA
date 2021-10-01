using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossNoParts : MonoBehaviour, IBossStateRoot
{
    [SerializeField]
    private GameObject normalBullet;
    private Vector2 nextPosition;
    private Vector2 force;
    private GameObject tempGameObj;
    private NormalBulletCtrl tempNormBul;
    private GameObject toAttack;

    private void Start()
    {
        toAttack = GameObject.FindWithTag("Face_Nose");
    }
    public bool First { get; set; }
    public void attack() { }
    public void defend() { }
    public void move()
    {
        BossDeepData.GetBDpData.bRigid.velocity = force;
        if (First)
        {
            StartCoroutine(RandomWalk());
            StartCoroutine(BulletGenerator());
            First = false;
        }
    }

    IEnumerator RandomWalk()
    {
        if (BossData.bossData.nowState != BossData.State.noP) yield break;
        nextPosition =
            new Vector2(UnityEngine.Random.Range(transform.position.x + 10.0f, transform.position.x - 10.0f),
                        UnityEngine.Random.Range(transform.position.y + 10.0f, transform.position.y - 10.0f));
        force = (nextPosition - (Vector2)transform.position).normalized * UnityEngine.Random.Range(1.0f, BossData.bossData.noPRandWalkSpeed);
        yield return new WaitForSeconds(BossData.bossData.noPRandWalkInterval);
        StartCoroutine(RandomWalk());
    }
    IEnumerator BulletGenerator()
    {
        if (BossData.bossData.nowState != BossData.State.noP) yield break;
        tempGameObj = Instantiate(normalBullet, transform.position, transform.rotation);
        tempNormBul = tempGameObj.GetComponent<NormalBulletCtrl>();
        tempNormBul.damage = BossData.bossData.noPAttackPowOfBullet;
        tempNormBul.force = (toAttack.transform.position - transform.position).normalized * BossData.bossData.noPAttackSpeed;
        yield return new WaitForSeconds(BossData.bossData.noPAttackIntervalOfBullet);
        StartCoroutine(BulletGenerator());
    }
}