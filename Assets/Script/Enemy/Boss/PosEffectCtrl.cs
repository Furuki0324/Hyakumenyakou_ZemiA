using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PosEffectCtrl : MonoBehaviour
{
    [SerializeField]
    float scale;
    static Transform _EFFECT_CANVAS;
    static Transform EFFECT_CANVAS
    {
        get
        {
            if (_EFFECT_CANVAS == null)
            {
                GameObject go = GameObject.Find("EffectCanvas");
                _EFFECT_CANVAS = go.transform;
            }
            return _EFFECT_CANVAS;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(scale,scale,0.0f);
        transform.SetParent(EFFECT_CANVAS, false);
    }
}
