using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEditor;

[ExecuteAlways]
[RequireComponent(typeof(VideoPlayer))]
public class AttackCollisionControl : MonoBehaviour
{
    private RawImage image;
    private VideoPlayer player;
    public float FXStart = 0, FXEnd = 0;
    [Range(0, 1)]
    [SerializeField] float startTime;

    [Range(0, 1)]
    [SerializeField] float endTime;

    private void Start()
    {
        player = GetComponent<VideoPlayer>();
    }

    private void Update()
    {
        if (Application.IsPlaying(gameObject) || EditorApplication.isPlaying)
        {
            
        }
        else
        {

            //Debug.Log("Update");
            image = GetComponent<RawImage>();
            player = GetComponent<VideoPlayer>();
            FXEnd = (float)player.clip.length;

            if (player.targetTexture == null)
            {
                CreateRenderTexture();
            }

            if (player)
            {
                player.time = FXEnd * startTime;
                //Debug.Log(player.time);

                player.Play();
                player.Pause();
            }
        }
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
        if (!player.isPlaying) return false;
        if (player.time >= startTime * FXEnd && player.time <= endTime * FXEnd) return true;
        else return false;
    }
}

