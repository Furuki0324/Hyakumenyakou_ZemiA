using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(RawImage))]
public class DestroyFXWhenFinishPlaying : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private RawImage image;
    private float wait;
    public bool doOnce;


    private RenderTexture texture;

    #region enum
    public enum Pattern { none,play,destroy}
    #endregion

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        image = GetComponent<RawImage>();
        CreateRenderTexture();
        //if(!videoPlayer.playOnAwake) image.enabled = false;

        wait = (float)videoPlayer.clip.length * 1.1f;

        if(doOnce) StartCoroutine(WaitForDestroy());
    }

    private void CreateRenderTexture()
    {
        texture = new RenderTexture(256, 256, 24);
        videoPlayer.targetTexture = texture;
        image.texture = texture;
    }

    public void StartTheCoroutine(Pattern pattern)
    {
        switch (pattern)
        {
            case Pattern.play:
                if (videoPlayer.isPlaying) videoPlayer.Stop();

                videoPlayer.Play();
                StartCoroutine(PlayFX());
                break;

            case Pattern.destroy:
                StartCoroutine(WaitForDestroy());
                break;
        }
    }

    IEnumerator PlayFX()
    {
        while (videoPlayer.isPlaying)
        {
            image.enabled = videoPlayer.isPlaying;
            yield return null;
        }
    }

    IEnumerator WaitForDestroy()
    {
        //再生開始を待つ
        if(doOnce) yield return new WaitForSeconds(0.1f);

        while (videoPlayer.isPlaying) yield return null;
        //yield return new WaitForSeconds(wait);
        Destroy(gameObject);
    }
}
