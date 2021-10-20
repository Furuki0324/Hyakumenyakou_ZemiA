using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


[RequireComponent(typeof(VideoPlayer))]
public class VideoLoader : MonoBehaviour
{
    [SerializeField] private string relativePath;

#if UNITY_WEBGL
    private void Start()
    {
        VideoPlayer player = GetComponent<VideoPlayer>();
        player.source = VideoSource.Url;
        player.url = Application.streamingAssetsPath + "/" + relativePath;
        player.prepareCompleted += PrepareCompleted;
        player.Prepare();
    }

    private void PrepareCompleted(VideoPlayer vp)
    {
        vp.prepareCompleted -= PrepareCompleted;
        //vp.Play();
    }
#endif

}
