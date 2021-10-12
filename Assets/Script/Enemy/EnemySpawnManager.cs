using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    //Singleton
    public static EnemySpawnManager Singleton;


    //-----------------------Public-------------------------
    public Camera mainCam;
    [Header("Prefab")]
    public EnemyPrefabInfo[] spawnPrefabs;
    public GameObject bossPrefab;

    [Header("Spawn  Option")]
    public float interval;
    public int spawnSize;
    public bool spawnOutsideCamera;

    [Header("Ohter")]
    public Transform spawnPointParent;
    [ReadOnly]public Transform[] points;
    [ReadOnly] public List<EnemySpawnPoint> spawnPoints;
    //-----------------------Private------------------------
    
    private float cacheTime;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        //Spawn first enemies
        for(int i = 0; i < spawnSize; i++)
        {
            SpawnOutsideCamera();
        }
    }

    public void SpawnEnemy()
    {
        for(int i = 0; i < spawnSize + PhaseManager.phaseNumber / 5; i++)
        {
            SpawnOutsideCamera();

            //最終的には数か所の指定されたポジションからスポーンするようにしますが、今はカメラの外から
            //ランダムでポジションが決まるようになっています。
            /*
            if (spawnOutsideCamera) SpawnOutsideCamera();
            else
            */
        }
        
    }

    public void SpawnBoss()
    {
        Vector3 spawnPosition = SetSpawnPosition();
        spawnPosition.z = 0;

        Instantiate(bossPrefab, spawnPosition, Quaternion.identity);
    }



    private Vector3 SetSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.zero;

        do
        {
            spawnPosition.x = Random.Range(-0.2f, 1.2f);
            spawnPosition.y = Random.Range(-0.2f, 1.2f);

        } while (InTheRange(spawnPosition.x, 0, 1) && InTheRange(spawnPosition.y, 0, 1));

        return mainCam.ViewportToWorldPoint(spawnPosition);
    }

    public void SpawnOutsideCamera()
    {
        //MainScript.LetEnemiesResetTarget();

        Vector3 spawnPosition = SetSpawnPosition();
        spawnPosition.z = 0;


        EnemyCtrl enemyCtrl = Instantiate(GetSpawnPrefab(), spawnPosition, Quaternion.identity);
        //MainScript.AddEnemyList(enemyCtrl);
    }

    private EnemyCtrl GetSpawnPrefab()
    {
        int index = Random.Range(0, spawnPrefabs.Length);
        return spawnPrefabs[index].prefab;
    }

    /// <summary>
    /// <para>対象(target)の数値が任意の範囲にある場合にtrueを返します。</para>
    /// </summary>
    /// <param name="target">チェックされる対象</param>
    /// <param name="min">範囲の最低値</param>
    /// <param name="max">範囲の最大値</param>
    /// <returns></returns>
    private bool InTheRange(float target, float min, float max)
    {
        if (target > min && target < max) return true;
        else return false;
    }
}

[System.Serializable]
public class EnemyPrefabInfo
{
    public string name;
    public EnemyCtrl prefab;
}

[System.Serializable]
public class EnemySpawnPoint
{
    public string name;
    public Transform transforms;
    public bool isActive;
}