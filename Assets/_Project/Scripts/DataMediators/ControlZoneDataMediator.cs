using UnityEngine;
using UnityEngine.UI;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Using Unity Messages")]
public class ControlZoneDataMediator : DataMediator<IControlZone>
{
    [SerializeField] private Image captureIndicator;

    private void OnCaptureRatioChanged()
    {
        captureIndicator.fillAmount = state.CaptureRatio;
    }

    private void OnColorChanged()
    {
        captureIndicator.color = state.Color;
    }
}
