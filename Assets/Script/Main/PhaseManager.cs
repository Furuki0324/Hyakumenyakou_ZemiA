using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseManager : MonoBehaviour
{
    [Header("Timer")]
    [Tooltip("制限時間 (単位:秒)")]
    public float time;
    private int time_;
    private int min;
    private int second;
    
    [Header("UI")]
    public Text text;

    public static int phaseNumber = 1;

    public static List<EnemyCtrl> enemies = new List<EnemyCtrl>();

    private void Start()
    {
        text.text = phaseNumber.ToString();
    }

    private void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;
        }
        
        time_ = (int)time;
        min = time_ / 60;
        second = time_ % 60;
        text.text = "Time " + min.ToString("00") + ":" + second.ToString("00");
    }

    /// <summary>
    /// <para>フェーズを次の段階へシフトします。</para>
    /// </summary>
    public static void PhaseShift()
    {
        phaseNumber++;
        
        EnemySpawnManager.Singleton.SpawnEnemy();

        phaseNumber++;
        Debug.Log("Phase: " + phaseNumber);
    }



    /// <summary>
    /// <para>フィールド上の敵が全滅した場合にフェーズを進めます。</para>
    /// </summary>
    public static void HowManyEnemies()
    {
        if (enemies.Count <= 0) PhaseShift();
    }


    public static void AnEnemyDied(EnemyCtrl enemy)
    {
        enemies.Remove(enemy);
        HowManyEnemies();
    }
}
