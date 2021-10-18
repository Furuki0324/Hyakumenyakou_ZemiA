using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemClickDetect : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    private enum Item { none, Face_Eye, Face_Ear, Face_Mouth}
    [SerializeField] private Item item;

    public AudioSource audioSource;

    [Header("Sound")]
    public AudioClip onPointerEnter;
    public AudioClip onPointerClick;

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioSource.PlayOneShot(onPointerEnter);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked.");
        audioSource.PlayOneShot(onPointerClick);
        DropItemManager.MoreSpendingElement(item.ToString());
    }
}
