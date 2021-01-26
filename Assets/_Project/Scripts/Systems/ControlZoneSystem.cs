using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class ControlZoneSystem : ServerSystem
{
    private const float ZoneRadius = .5f;

    public override void Execute(IGameState gameState)
    {
        UpdateZones();
        CheckWinCondition(gameState);
    }

    private static void UpdateZones()
    {
        foreach (var zone in SystemUtils.FindAll<IControlZone>())
        {
            var inZone = SystemUtils.FindAll<IGolfBallState>()
                .Where(i => Vector2.Distance(zone.Transform.Position, i.Transform.Position) < ZoneRadius);

            if (!inZone.Any())
            {
                if (!zone.Captured)
                {
                    Reset(zone);
                }
            }
            else if ((zone.TeamId == 0 || zone.TeamId == inZone.First().TeamId)
                && inZone.All(i => i.TeamId == inZone.First().TeamId))
            {
                zone.TeamId = inZone.First().TeamId;
                zone.Color = inZone.First().Color;
            }
            else
            {
                Reset(zone);
            }

            zone.Captured = Time.time - zone.CaptureTimestamp > GlobalSettings.ZoneCaptureTime;
            zone.CaptureRatio = Mathf.Clamp01((Time.time - zone.CaptureTimestamp) / GlobalSettings.ZoneCaptureTime);
        }
    }

    private static void Reset(IControlZone zone)
    {
        zone.TeamId = 0;
        zone.CaptureTimestamp = Time.time;
        zone.Color = Color.white;
    }

    private void CheckWinCondition(IGameState game)
    {
        var zones = SystemUtils.FindAll<IControlZone>();

        if (zones.Any() && zones.First().TeamId != 0
            && zones.All(zone => zone.TeamId == zones.First().TeamId && zone.Captured))
        {
            game.IsOver = true;
            game.WinnerColor = zones.First().Color;
        }
    }
}
