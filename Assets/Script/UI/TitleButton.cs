using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Image")]
    [SerializeField] Image fireImage;

    [Header("Audio")]
    [SerializeField] AudioClip clip;
    [SerializeField] AudioMixerGroup sfxMixer;

    private float alpha = 0;
    private Color fireColor;
    private CancellationTokenSource tokenSource_In, tokenSource_Out;
    private CancellationToken token_In, token_Out;

    private void Start()
    {
        //fireImage.enabled = false;
        tokenSource_In = new CancellationTokenSource();
        tokenSource_Out = new CancellationTokenSource();
        token_In = tokenSource_In.Token;
        token_Out = tokenSource_Out.Token;


        fireColor = fireImage.color;
        fireColor.a = 0;
        fireImage.color = fireColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ = NonSpatialSFXPlayer.PlayNonSpatialSFX(clip, sfxMixer);
        _ = FadeIn(token_In);
    }

    private async Task FadeIn(CancellationToken token)
    {
        
        float startAlpha = alpha;

        for (float i = 0; i < 0.1f; i += Time.deltaTime)
        {
            alpha = Mathf.Lerp(startAlpha, 1, i / 0.1f);
            SetAlpha(alpha);

            if (token.IsCancellationRequested)
            {
                break;
            }

            //50FPS
            await Task.Delay(20);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _ = FadeOut(token_Out);
    }

    private async Task FadeOut(CancellationToken token)
    {        
        float startAlpha = alpha;

        for(float i = 0; i < 0.1f; i += Time.deltaTime)
        {
            alpha = Mathf.Lerp(startAlpha, 0, i / 0.1f);
            SetAlpha(alpha);

            if (token.IsCancellationRequested)
            {
                break;
            }

            //50FPS
            await Task.Delay(20);
        }
    }

    private void SetAlpha(float alpha)
    {
        if (!fireImage)
        {
            Debug.LogError("No image was set to " + this.gameObject.name);
        }

        fireColor.a = alpha;
        fireImage.color = fireColor;
    }
}
