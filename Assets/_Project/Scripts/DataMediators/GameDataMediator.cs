using UnityEngine;
using UnityEngine.UI;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Using Unity Messages")]
public class GameDataMediator : DataMediator<IGameState>
{
    [SerializeField] private GameObject gameOverIndicator;

    private void OnIsOverChanged()
    {
        if (!state.IsOver) return;

        var ind = Instantiate(gameOverIndicator);
        ind.GetComponentInChildren<Image>().color = state.WinnerColor;
    }
}
