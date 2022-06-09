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
    /// 各AudioMixerGroupの初期値
    /// </summary>
    private float baseMaster, baseBgm, baseSe;
    /// <summary>
    /// AudioMixerに実際に適用する値
    /// </summary>
    private float masterVol, bgmVol, seVol;

    private bool isMasterMute = false, isBgmMute = false, isSeMute = false;
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
        masterSlider.value = masterSlider.maxValue / 2;
        bgmSlider.value = bgmSlider.maxValue / 2;
        seSlider.value = seSlider.maxValue / 2;

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
            masterVol = isMasterMute? -80.0f : baseMaster + masterSlider.value - masterSlider.maxValue / 2;
            mixer.SetFloat("Master", masterVol);
        }

        if (bgmSlider)
        {
            bgmVol = isBgmMute? -80.0f : baseBgm + bgmSlider.value - bgmSlider.maxValue / 2;
            mixer.SetFloat("BGM", bgmVol);
        }

        if (seSlider)
        {
            seVol = isSeMute? -80.0f : baseSe + seSlider.value - seSlider.maxValue / 2;
            mixer.SetFloat("SE", seVol);
        }

    }

    public void ToggleMasterMute(Toggle toggle)
    {
        isMasterMute = toggle.isOn;
    }

    public void ToggleBgmMute(Toggle toggle)
    {
        isBgmMute = toggle.isOn;
    }

    public void ToggleSeMute(Toggle toggle)
    {
        isSeMute = toggle.isOn;
    }
}
