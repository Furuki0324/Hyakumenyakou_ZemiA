//#define SAVE

using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class MainScript : MonoBehaviour
{

    //------------------------Public-----------------------
    public int defaultElementAmount;

    //----------------------Private-----------------------
    private static List<GameObject> faceObjects = new List<GameObject>();
    private enum Tags
    {
        Face_Eye,
        Face_Mouth,
        Face_Ear
    }
    [SerializeField]
    private GameObject resultParent;
    [SerializeField]
    private GameObject resultObj;
    private static TextMeshProUGUI result;

    [Header("Option")]
    [SerializeField] private InGameOption option;
    [SerializeField] private KeyCode openOptionKey;
    private bool optionIsOpened = false;

    private void Start()
    {


        //各素材の初期値を取得
        DropItemManager.ObtainItem("EyeElement", defaultElementAmount, initialize: true);
        DropItemManager.ObtainItem("EarElement", defaultElementAmount, initialize: true);
        DropItemManager.ObtainItem("MouthElement", defaultElementAmount, initialize: true);


        //開始時点で配置されているパーツを追加
        foreach (string i in Enum.GetNames(typeof(Tags)))
        {
            GameObject[] temp = GameObject.FindGameObjectsWithTag(i);
            foreach (GameObject j in temp) if (j.layer != 5) AddFaceObject(j); ;
        }

        GameObject tempInst = Instantiate(resultObj, resultParent.transform);
        result = tempInst.GetComponent<TextMeshProUGUI>();
        result.text = "";
        
        _ = GameStart();
    }

    private async Task GameStart()
    {
        Time.timeScale = 0;

        Debug.Log("Start task started.");

        List<Task> tasks = new List<Task>();
        tasks.Add(GameStartUIAnimation.FirstAnimation());
        tasks.Add(BGMPlayer.ChangeBGM(BGMInfo.Pattern.start, loop: false));

        await Task.WhenAll(tasks);

        await GameStartUIAnimation.SecondAnimation();

        BGMPlayer.ChangeBGM(BGMInfo.Pattern.defence);
        Time.timeScale = 1;
        Debug.Log("Start task finished.");

#if SAVE
        //ゲームが開始されたことを記録
        SaveData.AchievementStep(Achievement.StepType.playCount);
#endif
    }



    private void Update()
    {
        if (optionIsOpened) Time.timeScale = 0;
        if (Input.GetKeyDown(openOptionKey) && option) OptionModeToggle();
    }

    private void OptionModeToggle()
    {
        optionIsOpened = !optionIsOpened;

        if (optionIsOpened) Time.timeScale = 0;
        else Time.timeScale = 1;

        option.gameObject.SetActive(optionIsOpened);
    }

    //ここから顔パーツの情報

    /// <summary>
    /// <para>対象をリストに追加します</para>
    /// </summary>
    /// <param name="gameObject">リストに追加される対象</param>
    public static void AddFaceObject(GameObject gameObject)
    {
        faceObjects.Add(gameObject);
    }

    /// <summary>
    /// <para>対象をリストから削除します</para>
    /// </summary>
    /// <param name="gameObject">リストから削除される対象</param>
    public static void RemoveFaceObject(GameObject gameObject)
    {
        faceObjects.Remove(gameObject);
    }

    /// <summary>
    /// <para>リストに登録されている顔パーツのTransformを配列で返します。</para>
    /// </summary>
    /// <returns></returns>
    public static Transform[] GetFaceObjectTransformsInArray()
    {
        List<Transform> transforms = new List<Transform>();

        for (int i = 0; i < faceObjects.Count; i++)
        {
            transforms[i] = faceObjects[i].transform;
        }

        return transforms.ToArray();
    }

    /// <summary>
    /// <para>顔パーツリストに登録されているゲームオブジェクトのTransformをリストで返します。</para>
    /// </summary>
    /// <returns></returns>
    public static List<Transform> GetFaceObjectTransformInList()
    {
        List<Transform> transforms = new List<Transform>();

        for (int i = 0; i < faceObjects.Count; i++)
        {
            transforms.Add(faceObjects[i].transform);
        }

        return transforms;
    }

    //ここまで顔パーツの情報

    public static void GameOver()
    {
        Debug.Log("Game Over");
    }

    public static async Task GameClear()
    {
        Debug.Log("Animation task started.");
        Time.timeScale = 0;

        List<Task> tasks = new List<Task>();
        tasks.Add(BGMPlayer.ChangeBGM(BGMInfo.Pattern.clear, loop: false));
        tasks.Add(GameClearUIAnimation.ShowText());

        await Task.WhenAll(tasks);
                
        ResultData data = ResultCalculate.CalculateResultData(noseAsCenter: GameObject.FindWithTag("Face_Nose"));

        await GameClearUIAnimation.FadeOut();

        GameClearUIAnimation.CameraSwitch();
        GameClearUIAnimation.SetCullingMask();

        tasks.Clear();
        tasks.Add(GameClearUIAnimation.FadeIn());

        await Task.WhenAll(tasks);

        tasks.Clear();
        tasks.Add(BGMPlayer.ChangeBGM(BGMInfo.Pattern.result, loop: true));
        tasks.Add(GameClearUIAnimation.ScoreSliding(data));

        await Task.WhenAll(tasks);

#region
        Debug.Log("Game Clear! Your score: " + data.totalScore
             + "\nAmountScore: \n" + "Eye: " + data.eyeAmountScore
             + "\nEar: " + data.earAmountScore
             + "\nMouth: " + data.mouthAmountScore
             + "\n\nDistanceScore: \n" + "LeftEye: " + data.leftEyeDistanceScore
             + "\nRightEye: " + data.rightEyeDistanceScore
             + "\nLeftEar: " + data.leftEarDistanceScore
             + "\nRightEar: " + data.rightEarDistanceScore
             + "\nMouth: " + data.mouthDistanceScore
             + "\n\nSectionScore:\n" + "Eye: " + data.eyeSumScore
             + "\nEar: " + data.earSumScore
             + "\nMouth: " + data.mouthSumScore
             + "\n\nStars: \n" + "Eye: A" + data.eyeStar_A + " P" + data.eyeStar_P);

        result.text = "Game Clear!" + "\nYour score: " + data.totalScore
            + "\n\nAmountScore: \n" + "Eye: " + data.eyeAmountScore
            + "\nEar: " + data.earAmountScore
            + "\nMouth: " + data.mouthAmountScore
            + "\n\nDistanceScore: \n" + "LeftEye: " + data.leftEyeDistanceScore
            + "\nRightEye: " + data.rightEyeDistanceScore
            + "\nLeftEar: " + data.leftEarDistanceScore
            + "\nRightEar: " + data.rightEarDistanceScore
            + "\nMouth: " + data.mouthDistanceScore
            + "\n\nSectionScore:\n" + "Eye: " + data.eyeSumScore
            + "\nEar: " + data.earSumScore
            + "\nMouth: " + data.mouthSumScore
            + "\n\nStars: \n" + "Eye: A" + data.eyeStar_A + " P" + data.eyeStar_P;
#endregion

        result.GetComponent<TextMeshProSimpleAnimator>().Play();

        Debug.Log("Animation task finished.");
    }


}
