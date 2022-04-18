using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    #region public and Serialize variables

    [SerializeField] BGMInfo[] bgm;
    [SerializeField] private float fadeDuration;

    #endregion

    #region private variables

    private static AudioSource audioSource;
    private static Dictionary<BGMInfo.Pattern, AudioClip> bgmDictionary = new Dictionary<BGMInfo.Pattern, AudioClip>();
    private static float _fadeDuration;

    #endregion

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        _fadeDuration = fadeDuration;

        bgmDictionary.Clear();
        foreach(BGMInfo info in bgm)
        {
            bgmDictionary.Add(info.pattern, info.clip);
        }

        /*
        audioSource.clip = bgmDictionary[BGMInfo.Pattern.defence];
        audioSource.loop = true;
        audioSource.Play();
        */
    }

    public static async Task ChangeBGM(BGMInfo.Pattern next, bool loop = true)
    {
        if(audioSource.isPlaying) await FadeOut();

        if(next == BGMInfo.Pattern.none)
        {
            return;
        }

        audioSource.clip = bgmDictionary[next];
        audioSource.volume = 1;
        audioSource.loop = loop;
        audioSource.Play();



        if (!loop)
        {
            await Task.Delay((int)(audioSource.clip.length * 1000));
        }
    }

    public static async Task FadeOut()
    {
        float volume = 1;
        //Debug.Log("Fade start.");
        for(float i = 0; i < _fadeDuration; i += Time.unscaledDeltaTime)
        {
            volume = Mathf.Lerp(1, 0, i / _fadeDuration);

            audioSource.volume = volume;

            //FPS計測
            float fps = 1 / Time.unscaledDeltaTime;

            //1フレーム待機
            await Task.Delay((int)(1000 / fps));
        }

        //Debug.Log("Fade finished");
        audioSource.Stop();
    }
}

[System.Serializable]
public class BGMInfo
{
    public enum Pattern { none, start, defence, boss, clear, result, title};
    public Pattern pattern;
    public AudioClip clip;
}
