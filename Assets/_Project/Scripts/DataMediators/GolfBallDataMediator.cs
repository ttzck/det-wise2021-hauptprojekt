using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Bolt;
using System;

[System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Using Unity Messages")]
public class GolfBallDataMediator : DataMediator<IGolfBallState>
{
    [Header("Cooldown Indicator")]
    [SerializeField] private GameObject cooldownIndicatorPrefab;

    [Header("Score Indicator")]
    [SerializeField] private GameObject scoreIndicatorPrefab;
    [SerializeField] private float scoreIndicatorDistance;
    [SerializeField] private float scoreIndicatorRotationSpeed;

    [Header("PreMove Indicator")]
    [SerializeField] private LineRenderer preMoveIndicatorPrefab;
    [SerializeField] private float maxPreMoveIndicatorLength;

    [Header("King Indicator")]
    [SerializeField] private GameObject kingIndicator;


    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Image cooldownIndicator;
    private List<GameObject> scoreIndicators = new List<GameObject>();
    private LineRenderer preMoveIndicator;

    public override void Attached()
    {
        base.Attached();

        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cooldownIndicator = Instantiate(cooldownIndicatorPrefab, transform)
            .GetComponentInChildren<Image>();
        preMoveIndicator = Instantiate(preMoveIndicatorPrefab);
        preMoveIndicator.enabled = false;

        if (BoltNetwork.IsServer) 
            GameEventManager.Subscribe<HitBoltEvent>(m => OnHit(m as HitBoltEvent));
    }

    private void OnHit(HitBoltEvent hit)
    {
        if (hit.Id == entity.NetworkId)
            if (state.ReadyToMove)
                rb.AddForce(hit.Force * GlobalSettings.ForceScale);
            else
                state.PreMove = hit.Force;
    }

    private void Update()
    {
        if (BoltNetwork.IsServer && state.ReadyToMove && state.PreMove != Vector3.zero)
        {
            rb.AddForce(state.PreMove * GlobalSettings.ForceScale);
            state.PreMove = Vector3.zero;
        }

        state.Velocity = rb != null ? rb.velocity.magnitude : 0;
        UpdateScoreIndicators();
        UpdatePreMoveIndicator();
    }

    private void UpdateScoreIndicators()
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

    private void UpdatePreMoveIndicator()
    {
        if (!entity.HasControl) return;

        preMoveIndicator.enabled = state.PreMove != Vector3.zero;

        preMoveIndicator.SetPositions(new Vector3[] 
        { 
            transform.position, 
            transform.position + state.PreMove * maxPreMoveIndicatorLength
        });
    }
}
