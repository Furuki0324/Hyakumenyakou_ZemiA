using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Video;
using UnityEngine.UI;
using Cinemachine;

public class GameClearUIAnimation : MonoBehaviour
{
    /*
     *アニメーションのイベントとして呼び出すための関数が入っています
     */

    [SerializeField] private CinemachineVirtualCamera resultCamera;

    [Header("Audio")]
    [SerializeField] private AudioClip slidingSFX;
    [SerializeField] private AudioClip sumSFX;
    [SerializeField] private AudioMixerGroup sfxMixer;

    private void SwitchCamera()
    {
        resultCamera.gameObject.SetActive(true);

        Camera camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        //レイヤー5番(UI)と10番(顔面の背景)と11番(顔パーツ)と17番(ボス憑依時の特殊レイヤー)と18番(Cinemachine)にマスクをセット
        int bit1 = 1 << 5;
        int bit2 = 1 << 10;
        int bit3 = 1 << 11;
        int bit4 = 1 << 17;
        int bit5 = 1 << 18;
        int mask = bit1 | bit2 | bit3 | bit4 | bit5;
        camera.cullingMask = mask;

        BGMPlayer.ChangeBGM(BGMInfo.Pattern.result);
    }

    private void PlaySlidingSound()
    {
        NonSpatialSFXPlayer.PlayNonSpatialSFX(slidingSFX, sfxMixer);
    }

    private void PlayFinalSound()
    {
        NonSpatialSFXPlayer.PlayNonSpatialSFX(sumSFX, sfxMixer);
    }
}
