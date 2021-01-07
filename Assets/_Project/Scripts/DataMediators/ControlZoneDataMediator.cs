using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Using Unity Messages")]
public class ControlZoneDataMediator : DataMediator<IControlZone>
{
    private SpriteRenderer spriteRenderer;

    public override void Attached()
    {
        base.Attached();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnColorChanged()
    {
        var newColor = state.Color;
        newColor.a = spriteRenderer.color.a;
        spriteRenderer.color = newColor;
    }
}
