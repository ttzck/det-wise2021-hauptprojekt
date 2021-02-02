using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingBlock : MonoBehaviour
{
    public List<Vector2> gridCells = new List<Vector2>();
    public List<Vector2> collectableSpawnPoints = new List<Vector2>();
    public List<Vector2> controlZoneSpawnPoints = new List<Vector2>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, .2f);
        foreach (var cell in gridCells)
        {
            Gizmos.DrawCube(transform.position + (Vector3)cell, Vector3.one);
        }

        var points = collectableSpawnPoints.Union(controlZoneSpawnPoints);
        Gizmos.color = Color.white;
        foreach (var point in points)
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)point, .2f);
        }
    }
}
