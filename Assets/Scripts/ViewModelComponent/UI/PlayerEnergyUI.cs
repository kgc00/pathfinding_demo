using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEnergyUI : MonoBehaviour {
    private struct CoroutineInfo {
        public EnergyFillInfo energyFillInfo;
        public Coroutine coroutine;
        public CoroutineInfo (EnergyFillInfo _energyFillInfo, Coroutine _coroutine) {
            energyFillInfo = _energyFillInfo;
            coroutine = _coroutine;
        }
    }
    private struct EnergyFillInfo {
        public float currentEnergy;
        public float prevAmount;
        public Unit unit;
        public EnergyFillInfo (float _currentEnergy, float _prevAmount, Unit _unit) {
            currentEnergy = _currentEnergy;
            prevAmount = _prevAmount;
            unit = _unit;
        }
    }
    public float animationDuration = 0.5f;
    private Dictionary<Unit, CoroutineInfo> currentCoroutines;

    void Awake () {
        currentCoroutines = new Dictionary<Unit, CoroutineInfo> ();
        EnergyComponent.onEnergyChanged += PrepAnimation;
    }

    void OnDestroy () {
        EnergyComponent.onEnergyChanged -= PrepAnimation;
    }

    private void PrepAnimation (Unit unit, float prevAmount) {
        if (currentCoroutines.ContainsKey (unit)) {
            StopCoroutine (currentCoroutines[unit].coroutine);
            currentCoroutines.Remove (unit);
        }

        var currentEnergy = unit.EnergyComponent.data.CurrentEnergy;
        var maxEnergy = unit.EnergyComponent.data.MaxEnergy;
        var fillAmount = Mathf.Clamp01 ((currentEnergy) / maxEnergy);
        var fromAmount = Mathf.Clamp01 (prevAmount / maxEnergy);
        EnergyFillInfo fillInfo = new EnergyFillInfo (fillAmount, fromAmount, unit);
        CoroutineInfo coroutineInfo = new CoroutineInfo (fillInfo, StartCoroutine ("AnimateEnergyBar", fillInfo));
        currentCoroutines.Add (unit, coroutineInfo);
    }

    private IEnumerator AnimateEnergyBar (EnergyFillInfo info) {
        Image foregroundImage = transform.Find ("Energybar Wrapper/Foreground").GetComponent<Image> ();
        float elapsed = 0f;
        while (elapsed < animationDuration) {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Clamp01 (Mathf.Lerp (info.prevAmount, info.currentEnergy, elapsed / animationDuration));
            yield return null;
        }
        currentCoroutines.Remove (info.unit);
    }
}