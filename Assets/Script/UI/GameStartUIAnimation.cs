using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameStartUIAnimation : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image cover;
    [SerializeField] private Text T_start;

    [Header("Number")]
    [SerializeField] private float duration;

    private static Image _cover;
    private static Color _coverColor;

    private static Text _T_start;
    private static Color _T_start_Color;

    private static float _duration;
    private static float _atLeastDelayTime;

    private void Awake()
    {
        _cover = cover;
        _coverColor = _cover.color;
        //透明化
        _coverColor.a = 0;
        _cover.color = _coverColor;

        _T_start = T_start;
        _T_start_Color = _T_start.color;
        _T_start_Color.a = 0;
        _T_start.color = _T_start_Color;

        _duration = duration;
        _atLeastDelayTime = _duration * 3;
    }

    public static async Task FirstAnimation()
    {
        //FPS
        float fps = 1 / Time.fixedUnscaledDeltaTime;

        float coverAlpha = 0;
        float textAlpha = 0;
        for (float i = 0; i < _duration; i += Time.fixedUnscaledDeltaTime)
        {
            coverAlpha = Mathf.Lerp(0, 0.5f, i / _duration);
            textAlpha = Mathf.Lerp(0, 1, i / _duration);

            _coverColor.a = coverAlpha;
            _cover.color = _coverColor;

            _T_start_Color.a = textAlpha;
            _T_start.color = _T_start_Color;

            //wait 1 frame
            await Task.Delay((int)(1000 / fps));
        }

        //色の統一
        _coverColor.a = 0.5f;
        _cover.color = _coverColor;
        _T_start_Color.a = 1;
        _T_start.color = _T_start_Color;
    }

    public static async Task SecondAnimation()
    {
        //FPS
        float fps = 1 / Time.fixedUnscaledDeltaTime;

        float coverAlpha = 0.5f;
        float textAlpha = 1;
        for(float i = 0; i < _duration; i += Time.fixedUnscaledDeltaTime)
        {
            coverAlpha = Mathf.Lerp(0.5f, 0, i / _duration);
            textAlpha = Mathf.Lerp(1, 0, i / _duration);

            _coverColor.a = coverAlpha;
            _cover.color = _coverColor;

            _T_start_Color.a = textAlpha;
            _T_start.color = _T_start_Color;

            //wait 1 frame
            await Task.Delay((int)(1000 / fps));
        }

        //オブジェクトの無効化
        _cover.gameObject.SetActive(false);
        _T_start.gameObject.SetActive(false);
    }

}
