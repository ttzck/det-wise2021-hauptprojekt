using UnityEngine;
using Bolt;
using UnityEngine.UI;

public class GolfBallCooldownIndicator : EntityBehaviour<IGolfBallState>
{
    [SerializeField] private GameObject cooldownIndicatorPrefab;

    private Image indicator;

    public override void Attached()
    {
        indicator = Instantiate(cooldownIndicatorPrefab, transform)
            .GetComponentInChildren<Image>();
        state.AddCallback(nameof(state.Color), () => indicator.color = state.Color);
    }

    private void Update() =>
        indicator.fillAmount = 1 - state.CooldownRatio;

    private void OnDestroy() => Destroy(indicator.gameObject);
}
