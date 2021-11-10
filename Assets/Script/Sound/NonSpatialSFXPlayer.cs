using System.Threading.Tasks;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class NonSpatialSFXPlayer : MonoBehaviour
{
    private static AudioSource _audio;

    private void Awake()
    {
        _audio = GetComponent<AudioSource>();
    }

    public static async Task PlayNonSpatialSFX(AudioClip clip)
    {
        _audio.PlayOneShot(clip);
    }
}
