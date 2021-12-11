using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageGenerator : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> images;
    [SerializeField] Image image;
    [SerializeField] Transform startPos;
    [SerializeField] Transform parent;
    private int index = 0;
    private bool upper;

    private void Start()
    {
        GenerateImage();
    }



    private async Task GenerateImage()
    {
#if UNITY_EDITOR
        while(UnityEditor.EditorApplication.isPlaying)
#elif UNITY_STANDALONE
        while(UnityEngine.Application.isPlaying)
#endif
        {
            image.sprite = images[index].sprite;
            index++;
            if(index > images.Count - 1)
            {
                index = 0;
            }

            float additionalY = 50;
            if(!upper)
            {
                additionalY *= -1;
            }
            upper = !upper;
    
            Instantiate(image, new Vector2(startPos.position.x,startPos.position.y + additionalY), image.transform.rotation, parent);

            await Task.Delay(1500);
        }
    }
}
