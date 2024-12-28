using System;
using Plutono.Util;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game
{
    public class ScoreCalculator : MonoBehaviour
    {
        [SerializeField] private Text scoreText;
        private int totalScore;
        [SerializeField] private int plusScoreEach = 10;

        [SerializeField] private float minusCutHeight = 0.4f;

        private void OnEnable()
        {
            EventCenter.AddListener<HairCutEvent>(OnHairCut);
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener<HairCutEvent>(OnHairCut);
        }

        private void OnHairCut(HairCutEvent evt)
        {
            if (evt.CutHeight > minusCutHeight) return;
            
            CalculateScore();
            SetScoreText();
            
            EventCenter.Broadcast(new GetScoreEvent
            {
                EndCutPoint = evt.EndCutPoint,
            });
        }

        private void CalculateScore()
        {
            totalScore += plusScoreEach;
            Game.Instance.score = totalScore;
        }

        private void SetScoreText()
        {
            scoreText.text = $"Score: {totalScore}";
        }
    }
}