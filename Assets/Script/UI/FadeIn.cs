using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class FadeIn : MonoBehaviour
{
    private VideoPlayer vp;
    [SerializeField] private AnimationCurve acIn;
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

        Receiver();
    }

    public void fade()
    {
        if (!init)
        {
            timeTemp = 0f;
            init = true;
        }
        
        if (timeTemp < acIn.keys[acIn.keys.Length-1].time)
        {
            vp.targetCameraAlpha = acIn.Evaluate(timeTemp);
            timeTemp += Time.deltaTime;
        }
    }

    public async Task Receiver()
    {
        float currentTime = Time.time;
        float clipLength = (float)vp.clip.length;

        await Fade_In();

        //Debug.Log("Fade in ends");

        int delayTime = (int)((currentTime + clipLength * 0.95f) - Time.time);
        if(delayTime > 0)
        {
            await Task.Delay(delayTime * 1000);
        }


        //Debug.Log("Fade out start");

        await FadeOut();
    }


    public async Task Fade_In()
    {
        vp.Play();

        vp.targetCameraAlpha = 0;

        for(float time = 0; time < acIn.keys[acIn.keys.Length - 1].time; time += Time.deltaTime)
        {
            vp.targetCameraAlpha = acIn.Evaluate(time);

            //deltaTimeをミリ秒に合わせる
            await Task.Delay((int)(Time.deltaTime * 1000));
        }

        vp.targetCameraAlpha = 1;
    }

    public async Task FadeOut()
    {
        vp.targetCameraAlpha = 1;

        for(float time = acIn.keys[acIn.keys.Length - 1].time; time > 0; time -= Time.deltaTime)
        {
            vp.targetCameraAlpha = acIn.Evaluate(time);

            await Task.Delay((int)(Time.deltaTime * 1000));
        }

        vp.targetCameraAlpha = 0;
    }
}