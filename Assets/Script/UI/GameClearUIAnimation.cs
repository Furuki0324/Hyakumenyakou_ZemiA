using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Cinemachine;

public class GameClearUIAnimation : MonoBehaviour
{
    /*****************************************************************
    *         このスクリプト内ではstatic変数に「_」を付けています。      *
    *         private変数の印ではありません                            *
    ******************************************************************/

    [Header("UI or GameObject")]
    [SerializeField] private CinemachineVirtualCamera vcam2;
    [SerializeField] private Image cover;
    [SerializeField] private Text gameClearText;
    [SerializeField] private SceneChangeButton restartButton;
    [SerializeField] private GameObject texts;
    [SerializeField] private GameObject UI_overlay;

    [Header("Audio")]
    [SerializeField] private AudioClip slidingSFX;
    [SerializeField] private AudioClip sumSFX;
    [SerializeField] private AudioMixerGroup sfxMixer;

    [Header("Number")]
    [Tooltip("アニメーション開始時にテキストを表示させる場所までの幅")]
    [SerializeField] private float diff = 100;
    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float slideDuration = 0.2f;

    private static CinemachineVirtualCamera _vcam2;
    private static Image _cover;
    private static Text _gameClearText;
    private static SceneChangeButton _restartButton;
    private static GameObject _texts;
    private static GameObject _UI_overlay;

    private static AudioClip _slidingSFX;
    private static AudioClip _sumSFX;
    private static AudioMixerGroup _sfxMixer;

    private static float _diff;
    private static float _duration;
    private static float _slideDuration;

    private static Color _coverColor;
    private static Color _textColor;

    private static Transform transformMyself;

    private void Awake()
    {
        _vcam2 = vcam2;
        _vcam2.gameObject.SetActive(false);
        _texts = texts;
        _texts.SetActive(false);
        _UI_overlay = UI_overlay;
        _UI_overlay.SetActive(true);

        _slidingSFX = slidingSFX;
        _sumSFX = sumSFX;
        _sfxMixer = sfxMixer;

        _cover = cover;
        _gameClearText = gameClearText;

        //リスタート用のボタンを無効化
        _restartButton = restartButton;
        _restartButton.gameObject.SetActive(false);

        _diff = diff;
        _duration = duration;
        _slideDuration = slideDuration;

        //背景の透明化
        _coverColor = _cover.color;
        _coverColor.a = 0;
        _cover.color = _coverColor;

        //ゲームクリアのテキストを透明化
        _textColor = _gameClearText.color;
        _textColor.a = 0;
        _gameClearText.color = _textColor;

        transformMyself = this.transform;
    }

    /// <summary>
    /// 背景を半透明で表示して、その上にゲームクリアのテキストを表示します。
    /// </summary>
    /// <returns></returns>
    public static async Task ShowText()
    {
        float alpha;
        Debug.Log("GameClear animation started.");


        _cover.gameObject.SetActive(true);
        _gameClearText.gameObject.SetActive(true);

        //半透明の背景とテキストの表示
        for(float i = 0; i < _duration; i += Time.unscaledDeltaTime)
        {
            alpha = Mathf.Lerp(0, 1, i / _duration);
            _coverColor.a = alpha / 2;
            _cover.color = _coverColor;

            _textColor.a = alpha;
            _gameClearText.color = _textColor;

            //Debug.Log(alpha);

            //FPSの計算
            float fps = 1 / Time.unscaledDeltaTime;
            //Debug.Log(fps);

            //1フレーム待機
            await Task.Delay((int)(1000 / fps));
        }

        //2秒待機
        await Task.Delay(2000);
    }

    public static async Task Blind()
    {
        float alpha;

        _cover.gameObject.SetActive(true);

        //半透明の背景とテキストの表示
        for (float i = 0; i < _duration; i += Time.unscaledDeltaTime)
        {
            alpha = Mathf.Lerp(0, 1, i / _duration);
            _coverColor.a = alpha;
            _cover.color = _coverColor;

            //FPSの計算
            float fps = 1 / Time.unscaledDeltaTime;
            //Debug.Log(fps);

            //1フレーム待機
            await Task.Delay((int)(1000 / fps));
        }
    }

    public static async Task Movie()
    {
        FadeIn fade = transformMyself.FindChild("MovieComponent").GetComponentInChildren<FadeIn>();

        await fade.Receiver();
    }

