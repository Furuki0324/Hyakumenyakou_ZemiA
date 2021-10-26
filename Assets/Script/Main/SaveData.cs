using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    private static SaveData _S;

    public static SaveData S
    {
        get
        {
            return _S;
        }
        set
        {
            if(_S != null)
            {
                Debug.LogError("SaveData singleton - Second attempt");
            }
            _S = value;
        }
    }

    //public AchievementPopUp popUp;
    public StepRecord[] stepRecords;
    public Achievement[] achievements;

    private static Dictionary<Achievement.StepType, StepRecord> STEP_REC_DICT;
    //private static Dictionary<>

    private void Awake()
    {
        S = this;

        STEP_REC_DICT = new Dictionary<Achievement.StepType, StepRecord>();
        foreach(StepRecord sRec in stepRecords)
        {
            STEP_REC_DICT.Add(sRec.type, sRec);
        }
    }

    private void TriggerPopUp(string achievementName, string achievementDescription = "")
    {
        //popUp.PopUp(achievementName, achievementDescription);
    }


    public static void AchievementStep(Achievement.StepType stepType, int num = 1)
    {
        StepRecord sRec = STEP_REC_DICT[stepType];
        if(sRec != null)
        {
            sRec.Progress(num);

            foreach(Achievement ach in S.achievements)
            {
                if (!ach.complete)
                {
                    if (ach.CheckCompletion(stepType, sRec.num))
                    {
                        AnnounceAchievementCompletion(ach);

                        //SaveGameManager.Save();
                    }
                }
            }
        }
    }

    public static void AnnounceAchievementCompletion(Achievement ach)
    {
        string description = ach.description.Replace("#", ach.stepCount.ToString("N0"));
        //S.TriggerPopUp(ach.name, description);
        Debug.Log("Achievement unlocked.\n" +
            "Name: " + ach.name + "\n" +
            "Description: " + description);
    }

    public static Achievement[] GetAchievements()
    {
        return S.achievements;
    }

    public static StepRecord[] GetStepRecords()
    {
        return S.stepRecords;
    }

    public static void LoadFromSaveData(SaveFile saveFile)
    {
        foreach(StepRecord sRec in saveFile.stepRecords)
        {
            STEP_REC_DICT[sRec.type].num = sRec.num;
        }

        foreach(Achievement ach in saveFile.achievements)
        {

        }
    }
}


[System.Serializable]
public class Achievement
{
    public enum StepType
    {
        highScore,  //ハイスコア更新
        playCount,  //一定以上のプレイ回数-PracticeMakesPerfect
        killEnemy,  //敵を撃破-KillLeader(1プレイ中に敵を一定数以上撃破)、AllYouNeedIsKill(敵の累計撃破数が一定以上)
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
