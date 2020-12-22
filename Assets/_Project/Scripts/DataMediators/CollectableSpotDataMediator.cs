using UnityEngine;
using UnityEngine.UI;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Using Unity Messages")]
public class CollectableSpotDataMediator : DataMediator<ICollectableSpot>
{
    [SerializeField] private Image cooldownIndicator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private void OnCooldownRatioChanged()
    {
        cooldownIndicator.fillAmount = state.CooldownRatio;
    }

    private void OnEnabledChanged()
    {
        cooldownIndicator.enabled = state.Enabled;
        spriteRenderer.enabled = state.Enabled;
    }
}