using System;
using System.IO;
using System.Linq;
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
        Hero.onUnitCreated += SafelyPopulatePanel;
        SetupState.onAreaLoaded += StopAnimations;
        PlayerIdleState.onAbilitySet += AnimateAbilityPrepped;
        PlayerPrepState.onAbilitySet += AnimateAbilityPrepped;
        PlayerPrepState.onAbilityCanceled += StopAnimations;
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

            SafelyPopulatePanel ();
        } catch (System.Exception) {
            Debug.Log (string.Format ("unable to load tooltip or ability element prefab"));
            throw;
        }
    }

    private void SafelyPopulatePanel () {
        if (images == null) {
            var hero = FindObjectOfType<Board> ().Units.FirstOrDefault (data => data.Value is Hero).Value;
            if (hero == null) return;
            PopulateAbilityPanel (hero);
        }
    }

    private void SafelyPopulatePanel (Unit unit) {
        if (images == null) {
            PopulateAbilityPanel (unit);
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

    void OnDestroy () {
        Hero.onUnitCreated -= SafelyPopulatePanel;
        SetupState.onAreaLoaded -= StopAnimations;
        PlayerIdleState.onAbilitySet -= AnimateAbilityPrepped;
        PlayerPrepState.onAbilitySet -= AnimateAbilityPrepped;
        PlayerPrepState.onAbilityCanceled -= StopAnimations;
        PlayerPrepState.onAbilityCommited -= AnimateAbilityCommited;
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

        SetChildImage (panelItem, ability, index);
        SetChildText (panelItem, ability, index);
    }

    private void SetChildImage (GameObject panelItem, Ability ability, int index) {
        AssignImageRefs (panelItem, index);
        LoadIcon (ability, index);
    }

    private void AssignImageRefs (GameObject panelItem, int index) {
        images[index] = panelItem.transform
            .Find ("Wrapper/Image Wrapper/Ability Image")
            .GetComponent<Image> ();
        wrappers[index] = panelItem.transform.Find ("Wrapper").GetComponent<Animation> ();
    }

    ///<summary>
    /// Loads Icon from resources.
    /// <para>
    /// All icons must be 64x64 or they will not fit properly
    /// </para>
    ///</summary>
    private void LoadIcon (Ability ability, int index) {
        var resourcesPath = "Art/Abilities/" + ability.DisplayName;
        var fullApplicationPath = Application.dataPath + "/Resources/" + resourcesPath + ".png";

        if (File.Exists (fullApplicationPath)) images[index].sprite = Resources.Load<Sprite> (resourcesPath);
        else Debug.Log (string.Format ("could not find icon for: {0}", ability.DisplayName));
    }

    private void SetChildText (GameObject panelItem, Ability ability, int index) {
        var textChild = panelItem.transform.Find ("Wrapper/Text Wrapper/Key Text");
        textChild.GetComponent<TextMeshProUGUI> ().SetText ((index + 1).ToString ());
    }

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