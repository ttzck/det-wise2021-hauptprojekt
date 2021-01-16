using Bolt;
using Bolt.Matchmaking;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyManager : GlobalEventListener
{
    [Header("Scenes")]
    [SerializeField] private string gameScene;

    [Header("UI")]
    [SerializeField] private TMP_Text connectionUiText;
    [SerializeField] private TMP_InputField sesssionIdUiInputField;
    [SerializeField] private List<GameObject> serverUI;

    [Header("Setting")]
    [SerializeField] private TMP_InputField NumberOfGolfBallsPerTeamField;
    [SerializeField] private TMP_InputField CollectableSpawnCooldownField;
    [SerializeField] private TMP_InputField ForceScaleField;
    [SerializeField] private TMP_InputField CollectablesWinConditionField;
    [SerializeField] private TMP_InputField GolfBallCooldownField;

    private void Start()
    {
        foreach (var ui in serverUI)
            ui.SetActive(BoltNetwork.IsServer);
        DisplaySettings();
    }

    private void Update()
    {
        connectionUiText.text = BoltMatchmaking.CurrentSession.ConnectionsCurrent.ToString();
        sesssionIdUiInputField.text = BoltMatchmaking.CurrentSession.HostName;
        UpdateSettings();
        DisplaySettings();
    }

    private void DisplaySettings()
    {
        NumberOfGolfBallsPerTeamField.text = GlobalSettings.NumberOfGolfBallsPerTeam.ToString();
        CollectableSpawnCooldownField.text = GlobalSettings.CollectableSpawnCooldown.ToString();
        ForceScaleField.text = GlobalSettings.ForceScale.ToString();
        CollectablesWinConditionField.text = GlobalSettings.CollectablesWinCondition.ToString();
        GolfBallCooldownField.text = GlobalSettings.GolfBallCooldown.ToString();
    }

    private void UpdateSettings()
    {
        try { GlobalSettings.NumberOfGolfBallsPerTeam = int.Parse(NumberOfGolfBallsPerTeamField.text); } catch (System.FormatException e) { }
        try { GlobalSettings.CollectableSpawnCooldown = float.Parse(CollectableSpawnCooldownField.text); } catch (System.FormatException e) { }
        try { GlobalSettings.ForceScale = float.Parse(ForceScaleField.text); } catch (System.FormatException e) { }
        try { GlobalSettings.CollectablesWinCondition = int.Parse(CollectablesWinConditionField.text); } catch (System.FormatException e) { }
        try { GlobalSettings.GolfBallCooldown = float.Parse(GolfBallCooldownField.text); } catch (System.FormatException e) { }
    }

    public void StartGame()
    {
        BoltNetwork.LoadScene(gameScene);
    }
}
