using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

    GameObject tooltip;
    CanvasScaler canvasScaler;
    Ability ability;
    public void Initialize (GameObject tooltip, Ability ability) {
        this.tooltip = tooltip;
        this.ability = ability;
        tooltip.SetActive (false);
        canvasScaler = GameObject.Find ("Canvas").GetComponent<CanvasScaler> ();
    }

    public void OnPointerEnter (PointerEventData eventData) {
        // only works with 800 x 600 =(
        RectTransform tooltipRect;
        float sizeX;
        Vector3 desiredElementPos, altPos;
        CalculatePosition (out tooltipRect, out sizeX, out desiredElementPos, out altPos);

        SetTooltipText ();

        if (desiredElementPos.x + sizeX < (1000))
            tooltipRect.SetPositionAndRotation (desiredElementPos, Quaternion.identity);
        else
            tooltipRect.SetPositionAndRotation (altPos, Quaternion.identity);
        tooltip.SetActive (true);
    }

    private void CalculatePosition (out RectTransform tooltipRect, out float sizeX, out Vector3 desiredElementPos, out Vector3 altPos) {
        var myRect = gameObject.GetComponent<RectTransform> ();
        tooltipRect = tooltip.GetComponent<RectTransform> ();
        var margin = 15f;
        sizeX = (tooltipRect.rect.size.x * 1.25f) + margin;
        var altX = tooltipRect.rect.size.x * 0.21f + margin;
        var rightEdge = (myRect.anchoredPosition.x + myRect.rect.xMax);
        desiredElementPos = new Vector3 (rightEdge + sizeX, 0, 0);
        altPos = new Vector3 (myRect.anchoredPosition.x + altX, 0, 0);
    }

    public void OnPointerExit (PointerEventData eventData) {
        tooltip.SetActive (false);
    }

    void SetTooltipText () {

        Debug.Log (string.Format ("ability is {0}", JsonUtility.ToJson (ability)));
        tooltip.transform.Find ("Header/Name").GetComponent<TextMeshProUGUI> ().SetText (ability.DisplayName);

        tooltip.transform.Find ("Target Details/Text/Range")
            .GetComponent<TextMeshProUGUI> ()
            .SetText (string.Format ("Range: {0}", ability.Range.ToString ()));

        tooltip.transform.Find ("Target Details/Text/Range Finding")
            .GetComponent<TextMeshProUGUI> ()
            .SetText (string.Format ("Range Finding: {0}", ability.RangeComponentType.ToString ()));

        tooltip.transform.Find ("Body Content/Description")
            .GetComponent<TextMeshProUGUI> ()
            .SetText (ability.Description);

        tooltip.transform.Find ("Stat Details/Damage Wrapper").gameObject.SetActive (false);
        tooltip.transform.Find ("Stat Details/AoE Wrapper").gameObject.SetActive (false);

        if (ability.AreaOfEffect > 0) {
            tooltip.transform.Find ("Stat Details/AoE Wrapper").gameObject.SetActive (true);
            tooltip.transform.Find ("Stat Details/AoE Wrapper/Amount")
                .GetComponent<TextMeshProUGUI> ()
                .SetText (ability.AreaOfEffect.ToString ());
        }

        if (ability is AttackAbility) {
            var attackAbility = (AttackAbility) ability;
            tooltip.transform.Find ("Stat Details/Damage Wrapper").gameObject.SetActive (true);
            tooltip.transform.Find ("Stat Details/Damage Wrapper/Amount")
                .GetComponent<TextMeshProUGUI> ()
                .SetText (attackAbility.Damage.ToString ());
        }

        tooltip.transform.Find ("Stat Details/Energy Wrapper/Amount")
            .GetComponent<TextMeshProUGUI> ()
            .SetText (ability.EnergyCost.ToString ());
    }
}