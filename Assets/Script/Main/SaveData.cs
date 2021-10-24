using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public StepRecord[] stepRecords;
    public Achievement[] achievements;
}


[System.Serializable]
public class Achievement
{
    public enum StepType
    {
        highScore,  //ハイスコア更新
        practiceMakesPerfect,  //一定以上のプレイ回数
        killLeader,  //1プレイの中で一定数以上の敵を撃破
        collector,  //累積回収アイテム数が一定を超える
        creator,  //累積生成パーツ数が一定を超える
    }

    public string name;
    public string description;
    public StepType stepType;
    public int stepCount; //実績解除に必要な数値（プレイ回数やアイテム回収数など）

    [SerializeField] private bool _complete = false;
    public bool complete
    {
        get { return _complete; }
        internal set
        {
            _complete = value;
        }
    }

    public bool CheckCompletion(StepType type, int num)
    {
        if(type != stepType || complete)
        {
            return false;
        }

        if(num >= stepCount)
        {
            complete = true;
            return true;
        }
        return false;
    }
}


[System.Serializable]
public class StepRecord
{
    public Achievement.StepType type;
    [Tooltip("加算していくものか(True)、それとも特定の値が入力されるものか(False)")]
    /// <summary>
    /// 加算していくものか(True)、それとも特定の値が入力されるものか(False)
    /// </summary>
    public bool cumulative = false;
    [Tooltip("The current count of this step type. Only modify for testing purpose.")]
    [SerializeField] private int _num = 0;  //テストしたいときはこの変数に入力

    public void Progress(int n)
    {
        if (cumulative)
        {
            _num += n;
        }
        else
        {
            _num = n;
        }
    }

    public int num
    {
        get { return _num; }
        internal set { _num = value; }
    }

}