    /// <summary>
    /// ゲームクリアのテキストを透明化して、目隠し用の背景を不透明化します。
    /// </summary>
    /// <returns></returns>
    public static async Task FadeOut()
    {
        float alpha;

        //テキストの透明化
        for (float i = 0; i < _duration; i += Time.unscaledDeltaTime)
        {
            alpha = Mathf.Lerp(1, 0, i / _duration);
            _textColor.a = alpha;
            _gameClearText.color = _textColor;

            //FPSの計算
            float fps = 1 / Time.unscaledDeltaTime;
            //Debug.Log(fps);

            //1フレーム待機
            await Task.Delay((int)(1000 / fps));
        }

        //背景の不透明化
        for (float i = 0; i < _duration; i += Time.unscaledDeltaTime)
        {
            alpha = Mathf.Lerp(0.5f, 1, i / _duration);
            _coverColor.a = alpha;
            _cover.color = _coverColor;

            //FPSの計算
            float fps = 1 / Time.unscaledDeltaTime;
            //Debug.Log(fps);

            //1フレーム待機
            await Task.Delay((int)(1000 / fps));
        }

        Debug.Log("GameClear animation finished.");
    }

    /// <summary>
    /// 目隠しの背景の裏でカメラを切り替え、UIを表示します。
    /// </summary>
    /// <returns></returns>
    public static void CameraSwitch()
    {


        _vcam2.gameObject.SetActive(true);
        _texts.SetActive(true);
        _UI_overlay.SetActive(false);


    }

    public static void SetCullingMask()
    {
        GameObject mainCam = GameObject.FindWithTag("MainCamera");
        Camera camera = mainCam.GetComponent<Camera>();
        //レイヤー5番(UI)と10番(顔面の背景)と11番(顔パーツ)と17番(ボス憑依時の特殊レイヤー)と18番(Cinemachine)にマスクをセット
        int bit1 = 1 << 5;
        int bit2 = 1 << 10;
        int bit3 = 1 << 11;
        int bit4 = 1 << 17;
        int bit5 = 1 << 18;
        int bit = bit1 | bit2 | bit3 | bit4 | bit5;  //0b110000011000010000
        camera.cullingMask = bit << 0;

        CinemachineBrain brain = mainCam.GetComponent<CinemachineBrain>();
        brain.ManualUpdate();
    }



    /// <summary>
    /// 目隠し用の背景を透明化します。
    /// </summary>
    /// <returns></returns>
    public static async Task FadeIn()
    {
        float alpha;

        for(float i = 0; i < _duration; i += Time.unscaledDeltaTime)
        {
            alpha = Mathf.Lerp(1, 0, i / _duration);
            _coverColor.a = alpha;
            _cover.color = _coverColor;

            //FPSの計算
            float fps = 1 / Time.unscaledDeltaTime;
            //Debug.Log(fps);

            //1フレーム待機
            await Task.Delay((int)(1000 / fps));
        }
    }


