using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Cinemachine;

[RequireComponent(typeof(ResultCalculate))]
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

    [Header("GameClear/GameOver Animation")]
    [SerializeField] private Animator gameClearAnimator;
    [SerializeField] private Animator gameOverAnimator;

    [Header("Result UI-Total")]
    [SerializeField] Text total;

    [Header("Result UI-Eye")]
    [SerializeField] Text eyeSum;
    [SerializeField] Text eyeAmountStar;
    [SerializeField] Text eyePositionStar;

    [Header("Result UI-Ear")]
    [SerializeField] Text earSum;
    [SerializeField] Text earAmountStar;
    [SerializeField] Text earPositionStar;

    [Header("Result UI-Mouth")]
    [SerializeField] Text mouthSum;
    [SerializeField] Text mouthAmountStar;
    [SerializeField] Text mouthPositionStar;

    [Header("Option")]
    [SerializeField] private InGameOption option;
    [SerializeField] private KeyCode openOptionKey;
    [SerializeField] AudioMixer mixer;
    private static AudioMixer _mixer;
    private bool optionIsOpened = false;
    private static float defaultVol = 0;

    private void Start()
    {
        _mixer = mixer;
        if(defaultVol == 0)
        {
            _mixer.GetFloat("BGM", out float value);
            defaultVol = value;
        }
        else
        {
            _mixer.SetFloat("BGM", defaultVol);
        }

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
        
        _ = GameStart();
    }

    private async Task GameStart()
    {
        EyeScript.blindInitialize = true;
        Time.timeScale = 0;

        Debug.Log("Start task started.");

        await GameStartUIAnimation.CoverFadeOut();

        List<Task> tasks = new List<Task>();
        //tasks.Add(GameStartUIAnimation.FirstAnimation());
        tasks.Add(GameStartUIAnimation.Movie());
        tasks.Add(BGMPlayer.ChangeBGM(BGMInfo.Pattern.start, loop: false));

        await Task.WhenAll(tasks);

        //await GameStartUIAnimation.SecondAnimation();

        BGMPlayer.ChangeBGM(BGMInfo.Pattern.defence);
        Time.timeScale = 1;
        Debug.Log("Start task finished.");
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

    public void StartGameOverAnimation()
    {
        Time.timeScale = 0;
        BGMPlayer.ChangeBGM(BGMInfo.Pattern.none);
        gameOverAnimator.gameObject.SetActive(true);
        gameOverAnimator.Play("Start");
    }

    public async Task StartGameClearAnimation()
    {
        FadeIn fade = gameClearAnimator.GetComponentInChildren<FadeIn>();
        GameObject nose = GameObject.FindWithTag("Face_Nose");
        ResultCalculate calculater = GetComponent<ResultCalculate>();

        //耳のギミックによって下げられていたBGMのボリュームを戻す
        mixer.SetFloat("BGM", defaultVol);

        Time.timeScale = 0;
        BGMPlayer.ChangeBGM(BGMInfo.Pattern.clear, loop: false);
        if(fade)
        {
            //設定された動画を再生する
            await fade.Receiver();
        }

        //動画再生後の違和感の少ないタイミングに得点計算
        ResultData data = calculater.CalculateResultData(nose);
        //リザルトを対応したUIに入力
        eyeSum.text = data.eyeSumScore.ToString();
        eyeAmountStar.text = data.eyeStar_A;
        eyePositionStar.text = data.eyeStar_P;

        earSum.text = data.earSumScore.ToString();
        earAmountStar.text = data.earStar_A;
        earPositionStar.text = data.earStar_P;

        mouthSum.text = data.mouthSumScore.ToString();
        mouthAmountStar.text = data.mouthStar_A;
        mouthPositionStar.text = data.mouthStar_P;

        total.text = data.totalScore.ToString() + "/300";

        //アニメーションを開始
        gameClearAnimator.gameObject.SetActive(true);
        gameClearAnimator.Play("Base Layer.ShowResult");
    }
}