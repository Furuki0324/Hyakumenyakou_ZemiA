using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
[RequireComponent(typeof(VideoPlayer))]
public class AttackCollisionControl : MonoBehaviour
{
    private RawImage image;
    private VideoPlayer player;
    [SerializeField, ReadOnly] private float FXLength = 0;

    [Range(0, 1)]
    [ContextMenuItem("Set to start time", "SetSliderToStart")]
    [ContextMenuItem("Set to end time", "SetSliderToEnd")]
    [Tooltip("変数名を右クリックするとメニューを開き、コリジョンの時間設定を出来ます。")]
    [SerializeField] float timeSlider;

    [SerializeField, ReadOnly] float collisionStartTime;

    [SerializeField, ReadOnly] float collisionEndTime;

    private void Start()
    {
        player = GetComponent<VideoPlayer>();
        FXLength = (float)player.clip.length;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Application.IsPlaying(gameObject) || EditorApplication.isPlaying)
        {
            //Do nothing.
        }
        else
        {
            //Debug.Log("Update");
            image = GetComponent<RawImage>();
            player = GetComponent<VideoPlayer>();
            FXLength = (float)player.clip.length;

            if (player.targetTexture == null)
            {
                CreateRenderTexture();
            }

            if (player)
            {
                player.time = FXLength * timeSlider;
                //Debug.Log(player.time);

                //Preview
                player.Play();
                player.Pause();
            }
        }
#endif
    }

    private void CreateRenderTexture()
    {
        RenderTexture texture;
        texture = new RenderTexture(256, 256, 24);
        player.targetTexture = texture;
        image.texture = texture;
    }

    public bool IsColliderEnabled()
    {
        if (player)
        {
            if (!player.isPlaying) return false;
            if (player.time >= collisionStartTime * FXLength && player.time <= collisionEndTime * FXLength) return true;
            else return false;
        }
        else
        {
            Debug.LogWarning("No player was found: " + gameObject.name);
            RetryToGetPlayer();
            return false;
        }
    }

    private void RetryToGetPlayer()
    {
        player = GetComponent<VideoPlayer>();
        if (player)
        {
            Debug.Log("Retrying to find player was successful.");
        }
    }

#if UNITY_EDITOR
    private void SetSliderToStart()
    {
        collisionStartTime = timeSlider;
        timeSlider = 0;
    }

    private void SetSliderToEnd()
    {
        collisionEndTime = timeSlider;
        timeSlider = 0;
    }
#endif
}

