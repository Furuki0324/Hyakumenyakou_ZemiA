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

    [SerializeField] private Image EYE;
    private static Color EYECOLOR = Color.white;
    public float SPEED;


    void Start()
    {
        transform.SetParent(EYE_ANCHOR);

        EYE = GameObject.Find("EyeFog").GetComponent<Image>();


        volume -= 0.2f;

        EYECOLOR.a = volume;
        EYE.color = EYECOLOR;

        SetCache();
    }

    public override void TakeDamage()
    {
        base.TakeDamage();

        Debug.Log("Eye take damage.");
        /*
        if (Mathf.Approximately(health, cacheHealth * 0.8f)) Volume(0.8f);
        else if (Mathf.Approximately(health, cacheHealth * 0.6f)) Volume(0.6f);
        else if (Mathf.Approximately(health, cacheHealth * 0.4f)) Volume(0.4f);
        Debug.Log("overriden");
        */
        volume += SPEED;
        volume = Mathf.Clamp(volume, -1, 99);
        EYECOLOR.a = volume;
        EYE.color = EYECOLOR;

    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        volume += SPEED;
        volume = Mathf.Clamp(volume, -1, 1);
        EYECOLOR.a = volume;
        EYE.color = EYECOLOR;

    }

    public override void Repaired(int amount)
    {
        base.Repaired(amount);

        volume -= SPEED;

        EYECOLOR.a = volume;
        EYE.color = EYECOLOR;
    }
}
