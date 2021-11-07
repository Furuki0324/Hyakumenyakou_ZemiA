using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameClearUIAnimation : MonoBehaviour
{
    [SerializeField] private Image cover;
    [SerializeField] private Text gameClearText;
    [Tooltip("アニメーション開始時にテキストを表示させる場所までの幅")]
    [SerializeField] private float diff = 100;
    [SerializeField] private float duration = 0.5f;

    private static Image _cover;
    private static Text _gameClearText;
    private static float _diff;
    private static float _duration;

    private static Color _coverColor;
    private static Color _textColor;

    private void Awake()
    {
        _cover = cover;
        _gameClearText = gameClearText;
        _diff = diff;
        _duration = duration;

        _coverColor = _cover.color;
        _coverColor.a = 0;
        _cover.color = _coverColor;

        _textColor = _gameClearText.color;
        _textColor.a = 0;
        _gameClearText.color = _textColor;
    }

    static float alpha = 0;
    public static async Task ShowText()
    {
        float additional = _diff;
        Debug.Log("GameClear animation started.");


        _cover.gameObject.SetActive(true);
        _gameClearText.gameObject.SetActive(true);

        //半透明の背景とテキストの表示
        for(float i = 0; i < _duration; i += Time.unscaledDeltaTime)
        {
            alpha = Mathf.Lerp(0, 1, i / 0.5f);
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

    public static async Task FadeOut()
    {
        //テキストの透明化
        for (float i = 0; i < _duration; i += Time.unscaledDeltaTime)
        {
            alpha = Mathf.Lerp(1, 0, i / 0.5f);
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
            alpha = Mathf.Lerp(0.5f, 1, i / 0.5f);
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

    //public static Task 

}
