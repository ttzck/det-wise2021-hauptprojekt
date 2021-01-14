using Bolt;
using Bolt.Matchmaking;
using TMPro;
using UnityEngine;

public class LobbyManager : GlobalEventListener
{
    [Header("Scenes")]
    [SerializeField] private string gameScene;

    [Header("UI")]
    [SerializeField] private TMP_Text connectionUiText;
    [SerializeField] private TMP_InputField sesssionIdUiInputField;
    [SerializeField] private GameObject startGameUiElement;

    private void Update()
    {
        connectionUiText.text = BoltMatchmaking.CurrentSession.ConnectionsCurrent.ToString();
        sesssionIdUiInputField.text = BoltMatchmaking.CurrentSession.HostName;
        startGameUiElement.SetActive(BoltNetwork.IsServer);
    }

    public void StartGame()
    {
        BoltNetwork.LoadScene(gameScene);
    }
}
