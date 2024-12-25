using System;
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
        
        private void Start()
        {
            startButton.onClick.AddListener(() =>
                EventCenter.Broadcast(new TransitionEvent
                {
                    SceneName = "Game",
                    IsFadeEnable = true
                })
            );
            
            exitButton.onClick.AddListener(Application.Quit);
        }
    }
}