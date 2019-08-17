using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour {
    private struct CoroutineInfo {
        public HealthFillInfo healthFillInfo;
        public Coroutine coroutine;
        public CoroutineInfo (HealthFillInfo _healthFillInfo, Coroutine _coroutine) {
            healthFillInfo = _healthFillInfo;
            coroutine = _coroutine;
        }
    }
    private struct HealthFillInfo {
        public float currentHealth;
        public float prevAmount;
        public Unit unit;
        public HealthFillInfo (float _currentHealth, float _prevAmount, Unit _unit) {
            currentHealth = _currentHealth;
            prevAmount = _prevAmount;
            unit = _unit;
        }
    }
    public float animationDuration = 0.5f;
    private Dictionary<Unit, CoroutineInfo> currentCoroutines;

    // Use this for initialization
    void Awake () {
        currentCoroutines = new Dictionary<Unit, CoroutineInfo> ();
        HealthComponent.onHealthChanged += PrepAnimation;
    }

    void OnDestroy () {
        HealthComponent.onHealthChanged -= PrepAnimation;
    }

    private void PrepAnimation (Unit unit, int prevAmount) {
        if (unit is Monster) return;
        if (currentCoroutines.ContainsKey (unit)) {
            StopCoroutine (currentCoroutines[unit].coroutine);
            currentCoroutines.Remove (unit);
        }

        var currentHealth = unit.HealthComponent.data.CurrentHP;
        var maxHealth = unit.HealthComponent.data.MaxHP;
        float fillAmount = Mathf.Clamp01 ((float) (currentHealth) / (float) maxHealth);
        var fromAmount = Mathf.Clamp01 ((float) prevAmount / (float) maxHealth);
        HealthFillInfo fillInfo = new HealthFillInfo (fillAmount, fromAmount, unit);
        CoroutineInfo coroutineInfo = new CoroutineInfo (fillInfo, StartCoroutine ("AnimateHealthBar", fillInfo));
        currentCoroutines.Add (unit, coroutineInfo);
    }

    private IEnumerator AnimateHealthBar (HealthFillInfo info) {
        Image foregroundImage = transform.Find ("Healthbar Wrapper/Foreground").GetComponent<Image> ();
        float elapsed = 0f;
        while (elapsed < animationDuration) {
            elapsed += Time.deltaTime;
            foregroundImage.fillAmount = Mathf.Clamp01 (Mathf.Lerp (info.prevAmount, info.currentHealth, elapsed / animationDuration));
            yield return null;
        }
        currentCoroutines.Remove (info.unit);
    }
}