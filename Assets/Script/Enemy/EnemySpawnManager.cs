using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    //-----------------------Public-------------------------
    public Camera mainCam;
    [Header("Prefab")]
    public EnemyPrefabInfo[] spawnPrefabs;
    private static List<EnemyPrefabInfo> _spawnPrefabs = new List<EnemyPrefabInfo>();
    public BossCtrl bossPrefab;
    private static BossCtrl _bossPrefab;

    [Header("Spawn  Option")]
    public int spawnSize;
    private static int _spawnSize;

    [Header("Spawn Point")]
    [SerializeField] private float difference;
    private static float _difference;
    [SerializeField] private Transform[] spawnPoints;
    private static List<Transform> _spawnPoints = new List<Transform>();


    private void Awake()
    {
        //Get static variables
        _spawnPrefabs.Clear();
        foreach (EnemyPrefabInfo info in spawnPrefabs)
        {
            _spawnPrefabs.Add(info);
        }
        _bossPrefab = bossPrefab;
        _spawnSize = spawnSize;
        _difference = difference;

        _spawnPoints.Clear();
        foreach(Transform transform in spawnPoints)
        {
            _spawnPoints.Add(transform);
        }
        
    }

    private void Start()
    {
        //Spawn first enemies
        SpawnEnemy();
    }

    public static void SpawnEnemy()
    {
        if(_spawnPrefabs.Count <= 0)
        {
            Debug.LogError("No enemy prefab is set to EnemySpawner.");
            return;
        }

        for(int i = 0; i < _spawnSize + PhaseManager.phaseNumber / 5; i++)
        {
            Vector2 spawnPosition = GetSpawnPoint();
            AddDifference<Vector2>(ref spawnPosition);

            EnemyCtrl enemy = GetSpawnPrefab();

            Instantiate(enemy, spawnPosition, Quaternion.identity);
        }
    }

    public static void SpawnBoss()
    {
        if(_bossPrefab == null)
        {
            Debug.LogError("No boss prefab is set to EnemySpawner.");
            return;
        }

        Vector2 spawnPosition = GetSpawnPoint();

        Instantiate(_bossPrefab, spawnPosition, Quaternion.identity);
    }


    private static Vector2 GetSpawnPoint()
    {
        int index = Random.Range(0, _spawnPoints.Count);
        Vector2 position = _spawnPoints[index].position;

        return position;
    }
    

    private static EnemyCtrl GetSpawnPrefab()
    {
        int index = Random.Range(0, _spawnPrefabs.Count);
        return _spawnPrefabs[index].prefab;
    }

    /// <summary>
    /// <para>敵が全く同一の地点にスポーンしないように差異を加えます。</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="pos"></param>
    private static void AddDifference<T>(ref Vector2 pos)
    {
        float xDif = Random.Range(-_difference, _difference);
        float yDif = Random.Range(-_difference, _difference);

        pos.x += xDif;
        pos.y += yDif;
    }


    #region Never used methods
    /*
    private Vector2 SetSpawnPosition()
    {
        Vector2 spawnPosition = Vector2.zero;

        AddDifference<Vector2>(ref spawnPosition);

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
    */
    #endregion
}

[System.Serializable]
public class EnemyPrefabInfo
{
    public string name;
    public EnemyCtrl prefab;
}