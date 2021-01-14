using Bolt;
using Bolt.Matchmaking;
using TMPro;
using UdpKit;
using UnityEngine;

public class LobbyMenuManager : GlobalEventListener
{
    [SerializeField] private TMP_InputField startServerSessionIdInputField;
    [SerializeField] private TMP_InputField joinServerSessionIdInputField;

    public void CreateServer()
    {
        if (IsValidSessionId(startServerSessionIdInputField.text))
        {
            BoltLauncher.StartServer();
        }
    }

    public void JoinServer()
    {
        if (IsValidSessionId(joinServerSessionIdInputField.text))
        {
            BoltLauncher.StartClient();
        }
    }

    public override void BoltStartDone()
    {
        if (BoltNetwork.IsServer)
        {
            CreateSession();
        }
        else if (BoltNetwork.IsClient)
        {
            JoinWithSessionName();
        }
    }

    private void CreateSession()
    {
        string matchName = startServerSessionIdInputField.text.Trim();

        BoltMatchmaking.CreateSession(
            sessionID: matchName,
            sceneToLoad: "Lobby"
        );
    }

    private void JoinWithSessionName()
    {
        BoltMatchmaking.JoinSession(joinServerSessionIdInputField.text);
    }

    public override void SessionConnectFailed(UdpSession session, IProtocolToken token, UdpSessionError errorReason)
    {
        BoltLauncher.Shutdown();
    }

    public override void SessionCreationFailed(UdpSession session, UdpSessionError errorReason)
    {
        BoltLauncher.Shutdown();
    }

    private bool IsValidSessionId(string id) => !string.IsNullOrWhiteSpace(id);
}