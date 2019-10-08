using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSelectPanel : MonoBehaviour, IPointerDownHandler {
    int panelIndex;
    CharacterSelectUI characterSelectUI;
    internal void Initialize (int i, CharacterSelectUI characterSelectUI) {
        this.panelIndex = i;
        this.characterSelectUI = characterSelectUI;
    }
    public void OnPointerDown (PointerEventData eventData) {
        characterSelectUI.UpdateActiveItem (panelIndex);
    }
}