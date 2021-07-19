using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EyeScript : FacePartsBaseScript
{
    static Transform _EYE_ANCHOR;
    static Transform EYE_ANCHOR
    {
        get
        {
            if(_EYE_ANCHOR == null)
            {
                GameObject go = new GameObject("EYE_ANCHOR");
                _EYE_ANCHOR = go.transform;
            }
            return _EYE_ANCHOR;
        }
    }


    // Start is called before the first frame update
    private static float volume = 0;
    [SerializeField]
    private float hp = 20; //体力
    private float cacheHp;
    [SerializeField] private Image EYE;
    private static Color EYECOLOR = Color.white;
    public float SPEED;

    private void Volume(float a)
    {
        switch (a)
        {
            case 0.8f:
                Debug.Log("call");
                volume += SPEED;
                break;

            case 0.6f:
                Debug.Log("call2");
                volume+= SPEED;
                break;

            case 0.4f:
                Debug.Log("call3");
                volume+= SPEED;
                break;
        }

        //volume = Mathf.Clamp(volume, 0, 1);
        EYECOLOR.a = volume;
        EYE.color = EYECOLOR;
    }

    void Start()
    {
        transform.SetParent(EYE_ANCHOR);

        EYE = GameObject.Find("Image").GetComponent<Image>();


        volume -= 0.2f;

        EYECOLOR.a = volume;
        EYE.color = EYECOLOR;
        cacheHp = hp;
    }

    // Update is called once per frame
    /*
    private float cacheTime = 0;
    void Update()
    {
        if (Time.time > cacheTime + 2)
        {
            hp--;
            cacheTime = Time.time;

            if (Mathf.Approximately(hp, cacheHp * 0.8f)) Volume(0.8f);
            else if (Mathf.Approximately(hp, cacheHp * 0.6f)) Volume(0.6f);
            else if (Mathf.Approximately(hp, cacheHp * 0.4f)) Volume(0.4f);
        }
    }
    */
    public override void TakeDamage()
    {

        hp--;
        health--;

        /*
        if (Mathf.Approximately(health, cacheHealth * 0.8f)) Volume(0.8f);
        else if (Mathf.Approximately(health, cacheHealth * 0.6f)) Volume(0.6f);
        else if (Mathf.Approximately(health, cacheHealth * 0.4f)) Volume(0.4f);
        Debug.Log("overriden");
        */
        volume += SPEED;
        volume = Mathf.Clamp(volume, -1, 1);
        EYECOLOR.a = volume;
        EYE.color = EYECOLOR;
    }
}
