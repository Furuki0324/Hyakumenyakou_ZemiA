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
}
