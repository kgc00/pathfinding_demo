using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
class AbilityPanel : MonoBehaviour {
    private Image[] images;
    private Animation[] wrappers;
    private AnimationClip[] clips;
    GameObject tooltip;
    GameObject abilityElement;
    void Awake () {
        Hero.onUnitCreated += PopulateAbilityPanel;
        SetupState.onAreaLoaded += StopAnimations;
        PlayerIdleState.onAbilitySet += AnimateAbilityPrepped;
        PlayerPrepState.onAbilitySet += AnimateAbilityPrepped;
        PlayerPrepState.onAbilityCommited += AnimateAbilityCommited;
        clips = new AnimationClip[2];
        clips[0] = (AnimationClip) Resources.Load ("Animations/ScaleImage", typeof (AnimationClip));
        clips[1] = (AnimationClip) Resources.Load ("Animations/ColorImage", typeof (AnimationClip));
        try {
            tooltip = Instantiate (Resources.Load<GameObject> ("Prefabs/UI/Tooltip"));
            tooltip.gameObject.SetActive (false);
            tooltip.name = "Tooltip";

            var canvas = new GameObject ();
            canvas.AddComponent<Canvas> ();
            canvas.name = "Tooltip Canvas";
            canvas.transform.SetParent (GameObject.Find ("Canvas").transform);
            canvas.transform.localScale = new Vector3 (1, 1, 1);

            var canvasRect = canvas.GetComponent<RectTransform> ();
            canvasRect.anchorMin = new Vector2 (0, 0);
            canvasRect.anchorMax = new Vector2 (1, 1);
            canvasRect.anchoredPosition = new Vector2 (0, 0);
            canvasRect.offsetMax = new Vector2 (0, 0);
            canvasRect.offsetMin = new Vector2 (0, 0);

            tooltip.transform.SetParent (canvas.transform);
            tooltip.transform.localScale = new Vector3 (1, 1, 1);
            tooltip.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, 0);

            abilityElement = Resources.Load<GameObject> ("Prefabs/UI/Ability Element");
        } catch (System.Exception) {
            Debug.Log (string.Format ("unable to load tooltip or ability element prefab"));
            throw;
        }
    }

    private void StopAnimations () {
        foreach (var item in images) {
            item.GetComponent<Animation> ().Stop ();
            item.color = Color.white;
        }
        foreach (var item in wrappers) {
            item.Stop ();
            item.transform.localScale = new Vector3 (1, 1, 1);
        }
    }

    ~AbilityPanel () {
        Hero.onUnitCreated -= PopulateAbilityPanel;
        PlayerIdleState.onAbilitySet -= AnimateAbilityPrepped;
        PlayerIdleState.onEntered -= StopAnimations;
    }

    void PopulateAbilityPanel (Unit unit) {
        var abilities = unit.AbilityComponent.EquippedAbilities;
        images = new Image[abilities.Count];
        wrappers = new Animation[abilities.Count];
        for (int i = 0; i < abilities.Count; i++) {
            CreatePanelItem (abilities[i], i);
        }
    }

    private void CreatePanelItem (Ability ability, int index) {
        var panelItem = Instantiate (abilityElement);
        panelItem.name = string.Format ("{0} Panel", ability.name);
        panelItem.transform.SetParent (transform);
        panelItem.transform.localScale = new Vector3 (1, 1, 1);
        var tooltipTrigger = panelItem.AddComponent<AbilityTooltip> ();
        tooltipTrigger.Initialize (tooltip, ability);
        // var rect = panelItem.AddComponent<RectTransform> ();
        // rect.pivot = new Vector2 (0, 0);
        // rect.sizeDelta = new Vector2 (180, 75);
        // var layoutGroup = panelItem.AddComponent<HorizontalLayoutGroup> ();
        // layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        // layoutGroup.childForceExpandHeight = true;
        // layoutGroup.childForceExpandWidth = true;
        // layoutGroup.childControlHeight = false;
        // layoutGroup.childControlWidth = false;

        SetChildImage (panelItem, ability, index);
        SetChildText (panelItem, ability, index);
    }

    private void SetChildImage (GameObject panelItem, Ability ability, int index) {
        images[index] = panelItem.transform
            .Find ("Wrapper/Image Wrapper/Ability Image")
            .GetComponent<Image> ();
        wrappers[index] = panelItem.transform.Find ("Wrapper").GetComponent<Animation> ();
    }

    private void SetChildText (GameObject panelItem, Ability ability, int index) {
        var textChild = panelItem.transform.Find ("Wrapper/Text Wrapper/Key Text");
        textChild.GetComponent<TextMeshProUGUI> ().SetText ((index + 1).ToString ());
    }

    // private void SetChildText (GameObject panelItem, Ability ability, int index) {
    //     var textParent = new GameObject (string.Format ("Text Holder"));
    //     textParent.transform.SetParent (panelItem.transform);
    //     textParent.transform.localScale = new Vector3 (1, 1, 1);

    //     var layoutGroup = textParent.AddComponent<VerticalLayoutGroup> ();
    //     layoutGroup.childAlignment = TextAnchor.MiddleCenter;
    //     layoutGroup.padding = new RectOffset (0, 0, 15, 15);
    //     layoutGroup.childForceExpandHeight = true;
    //     layoutGroup.childForceExpandWidth = true;
    //     layoutGroup.childControlHeight = false;
    //     layoutGroup.childControlWidth = false;

    //     var textChild1 = new GameObject (string.Format ("Button Text"));
    //     textChild1.transform.SetParent (textParent.transform);
    //     textChild1.transform.localScale = new Vector3 (1, 1, 1);

    //     var rect1 = textChild1.AddComponent<RectTransform> ();
    //     rect1.pivot = new Vector2 (0.5f, 0.5f);
    //     rect1.sizeDelta = new Vector2 (80, 30);

    //     var textComponent1 = textChild1.AddComponent<Text> ();
    //     textComponent1.alignment = TextAnchor.MiddleCenter;
    //     textComponent1.text = "Press: " + (index + 1).ToString ();
    //     textComponent1.fontSize = 26;
    //     textComponent1.font = Resources.GetBuiltinResource (typeof (Font), "Arial.ttf") as Font;
    //     textComponent1.resizeTextForBestFit = true;

    //     var textChild2 = new GameObject (string.Format ("Ability Text"));
    //     textChild2.transform.SetParent (textParent.transform);
    //     textChild2.transform.localScale = new Vector3 (1, 1, 1);

    //     var rect2 = textChild2.AddComponent<RectTransform> ();
    //     rect2.pivot = new Vector2 (0.5f, 0.5f);
    //     rect2.sizeDelta = new Vector2 (80, 30);

    //     var textComponent2 = textChild2.AddComponent<Text> ();
    //     textComponent2.alignment = TextAnchor.MiddleCenter;
    //     textComponent2.text = ability.DisplayName;
    //     textComponent2.fontSize = 18;
    //     textComponent2.resizeTextForBestFit = true;
    //     textComponent2.font = Resources.GetBuiltinResource (typeof (Font), "Arial.ttf") as Font;
    // }

    void AnimateAbilityPrepped (Unit unit, int slot) {
        StopAnimations ();
        images[slot].GetComponent<Animation> ().Play ();
    }

    void AnimateAbilityCommited (Unit unit, int slot) {
        images[slot].GetComponent<Animation> ().Stop ();
        wrappers[slot].Play ("ScaleImage");
        images[slot].color = Color.white;
    }
}