using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class testdo : MonoBehaviour
{
    private SpriteRenderer bossSprite;

    // Start is called before the first frame update
    void Start()
    {
        bossSprite = gameObject.GetComponent<SpriteRenderer>();
        var bossTempColor = bossSprite.color;
        transform.DOMoveY(transform.position.y + -1f, 1f)
            .OnStart(() => bossSprite.color = new Color(bossTempColor.r, bossTempColor.g, bossTempColor.b, 0f))
            .OnUpdate(() => bossSprite.color += new Color(0, 0, 0, 1f * Time.deltaTime))
            .OnComplete(() => bossSprite.color = new Color(bossTempColor.r, bossTempColor.g, bossTempColor.b, 1f));
    }

    // Update is called once per frame
    void Update()
    {
    }
}