using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class InGameOption : MonoBehaviour
{
    #region Public/Serialize variables

    [Header("Sound Slider")]
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider seSlider;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer mixer;

    #endregion

    #region Private variables

    /// <summary>
    /// 各スライダーの初期値
    /// </summary>
    private float baseMaster, baseBgm, baseSe;
    /// <summary>
    /// AudioMixerに実際に適用する値
    /// </summary>
    private float masterVol, bgmVol, seVol;

    #endregion

    private void Start()
    {
        InitializeVolume();
    }

    private void Update()
    {
        VolumeChange();
    }


    private void InitializeVolume()
    {
        mixer.GetFloat("Master", out float master);
        mixer.GetFloat("BGM", out float bgm);
        mixer.GetFloat("SE", out float se);

        baseMaster = master;
        baseBgm = bgm;
        baseSe = se;
    }

    private void VolumeChange()
    {
        if (masterSlider)
        {
            masterVol = baseMaster + masterSlider.value;
            mixer.SetFloat("Master", masterVol);
        }

        if (bgmSlider)
        {
            bgmVol = baseBgm + bgmSlider.value;        
            mixer.SetFloat("BGM", bgmVol);
        }

        if (seSlider)
        {
            seVol = baseSe + seSlider.value;
            mixer.SetFloat("SE", seVol);
        }

    }
}
