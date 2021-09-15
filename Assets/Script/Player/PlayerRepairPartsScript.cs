using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepairPartsScript : MonoBehaviour
{
    //Private
    [ReadOnly, SerializeField]private List<FacePartsBaseScript> partsList = new List<FacePartsBaseScript>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FacePartsBaseScript baseScript = collision.gameObject.GetComponent<FacePartsBaseScript>();
        if (baseScript)
        {
            partsList.Add(baseScript);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FacePartsBaseScript baseScript = collision.gameObject.GetComponent<FacePartsBaseScript>();
        if (baseScript)
        {
            partsList.Remove(baseScript);
        }
    }

}
