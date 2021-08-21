using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseManager : MonoBehaviour
{
    public static int phaseNumber = 1;

    public static List<EnemyCtrl> enemies = new List<EnemyCtrl>();

    /// <summary>
    /// <para>フェーズを次の段階へシフトします。</para>
    /// </summary>
    public static void PhaseShift()
    {
        phaseNumber++;
        Debug.Log("Phase: " + phaseNumber);
    }

    public static void HowManyEnemies()
    {
        if (enemies.Count <= 0) PhaseShift();
    }
}
