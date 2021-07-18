using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySpawnOutsideMainCamera))]
public class MainScript : MonoBehaviour
{
    //------------------Singleton--------------------
    private static MainScript _S;
    public static MainScript S
    {
        get { return _S; }
        set { _S = value; }
    }

    //------------------------Public-----------------------
    public int phase;

    [Header("DEBUG ONLY")]
    public KeyCode spawnEnemy;

    private static List<EnemyCtrl> enemies = new List<EnemyCtrl>();

    //----------------------Private-----------------------
    private EnemySpawnOutsideMainCamera spawner;

    // Start is called before the first frame update
    void Start()
    {
        spawner = GetComponent<EnemySpawnOutsideMainCamera>();

        spawner.SpawnOutsideCamera();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(spawnEnemy)) spawner.SpawnOutsideCamera();
    }

    private void PhaseShift()
    {
        phase++;
    }

    public static void AddEnemyList(EnemyCtrl newObject)
    {
        enemies.Add(newObject);
    }

    public static void RemoveFromEnemyList(EnemyCtrl removeObject)
    {
        enemies.Remove(removeObject);
    }

    public static void LetEnemiesResetTarget()
    {
        if (enemies.Count <= 0) return;

        for(int i = 0; i < enemies.Count; i++)
        {
            enemies[i].ResetTarget();
        }
    }
}
