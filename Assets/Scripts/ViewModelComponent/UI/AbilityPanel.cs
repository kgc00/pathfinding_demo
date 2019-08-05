using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class AbilityPanel : MonoBehaviour {
    private Image[] images;
    private AnimationClip[] clips;
    void Awake () {
        Hero.onUnitCreated += PopulateAbilityPanel;
        PlayerIdleState.onAbilitySet += AnimateAbilityPrepped;
        PlayerPrepState.onAbilityCommited += AnimateAbilityCommited;
        clips = new AnimationClip[2];
        clips[0] = (AnimationClip) Resources.Load ("Animations/ScaleImage", typeof (AnimationClip));
        clips[1] = (AnimationClip) Resources.Load ("Animations/ColorImage", typeof (AnimationClip));

    }

    ~AbilityPanel () {
        Hero.onUnitCreated -= PopulateAbilityPanel;
        PlayerIdleState.onAbilitySet -= AnimateAbilityPrepped;
    }

    void PopulateAbilityPanel (Unit unit) {
        var abilities = unit.AbilityComponent.EquippedAbilities;
        images = new Image[abilities.Count];
        for (int i = 0; i < abilities.Count; i++) {
            CreatePanelItem (abilities[i], i);
        }
    }

    private void CreatePanelItem (Ability ability, int index) {
        var panelItem = new GameObject (string.Format ("{0} Panel", ability.name));
        panelItem.transform.SetParent (transform);
        panelItem.transform.localScale = new Vector3 (1, 1, 1);
        var rect = panelItem.AddComponent<RectTransform> ();
        rect.pivot = new Vector2 (0, 0);
        rect.sizeDelta = new Vector2 (180, 75);
        var layoutGroup = panelItem.AddComponent<HorizontalLayoutGroup> ();
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        layoutGroup.childForceExpandHeight = true;
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childControlHeight = false;
        layoutGroup.childControlWidth = false;
        CreateTextChild (panelItem, ability, index);
        CreateImageChild (panelItem, ability, index);
    }

    private void CreateImageChild (GameObject panelItem, Ability ability, int index) {
        var imageChild = new GameObject (string.Format ("Image"));
        imageChild.transform.SetParent (panelItem.transform);
        imageChild.transform.localScale = new Vector3 (1, 1, 1);

        var rect = imageChild.AddComponent<RectTransform> ();
        rect.pivot = new Vector2 (0.5f, 0.5f);
        rect.sizeDelta = new Vector2 (55, 55);

        images[index] = imageChild.AddComponent<Image> ();
        var animation = images[index].gameObject.AddComponent<Animation> ();
        animation.AddClip (clips[0], "scale");
        animation.AddClip (clips[1], "color");
    }

    private void CreateTextChild (GameObject panelItem, Ability ability, int index) {
        var textParent = new GameObject (string.Format ("Text Holder"));
        textParent.transform.SetParent (panelItem.transform);
        textParent.transform.localScale = new Vector3 (1, 1, 1);

        var layoutGroup = textParent.AddComponent<VerticalLayoutGroup> ();
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        layoutGroup.padding = new RectOffset (0, 0, 15, 15);
        layoutGroup.childForceExpandHeight = true;
        layoutGroup.childForceExpandWidth = true;
        layoutGroup.childControlHeight = false;
        layoutGroup.childControlWidth = false;

        var textChild1 = new GameObject (string.Format ("Button Text"));
        textChild1.transform.SetParent (textParent.transform);
        textChild1.transform.localScale = new Vector3 (1, 1, 1);

        var rect1 = textChild1.AddComponent<RectTransform> ();
        rect1.pivot = new Vector2 (0.5f, 0.5f);
        rect1.sizeDelta = new Vector2 (80, 30);

        var textComponent1 = textChild1.AddComponent<Text> ();
        textComponent1.alignment = TextAnchor.MiddleCenter;
        textComponent1.text = "Press: " + (index + 1).ToString ();
        textComponent1.fontSize = 26;
        textComponent1.font = Resources.GetBuiltinResource (typeof (Font), "Arial.ttf") as Font;
        textComponent1.resizeTextForBestFit = true;

        var textChild2 = new GameObject (string.Format ("Ability Text"));
        textChild2.transform.SetParent (textParent.transform);
        textChild2.transform.localScale = new Vector3 (1, 1, 1);

        var rect2 = textChild2.AddComponent<RectTransform> ();
        rect2.pivot = new Vector2 (0.5f, 0.5f);
        rect2.sizeDelta = new Vector2 (80, 30);

        var textComponent2 = textChild2.AddComponent<Text> ();
        textComponent2.alignment = TextAnchor.MiddleCenter;
        textComponent2.text = ability.DisplayName;
        textComponent2.fontSize = 18;
        textComponent2.resizeTextForBestFit = true;
        textComponent2.font = Resources.GetBuiltinResource (typeof (Font), "Arial.ttf") as Font;
    }

    void AnimateAbilityPrepped (Unit unit, int slot) {
        images[slot].GetComponent<Animation> ().Play ("color");
    }
    void AnimateAbilityCommited (Unit unit, int slot) {
        var animation = images[slot].GetComponent<Animation> ();
        animation.Stop ("color");
        images[slot].color = Color.white;
        animation.Play ("scale");
    }
}