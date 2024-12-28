using System;
using System.Collections;
using Plutono.Util;
using Transition;
using UnityEngine;
using UnityEngine.UI;

namespace Start
{
    public class StartManager : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private AudioSource fx;

        private void Start()
        {
            startButton.onClick.AddListener(() =>
                {
                    fx.Play();
                    EventCenter.Broadcast(new TransitionEvent
                    {
                        SceneName = "Game",
                        IsFadeEnable = true
                    });
                }
            );

            exitButton.onClick.AddListener(Application.Quit);
        }
    }
}