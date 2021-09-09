using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseManager : MonoBehaviour
{
    [Header("Timer")]
    [Tooltip("制限時間 (単位:秒)")]
    public float time;
    private static int time_;
    private int min;
    private int second;
    
    [Header("UI")]
    public Text text;

    public static int phaseNumber = 1;

    private static List<EnemyBaseScript> enemyList = new List<EnemyBaseScript>();

    private void Start()
    {
        text.text = phaseNumber.ToString();
    }

    private void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;

            time_ = (int)time;
            min = time_ / 60;
            second = time_ % 60;
            text.text = "Time " + min.ToString("00") + ":" + second.ToString("00");
        }
        else
        {
            text.text = "The boss incoming!";
            text.color = Color.red;
        }
        
        
    }

    /// <summary>
    /// <para>フェーズを次の段階へシフトします。</para>
    /// </summary>
    public static void PhaseShift()
    {
        phaseNumber++;
        
        if(time_ > 0)
        {
            EnemySpawnManager.Singleton.SpawnEnemy();
        }
        else
        {
            EnemySpawnManager.Singleton.SpawnBoss();
        }
        

        Debug.Log("Phase: " + phaseNumber);
    }



    /// <summary>
    /// <para>フィールド上の敵が全滅した場合にフェーズを進めます。</para>
    /// </summary>
    public static void HowManyEnemies()
    {
        if (enemyList.Count <= 0) PhaseShift();
    }

    public static void AddEnemyList(EnemyBaseScript newEnemy)
    {
        enemyList.Add(newEnemy);
        Debug.Log("Count" + enemyList.Count);
    }

    public static void RemoveFromEnemyList(EnemyBaseScript removeEnemy)
    {
        enemyList.Remove(removeEnemy);
        HowManyEnemies();
    }
}
