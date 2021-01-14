using UnityEngine;

public static class GameUtils
{
    private const int RandomPositionCheckRadius = 1;

    public static Vector2 RandomPosition
    {
        get
        {
            while (true)
            {
                var randomPositon = new Vector2(Random.value, Random.value)
                    * GlobalSettings.ArenaDimensions
                    - (GlobalSettings.ArenaDimensions * .5f);

                if (Physics2D.OverlapCircle(randomPositon, RandomPositionCheckRadius) == null)
                {
                    return randomPositon;
                }
            }
        }
    }

    public static Color GetTeamColor(int teamId, int numberOfTeams)
        => Color.HSVToRGB((1f / numberOfTeams) * teamId, 1, 1);
}