    /// <summary>
    /// 目・耳・口のそれぞれの評価をスライドしながらフェードインさせます。
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static async Task ScoreSliding(ResultData data)
    {
        float fps = 1 / Time.fixedUnscaledDeltaTime;

        float alpha;
        float difference;
        Vector2 scorePos, star_A_Pos, star_P_Pos, movePos = Vector2.zero;
        Color scoreColor, star_A_Color, star_P_Color;

        //テキストのもとの座標を取得
        scorePos = data.T_eyeSumScore.transform.position;
        star_A_Pos = data.T_eyeStar_A.transform.position;
        star_P_Pos = data.T_eyeStar_P.transform.position;

        //テキストの色を取得
        scoreColor = data.T_eyeSumScore.color;
        star_A_Color = data.T_eyeStar_A.color;
        star_P_Color = data.T_eyeStar_P.color;

        //目のスライドと不透明化
        _ = NonSpatialSFXPlayer.PlayNonSpatialSFX(_slidingSFX, _sfxMixer);
        for (float i = 0; i < _slideDuration; i += Time.fixedUnscaledDeltaTime)
        {
            alpha = Mathf.Lerp(0, 1, i / _slideDuration);
            scoreColor.a = alpha;
            star_A_Color.a = alpha;
            star_P_Color.a = alpha;

            difference = Mathf.Lerp(_diff, 0, i / _slideDuration);
            movePos.x = difference;

            data.T_eyeSumScore.transform.position = scorePos + movePos;
            data.T_eyeSumScore.color = scoreColor;

            data.T_eyeStar_A.transform.position = star_A_Pos + movePos;
            data.T_eyeStar_A.color = star_A_Color;

            data.T_eyeStar_P.transform.position = star_P_Pos + movePos;
            data.T_eyeStar_P.color = star_P_Color;


            //1フレーム待機
            await Task.Delay((int)(1000 / fps));
        }

        //0.5秒待機
        await Task.Delay(500);

        //耳のテキストの座標を取得
        scorePos = data.T_earSumScore.transform.position;
        star_A_Pos = data.T_earStar_A.transform.position;
        star_P_Pos = data.T_earStar_P.transform.position;

        //耳のテキストの色を取得
        scoreColor = data.T_earSumScore.color;
        star_A_Color = data.T_earStar_A.color;
        star_P_Color = data.T_earStar_P.color;

        //耳のスライドと不透明化
        _ = NonSpatialSFXPlayer.PlayNonSpatialSFX(_slidingSFX, _sfxMixer);
        for (float i = 0; i < _slideDuration; i += Time.fixedUnscaledDeltaTime)
        {
            alpha = Mathf.Lerp(0, 1, i / _slideDuration);
            scoreColor.a = alpha;
            star_A_Color.a = alpha;
            star_P_Color.a = alpha;

            difference = Mathf.Lerp(_diff, 0, i / _slideDuration);
            movePos.x = difference;

            data.T_earSumScore.transform.position = scorePos + movePos;
            data.T_earSumScore.color = scoreColor;

            data.T_earStar_A.transform.position = star_A_Pos + movePos;
            data.T_earStar_A.color = star_A_Color;

            data.T_earStar_P.transform.position = star_P_Pos + movePos;
            data.T_earStar_P.color = star_P_Color;


            //1フレーム待機
            await Task.Delay((int)(1000 / fps));
        }

        //0.5秒待機
        await Task.Delay(500);

        //口のテキストの座標を取得
        scorePos = data.T_mouthSumScore.transform.position;
        star_A_Pos = data.T_mouthStar_A.transform.position;
        star_P_Pos = data.T_mouthStar_P.transform.position;

        //口のテキストの色を取得
        scoreColor = data.T_mouthSumScore.color;
        star_A_Color = data.T_mouthStar_A.color;
        star_P_Color = data.T_mouthStar_P.color;


        //口のスライドと不透明化
        _ = NonSpatialSFXPlayer.PlayNonSpatialSFX(_slidingSFX, _sfxMixer);
        for (float i = 0; i < _slideDuration; i += Time.fixedUnscaledDeltaTime)
        {
            alpha = Mathf.Lerp(0, 1, i / _slideDuration);
            scoreColor.a = alpha;
            star_A_Color.a = alpha;
            star_P_Color.a = alpha;

            difference = Mathf.Lerp(_diff, 0, i / _slideDuration);
            movePos.x = difference;

            data.T_mouthSumScore.transform.position = scorePos + movePos;
            data.T_mouthSumScore.color = scoreColor;

            data.T_mouthStar_A.transform.position = star_A_Pos + movePos;
            data.T_mouthStar_A.color = star_A_Color;

            data.T_mouthStar_P.transform.position = star_P_Pos + movePos;
            data.T_mouthStar_P.color = star_P_Color;


            //1フレーム待機
            await Task.Delay((int)(1000 / fps));
        }

        //1秒待機
        await Task.Delay(1000);

        //合計点のテキストの座標を取得
        scorePos = data.T_totalScore.transform.position;

        //合計点のテキストの色を取得
        scoreColor = data.T_totalScore.color;

        //合計点のテキストのスライドと不透明化
        _ = NonSpatialSFXPlayer.PlayNonSpatialSFX(_sumSFX,_sfxMixer);
        for (float i = 0; i < _slideDuration; i += Time.fixedUnscaledDeltaTime)
        {
            alpha = Mathf.Lerp(0, 1, i / _slideDuration);
            scoreColor.a = alpha;
            data.T_totalScore.color = scoreColor;

            difference = Mathf.Lerp(_diff, 0, i / _slideDuration);
            movePos.x = difference;
            data.T_totalScore.transform.position = scorePos + movePos;


            //1フレーム待機
            await Task.Delay((int)(1000 / fps));
        }

        //1秒待機
        await Task.Delay(1000);

        //リスタートボタンを有効化
        _restartButton.gameObject.SetActive(true);
    }


}
