using UnityEngine;
using Bolt;
using UnityEngine.UI;

public class CooldownIndicator : EntityBehaviour<IGolfBallState>
{
    [SerializeField] private GameObject cooldownIndicatorPrefab;

    private Image indicator;

    public override void Attached()
    {
        indicator = Instantiate(cooldownIndicatorPrefab, transform)
            .GetComponentInChildren<Image>();
    }

    private void Update() =>
        indicator.fillAmount = 1 - state.CooldownRatio;

    private void OnDestroy() => Destroy(indicator.gameObject);
}
