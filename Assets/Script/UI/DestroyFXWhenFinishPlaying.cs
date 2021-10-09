using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class DestroyFXWhenFinishPlaying : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    private float wait;

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        wait = (float)videoPlayer.clip.length * 1.1f;

        StartCoroutine(WaitForDestroy());
    }

    IEnumerator WaitForDestroy()
    {
        yield return new WaitForSeconds(wait);
        Destroy(gameObject);
    }
}
