using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlider : MonoBehaviour
{
    Transform endPos;

    private void Start()
    {
        endPos = GameObject.Find("EndPos").transform;
    }
    private void Update()
    {
        transform.position += new Vector3(-1, 0, 0);

        if(transform.position.x < endPos.position.x)
        {
            Destroy(gameObject);
        }
    }
}
