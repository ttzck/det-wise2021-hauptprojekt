using System;
using UnityEngine;

public static class GlobalSettings
{
    public static Vector2 ArenaDimensions = new Vector2(16, 9);

    public static int NumberOfGolfBallsPerTeam = 3;

    public static int NumberOfCollectablesPerSpawnRound = 3;

    public static int CollectableSpawnCooldown = 10;

    public static float ForceScale = 400f;

    public static readonly Type[] MessageTypes = { typeof(HitBoltEvent), typeof(CollisionMessage), typeof(EffectBoltEvent) };
}
