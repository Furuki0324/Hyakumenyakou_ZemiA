using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    //Singleton
    public static EnemySpawnManager Singleton;


    //-----------------------Public-------------------------
    public Camera mainCam;
    public float interval;
    public int spawnSize;
    public EnemyPrefabInfo[] spawnPrefabs;
    public Transform spawnPointParent;
    [ReadOnly]public Transform[] points;
    [ReadOnly] public List<EnemySpawnPoint> spawnPoints;

    [Header("Spawn  Option")]
    public bool spawnOutsideCamera;
    
    //-----------------------Private------------------------
    
    private float cacheTime;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        //敵のスポーンポイントの管理。最終的にはこちらになる予定
        /*
        points = spawnPointParent.GetComponentsInChildren<Transform>();
        spawnPoints.Capacity = points.Length;
        Debug.Log(spawnPoints.Capacity);
        for(int i = 0; i < points.Length; i++)
        {
            spawnPoints[i].transforms = points[i];
        }
        */

        //Spawn first enemies
        for(int i = 0; i < spawnSize; i++)
        {
            SpawnOutsideCamera();
        }
    }

    private void Update()
    {
        //フェーズ管理機能が完成し次第、この時間経過でのスポーンは停止
        if (Time.time > cacheTime + interval)
        {
            for(int i = 0; i < spawnSize; i++)
            {
                //SpawnEnemy();
            }
            cacheTime = Time.time;
        }
    }

    public void SpawnEnemy()
    {
        for(int i = 0; i < spawnSize; i++)
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



    private Vector3 SetSpawnPosition()
    {
        Vector3 spawnPosition = Vector3.one;

        do
        {
            spawnPosition.x = Random.Range(-0.5f, 1.5f);
            spawnPosition.y = Random.Range(-0.5f, 1.5f);

        } while (InTheRange(spawnPosition.x, 0, 1) && InTheRange(spawnPosition.y, 0, 1));

        return mainCam.ViewportToWorldPoint(spawnPosition);
    }

    public void SpawnOutsideCamera()
    {
        //MainScript.LetEnemiesResetTarget();

        Vector3 spawnPosition = SetSpawnPosition();
        spawnPosition.z = 0;


        EnemyCtrl enemyCtrl = Instantiate(GetSpawnPrefab(), spawnPosition, Quaternion.identity);
        MainScript.AddEnemyList(enemyCtrl);
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