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

    public static bool blindInitialize;
    private static float blind = 0;

    private Image EYE;
    private static Color EYECOLOR = Color.white;



    void Start()
    {
        if(blindInitialize)
        {
            blind = 0;
            blindInitialize = false;
        }

        transform.SetParent(EYE_ANCHOR);

        EYE = GameObject.Find("EyeFog").GetComponent<Image>();


        blind -= 0.2f;

        EYECOLOR.a = blind;
        EYE.color = EYECOLOR;

        SetCache();
    }

    private void BlindControl(float diff)
    {
        blind += diff;

        EYECOLOR.a = blind;
        EYE.color = EYECOLOR;
    }

    


    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);


        float diff = (float)damage / cacheHealth;

        BlindControl(diff);

    }

    public override void Repaired(int amount)
    {
        base.Repaired(amount);

        float diff = (float)amount / cacheHealth;

        BlindControl(-diff);
    }

    public override void Repaired(float percent)
    {
        base.Repaired(percent);

        float diff = (float)percent / 100;

        BlindControl(-diff);
    }
}
