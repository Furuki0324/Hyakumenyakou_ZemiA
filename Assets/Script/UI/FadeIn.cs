using System;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class FadeIn : MonoBehaviour
{
    private VideoPlayer vp;
    [SerializeField] private AnimationCurve acIn;
    private float timeTemp = 0f;
    private bool init = false;

    public float VideoAlpha
    {
        get => vp.targetCameraAlpha;
        set => vp.targetCameraAlpha = value;
    }

    void Start()
    {
        vp = GetComponent<VideoPlayer>();
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

    public async Task Fade()
    {

    }

    bool doOnce;
    private void Update()
    {
        if (!doOnce)
        {
            vp.Play();
            doOnce = true;
        }
        fade();
    }
}