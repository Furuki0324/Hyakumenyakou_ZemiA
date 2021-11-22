﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseManager : MonoBehaviour
{
    [Header("Timer")]
    [Tooltip("制限時間 (単位:秒)")]
    public float time;
    private static int _time;
    private int min;
    private int second;
    
    [Header("UI")]
    public Text text;

    [Header("Text")]
    [Tooltip("カウントダウンが0になった以降のテキスト")]
    [SerializeField] private string afterCountdownText;
    [Tooltip("カウントダウンが0になった以降のテキストサイズ")]
    [SerializeField] private int afterCountdownSize;
    [Tooltip("カウントダウンが0になった以降のテキストの色")]
    [SerializeField] private Color afterCountdownColor;

    public static int phaseNumber = 1;

    private static List<EnemyBaseScript> enemyList = new List<EnemyBaseScript>();
    /// <summary>
    /// <para>ボスがスポーンしているか</para>
    /// </summary>
    private static bool boss;

    private void Start()
    {
        boss = false;
        enemyList.Clear();
        phaseNumber = 1;
    }

    private void Update()
    {
        if(time > 0)
        {
            time -= Time.deltaTime;

            _time = (int)time;
            min = _time / 60;
            second = _time % 60;
            text.text = $"Time {min:00}:{second:00}";
        }
        else
        {
            text.text = afterCountdownText;
            text.color = afterCountdownColor;
            text.fontSize = afterCountdownSize;
        }
        
        
    }

    /// <summary>
    /// <para>フェーズを次の段階へシフトします。</para>
    /// </summary>
    public static void PhaseShift()
    {
        if (boss)
        {
            Debug.LogWarning("The boss had been spawned.\nYou need to modify script if you wanna spawn the boss twice or more.");
            return;
        }

        phaseNumber++;
        
        if(_time > 0)
        {
            EnemySpawnManager.SpawnEnemy();
        }
        else
        {
            EnemySpawnManager.SpawnBoss();
            _ = BGMPlayer.ChangeBGM(BGMInfo.Pattern.boss);
            boss = true;
        }
        

        //Debug.Log("Phase: " + phaseNumber);
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
        //Debug.Log("Count" + enemyList.Count);
    }

    public static void RemoveFromEnemyList(EnemyBaseScript removeEnemy)
    {
        enemyList.Remove(removeEnemy);
        HowManyEnemies();
    }
}
