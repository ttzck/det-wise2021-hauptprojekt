using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LeverGenerator : MonoBehaviour
{
    [SerializeField] private List<BuildingBlock> buildingBlocks;
    [ContextMenu("Generator")]

    public void Genertor()
    {
        GameObject level = new GameObject("level");
        HashSet<Vector2> freeGrids = new HashSet<Vector2>();
        for (int x = 0; x < GlobalSettings.ArenaDimensions.x; x++)
        {
            for (int y = 0; y < GlobalSettings.ArenaDimensions.y; y++)
            {
                freeGrids.Add(new Vector2(x, y));
            }
        }

        for (int j = 0; freeGrids.Count > 0 && j < 1000; j++)
        {
            var freeGrid = RandomElementFrom(freeGrids);
            var buildingBlock = RandomElementFrom(buildingBlocks);
            var rotation = Random.Range(0, 4);
            var toMove =  buildingBlock.gridCells;
         
            for(int i =0; i< rotation; i ++)
            {
                Rotate(ref toMove);
            }
            var hashSet = new HashSet<Vector2>(toMove.Select(i => freeGrid + i));

            if (hashSet.IsSubsetOf(freeGrids))
            {
                freeGrids.ExceptWith(hashSet);
                var instance = Instantiate(buildingBlock,
                    freeGrid - 0.5f * GlobalSettings.ArenaDimensions + Vector2.one * 0.5f,
                    Quaternion.Euler(0 , 0 , -90 * rotation));
                instance.transform.parent = level.transform;

            }
        }
    }

    private T RandomElementFrom<T>(IEnumerable<T> ts)
    {
            int index = Random.Range(0, ts.Count());
            return ts.ElementAt(index);
    }

    private void Rotate( ref List<Vector2> list)
    {
        list = list.Select(i => new Vector2(i.y, -i.x)).ToList();
    }

 }

