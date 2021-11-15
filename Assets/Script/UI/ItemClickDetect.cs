using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class ItemClickDetect : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
{
    private enum Item { none, Face_Eye, Face_Ear, Face_Mouth}
    [SerializeField] private Item item;


    [Header("Sound")]
    public AudioClip onPointerEnter;
    public AudioClip onPointerClick;
    [SerializeField] private AudioMixerGroup sfxMixer;

    public void OnPointerEnter(PointerEventData eventData)
    {
        _ = NonSpatialSFXPlayer.PlayNonSpatialSFX(onPointerEnter, sfxMixer);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            _ = NonSpatialSFXPlayer.PlayNonSpatialSFX(onPointerClick, sfxMixer);
            DropItemManager.MoreSpendingElement(item.ToString());
        }

    }
}
