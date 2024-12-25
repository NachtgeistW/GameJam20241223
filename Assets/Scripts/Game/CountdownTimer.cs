using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class CountdownTimer : MonoBehaviour
    {
        [SerializeField] private Text countdownText;
        
        private bool takingAway = false;
        [SerializeField] private float secondsLeft = 30f;

        private void Start()
        {
            countdownText.text = $"TIME: {secondsLeft}s";
        }

        private void Update()
        {
            if (!takingAway && secondsLeft > 0f)
            {
                StartCoroutine(TimerTake());
            }
        }

        private IEnumerator TimerTake()
        {
            takingAway = true;
            yield return new WaitForSeconds(1);
            secondsLeft -= 1;
            countdownText.text = $"TIME: {secondsLeft}s";
            takingAway = false;
        }
    }
}