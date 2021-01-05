using UnityEngine;

public class ControlZoneDataMediator : DataMediator<IControlZone>
{
    private void OnColorChanged()
    {
        GetComponent<SpriteRenderer>().color = state.Color;
    }
}
