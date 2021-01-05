using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bolt;
using System;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Using Unity Messages")]
public class GolfBallDataMediator : DataMediator<IGolfBallState>
{
    [SerializeField] private GameObject cooldownIndicatorPrefab;
    [SerializeField] private GameObject scoreIndicatorPrefab;
    [SerializeField] private float scoreIndicatorDistance;
    [SerializeField] private float scoreIndicatorRotationSpeed;
    [SerializeField] private GameObject kingIndicator;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Image cooldownIndicator;
    private List<GameObject> scoreIndicators = new List<GameObject>();

    public override void Attached()
    {
        base.Attached();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cooldownIndicator = Instantiate(cooldownIndicatorPrefab, transform)
            .GetComponentInChildren<Image>();

        if (BoltNetwork.IsServer) GameEventManager.Subscribe<HitBoltEvent>(OnHit);
    }

    private void OnHit(object message)
    {
        var hit = message as HitBoltEvent;
        if (state.ReadyToMove && hit.Id == entity.NetworkId)
            rb.AddForce(hit.Force * GlobalSettings.ForceScale);
    }

    private void Update()
    {
        state.Velocity = rb != null ? rb.velocity.magnitude : 0;
        AnimateScoreIndicators();
    }

    private void AnimateScoreIndicators()
    {
        for (int i = 0; i < scoreIndicators.Count; i++)
        {
            float offset = (scoreIndicatorRotationSpeed * Time.time) / scoreIndicators.Count;
            float z = (360f / scoreIndicators.Count) * i + offset;
            scoreIndicators[i].transform.localPosition = Quaternion.Euler(0, 0, z)
                * Vector2.up * scoreIndicatorDistance;
        }
    }

    private void OnColorChanged()
    {
        spriteRenderer.color = state.Color;
        cooldownIndicator.color = state.Color;
    }

    private void OnCooldownRatioChanged() 
        => cooldownIndicator.fillAmount = 1 - state.CooldownRatio;

    private void OnScoreChanged()
    {
        while (scoreIndicators.Count < state.Score)
        {
            scoreIndicators.Add(Instantiate(scoreIndicatorPrefab, transform));
        }

        while (scoreIndicators.Count > state.Score)
        {
            Destroy(scoreIndicators[0]);
            scoreIndicators.RemoveAt(0);
        }
    }
    private void OnIsKingChanged()
    {
        kingIndicator.SetActive(state.IsKing);  
    }
}
