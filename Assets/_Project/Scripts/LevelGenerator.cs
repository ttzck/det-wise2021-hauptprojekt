using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private const int UpperBound = 100000;
    [SerializeField] private List<BuildingBlock> buildingBlocks;
    [SerializeField] private int numberOfCollectablePoints;
    [SerializeField] private int numberOfControlZones;

    [SerializeField] private GameObject Collectable_Spot;
    [SerializeField] private GameObject ControlZone;

    [ContextMenu("Generator")]
    public void Generate()
    {
        Generate(Random.Range(int.MinValue, int.MaxValue));
    }

    public void Generate(int seed)
    {
        GameObject level = new GameObject("level");
        HashSet<Vector2> freeGrids = new HashSet<Vector2>();

        Random.InitState(seed);

        for (int x = 0; x < GlobalSettings.ArenaDimensions.x; x++)
        {
            for (int y = 0; y < GlobalSettings.ArenaDimensions.y; y++)
            {
                freeGrids.Add(new Vector2(x, y));
            }
        }

        //-------------------------------------------------------------------

        for (int j = 0; freeGrids.Count > 0 && j < UpperBound; j++)
        {
            var freeGrid = RandomElementFrom(freeGrids);
            var buildingBlock = RandomElementFrom(buildingBlocks);
            var rotation = Random.Range(0, 4);
            var toMove = buildingBlock.gridCells.ToList();

            if (buildingBlock.probabilityWeight < Random.value) continue;

            Rotate(ref toMove, rotation);
            Translate(ref toMove, freeGrid);

            var hashSet = new HashSet<Vector2>(toMove);

            if (hashSet.IsSubsetOf(freeGrids))
            {
                freeGrids.ExceptWith(hashSet);
                var instance = Instantiate(buildingBlock,
                    ToArenaCoords(freeGrid),
                    Quaternion.Euler(0, 0, -90 * rotation));
                instance.transform.parent = level.transform;
                var buidlingBlockInstance = instance.GetComponent<BuildingBlock>();

                Rotate(ref buidlingBlockInstance.gridCells, rotation);
                Rotate(ref buidlingBlockInstance.collectableSpawnPoints, rotation);
                Rotate(ref buidlingBlockInstance.controlZoneSpawnPoints, rotation);
                Translate(ref buidlingBlockInstance.gridCells, freeGrid);
                Translate(ref buidlingBlockInstance.collectableSpawnPoints, freeGrid);
                Translate(ref buidlingBlockInstance.controlZoneSpawnPoints, freeGrid);
            }
        }

        if (BoltNetwork.IsServer)
        {
            var collectableSpawnPoints = FindObjectsOfType<BuildingBlock>().SelectMany(i => i.collectableSpawnPoints).ToList();
            for (int i = 0; i < numberOfCollectablePoints && collectableSpawnPoints.Any(); i++)
            {
                var randPos = RandomElementFrom(collectableSpawnPoints);
                BoltNetwork.Instantiate(Collectable_Spot, ToArenaCoords(randPos), Quaternion.identity);
                collectableSpawnPoints.Remove(randPos);
            }

            var controlZoneSpawnPoints = FindObjectsOfType<BuildingBlock>().SelectMany(i => i.controlZoneSpawnPoints).ToList();
            for (int i = 0; i < numberOfControlZones && controlZoneSpawnPoints.Any(); i++)
            {
                var randPos = RandomElementFrom(controlZoneSpawnPoints);
                BoltNetwork.Instantiate(ControlZone, ToArenaCoords(randPos), Quaternion.identity);
                controlZoneSpawnPoints.Remove(randPos);
            }
        }
    }

    private T RandomElementFrom<T>(IEnumerable<T> ts)
    {
        int index = Random.Range(0, ts.Count());
        return ts.ElementAt(index);
    }

    private void Rotate(ref List<Vector2> list, int rotation)
    {
        for (int i = 0; i < rotation; i++)
        {
            list = list.Select(j => new Vector2(j.y, -j.x)).ToList();
        }
    }

    private void Translate(ref List<Vector2> list, Vector2 translation)
    {
        list = list.Select(i => translation + i).ToList();
    }

    public static Vector2 ToArenaCoords(Vector2 worldCoords)
        => worldCoords - 0.5f * GlobalSettings.ArenaDimensions + Vector2.one * 0.5f;
}

