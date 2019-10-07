using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class CharacterSelectUI : MonoBehaviour {
    GameObject characterWrapper;
    public List<GameObject> PanelItems { get; private set; } = new List<GameObject> ();
    Dictionary<int, PlayableUnits> panelToTypeMap = new Dictionary<int, PlayableUnits> ();
    public int ActiveItemIndex { get; private set; } = 0;
    CharacterSelector selector;
    void Awake () {
        characterWrapper = Resources.Load<GameObject> ("Prefabs/UI/Character Wrapper");
        selector = gameObject.AddComponent<CharacterSelector> ().Initialize (this);
        PopulatePanel ();
        SetActiveItem ();
    }

    // kinda hacky, just trying to avoid using anything more complex like events until i need to
    // should just fire off ONLY once and only when the player has transitioned screens from char select => game
    void OnDestroy () {
        UnitsClearedManager.SetPlayerUnit (panelToTypeMap[ActiveItemIndex]);
    }

    internal void UpdateActiveItem (int newActiveIndex) {
        ActiveItemIndex = newActiveIndex;
        SetActiveItem ();
    }

    private void SetActiveItem () {
        for (int i = 0; i < PanelItems.Count; i++) {
            if (i == ActiveItemIndex) {
                PanelItems[i].GetComponent<Image> ().enabled = true;
            } else {
                PanelItems[i].GetComponent<Image> ().enabled = false;
            }
        }
    }

    void PopulatePanel () {
        var characterSelectData = Resources.LoadAll ("Data/Character Select", typeof (CharacterSelectData)).Cast<CharacterSelectData> ().ToList ();
        for (int i = 0; i < characterSelectData.Count; i++) {
            GameObject panelItem = InstantiateItem (characterSelectData[i]);
            SetChildImage (panelItem, characterSelectData[i]);
            SetChildText (panelItem, characterSelectData[i]);
            PanelItems.Add (panelItem);
            panelToTypeMap.Add (i, characterSelectData[i].UnitType);
        }
    }

    private GameObject InstantiateItem (CharacterSelectData ability) {
        var panelItem = Instantiate (characterWrapper);
        panelItem.transform.SetParent (transform);
        panelItem.transform.localScale = new Vector3 (1, 1, 1);
        panelItem.name = ability.Name;
        return panelItem;
    }

    private void SetChildImage (GameObject panelItem, CharacterSelectData ability) {
        var image = panelItem.transform
            .Find ("Image Wrapper/Image")
            .GetComponent<Image> ();

        var resourcesPath = "Art/Abilities/" + ability.IconName;
        var fullApplicationPath = Application.dataPath + "/Resources/" + resourcesPath + ".png";

        if (File.Exists (fullApplicationPath)) image.sprite = Resources.Load<Sprite> (resourcesPath);
        else Debug.Log (string.Format ("could not find icon for: {0}", ability.IconName));
    }

    private void SetChildText (GameObject panelItem, CharacterSelectData ability) {
        panelItem.transform
            .Find ("Label Wrapper/Character Name")
            .GetComponent<TextMeshProUGUI> ()
            .SetText (ability.Name);
    }
}