using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bolt;

public class ScoreIndicator : EntityBehaviour<IGolfBallState>
{
    [SerializeField] private GameObject scoreIndicatorPrefab;
    [SerializeField] private float indicatorDistance;
    [SerializeField] private float rotationSpeed;

    private List<GameObject> indicators = new List<GameObject>();

    private void Update()
    {
        while (indicators.Count < state.Score)
        {
            indicators.Add(Instantiate(scoreIndicatorPrefab, transform));
        }

        while (indicators.Count > state.Score)
        {
            Destroy(indicators[0]);
            indicators.RemoveAt(0);
        }

        for (int i = 0; i < indicators.Count; i++)
        {
            float offset = (rotationSpeed * Time.time) / indicators.Count;
            float z = (360f / indicators.Count) * i + offset;
            indicators[i].transform.localPosition = Quaternion.Euler(0, 0, z) 
                * Vector2.up * indicatorDistance;
        }
    }
}
