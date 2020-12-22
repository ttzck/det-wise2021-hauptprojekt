using UnityEngine;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Using Unity Messages")]
public class CollectableDataMediator : DataMediator<ICollectable>
{
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    public override void Attached()
    {
        base.Attached();

        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        UpdateComponents();
    }

    private void OnEnabledChanged() => UpdateComponents();


    private void UpdateComponents()
    {
        spriteRenderer.enabled = state.Enabled;
        if (col != null) col.enabled = state.Enabled;
    }
}