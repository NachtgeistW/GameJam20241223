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

        [SerializeField] private GameObject startPanel;
        [SerializeField] private GameObject tutorialPanel;
        [SerializeField] private Button tutorialButton;

        private void Start()
        {
            startButton.onClick.AddListener(() =>
                {
                    fx.Play();
                    startPanel.SetActive(false);
                    tutorialPanel.SetActive(true);
                }
            );

            exitButton.onClick.AddListener(Application.Quit);

            tutorialButton.onClick.AddListener(() =>
                {
                    EventCenter.Broadcast(new TransitionEvent
                    {
                        SceneName = "Game",
                        IsFadeEnable = true
                    });
                }
            );
        }
    }
}