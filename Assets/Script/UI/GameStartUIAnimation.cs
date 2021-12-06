using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class GameStartUIAnimation : MonoBehaviour
{
    private static Transform transformMyself;
    private static Image cover;

    private void Awake()
    {
        transformMyself = this.transform;
        cover = transform.FindChild("Cover").GetComponent<Image>();

        Color color = new Color(0, 0, 0, 1);
        cover.color = color;
    }

    public static async Task CoverFadeOut()
    {
        Color color = new Color(0, 0, 0, 1);
        float alpha = 1;

        float fps = 1 / Time.fixedUnscaledDeltaTime;

        for(float i = 0; i < 0.5f; i += Time.fixedUnscaledDeltaTime)
        {
            alpha = Mathf.Lerp(1, 0, i / 0.5f);

            color.a = alpha;
            cover.color = color;

            await Task.Delay((int)(1000 / fps));
        }

        color.a = 0;
        cover.color = color;
        cover.gameObject.SetActive(false);
    }

    public static async Task Movie()
    {
        FadeIn fade = transformMyself.FindChild("MovieComponent").GetComponentInChildren<FadeIn>();

        await fade.Receiver();
    }

    #region Never Used
    //public static async Task FirstAnimation()
    //{
    //    //FPS
    //    float fps = 1 / Time.fixedUnscaledDeltaTime;

    //    float coverAlpha = 0;
    //    float textAlpha = 0;
    //    for (float i = 0; i < _duration; i += Time.fixedUnscaledDeltaTime)
    //    {
    //        coverAlpha = Mathf.Lerp(1, 0.5f, i / _duration);
    //        textAlpha = Mathf.Lerp(0, 1, i / _duration);

    //        _coverColor.a = coverAlpha;
    //        _cover.color = _coverColor;

    //        _T_start_Color.a = textAlpha;
    //        _T_start.color = _T_start_Color;

    //        //wait 1 frame
    //        await Task.Delay((int)(1000 / fps));
    //    }

    //    //色の統一
    //    _coverColor.a = 0.5f;
    //    _cover.color = _coverColor;
    //    _T_start_Color.a = 1;
    //    _T_start.color = _T_start_Color;
    //}

    //public static async Task SecondAnimation()
    //{
    //    //FPS
    //    float fps = 1 / Time.fixedUnscaledDeltaTime;

    //    float coverAlpha = 0.5f;
    //    float textAlpha = 1;
    //    for(float i = 0; i < _duration; i += Time.fixedUnscaledDeltaTime)
    //    {
    //        coverAlpha = Mathf.Lerp(0.5f, 0, i / _duration);
    //        textAlpha = Mathf.Lerp(1, 0, i / _duration);

    //        _coverColor.a = coverAlpha;
    //        _cover.color = _coverColor;

    //        _T_start_Color.a = textAlpha;
    //        _T_start.color = _T_start_Color;

    //        //wait 1 frame
    //        await Task.Delay((int)(1000 / fps));
    //    }

    //    //オブジェクトの無効化
    //    _cover.gameObject.SetActive(false);
    //    _T_start.gameObject.SetActive(false);
    //}
    #endregion
}
