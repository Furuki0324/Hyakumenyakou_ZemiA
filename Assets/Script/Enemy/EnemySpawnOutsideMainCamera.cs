using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnOutsideMainCamera : MonoBehaviour
{
    //-----------------------Public-------------------------
    public Camera mainCam;
    public float interval;
    public int spawnSize;
    public EnemyPrefabInfo[] spawnPrefabs;
    
    //-----------------------Private------------------------
    
    private float cacheTime;


    private void Start()
    {
        for(int i = 0; i < spawnSize; i++)
        {
            SpawnOutsideCamera();
        }
    }

    private void Update()
    {
        if (Time.time > cacheTime + interval)
        {
            for(int i = 0; i < spawnSize; i++)
            {
                SpawnOutsideCamera();
            }
            cacheTime = Time.time;
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