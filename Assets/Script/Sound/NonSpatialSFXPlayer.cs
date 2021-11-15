using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class NonSpatialSFXPlayer : MonoBehaviour
{
    private static AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    /// <summary>
    /// <para>Non spatialな効果音を再生します。UIの効果音に向いています。</para>
    /// <para>第二引数にミキサーグループを指定することができます。指定しない場合はミキサーを通さずに効果音を再生します。</para>
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="mixer"></param>
    /// <returns></returns>
    public static async Task PlayNonSpatialSFX(AudioClip clip, AudioMixerGroup mixer = null)
    {
        if(mixer == null)
        {
            Debug.LogWarning("Clip " + clip.name + " was played through no mixer.");
        }

         _audio.outputAudioMixerGroup = mixer;
        _audio.PlayOneShot(clip);
    }
}
