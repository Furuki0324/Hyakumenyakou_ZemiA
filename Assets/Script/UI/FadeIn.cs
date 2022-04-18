using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(VideoPlayer))]
[RequireComponent(typeof(PostProcessVolume))]
public class FadeIn : MonoBehaviour
{
    private VideoPlayer vp;
    [SerializeField] private AnimationCurve acIn;
    private PostProcessVolume ppv;
    private float timeTemp = 0f;
    public bool init = false;

    public float VideoAlpha
    {
        get => vp.targetCameraAlpha;
        set => vp.targetCameraAlpha = value;
    }

    void Start()
    {
        vp = GetComponent<VideoPlayer>();
        ppv = GetComponent<PostProcessVolume>();
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Mouse3))
    //    {
    //        _ = Receiver();
    //    }
    //}

    public async Task Receiver()
    {
        transform.parent.gameObject.SetActive(true);

        float currentTime = Time.realtimeSinceStartup;
        float clipLength = (float)vp.clip.length;
        if (vp.playbackSpeed > 0)
        {
            clipLength /= vp.playbackSpeed;
        }
        List<Task> tasks = new List<Task>();
        tasks.Add(Fade_In());

        await Task.WhenAll(tasks);

        int delayTime = (int)((currentTime + clipLength * 0.95f) - Time.realtimeSinceStartup);
        if(delayTime > 0)
        {
            await Task.Delay(delayTime * 1000);
        }


        //Debug.Log("Fade out start");
        tasks.Clear();
        tasks.Add(FadeOut());

        await Task.WhenAll(tasks);

        transform.parent.gameObject.SetActive(false);
    }


    public async Task Fade_In()
    {
        vp.Play();

        vp.targetCameraAlpha = 0;
        ppv.weight = 0;

        for(float time = 0; time < acIn.keys[acIn.keys.Length - 1].time; time += Time.fixedUnscaledDeltaTime)
        {
            vp.targetCameraAlpha = acIn.Evaluate(time);
            ppv.weight = time;

            //deltaTimeをミリ秒に合わせる
            await Task.Delay((int)(Time.fixedUnscaledDeltaTime * 1000));
        }

        vp.targetCameraAlpha = acIn.keys[acIn.keys.Length - 1].value;
        ppv.weight = 1;
    }

    public async Task FadeOut()
    {
        vp.targetCameraAlpha = acIn.keys[acIn.keys.Length - 1].value;
        ppv.weight = 1;

        for(float time = acIn.keys[acIn.keys.Length - 1].time; time > 0; time -= Time.fixedUnscaledDeltaTime)
        {
            vp.targetCameraAlpha = acIn.Evaluate(time);
            ppv.weight = time;

            await Task.Delay((int)(Time.fixedUnscaledDeltaTime * 1000));
        }

        vp.targetCameraAlpha = 0;
        ppv.weight = 0;
    }
}