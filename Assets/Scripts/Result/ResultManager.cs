using System;
using Plutono.Util;
using Transition;
using UnityEngine;
using UnityEngine.UI;

namespace Result
{
    public class ResultManager : MonoBehaviour
    {
        [SerializeField] private Text resultText;
        [SerializeField] private Text finalScoreText;
        
        [SerializeField] private Button retryButton;

        private void Start()
        {
            if (Game.Game.Instance.gameStatus == 2)
            {
                resultText.text = "你切到人了！！！";
                finalScoreText.text = "";
            }

            if (Game.Game.Instance.gameStatus == 1)
            {
                resultText.text = "游戏结束！";
                finalScoreText.text = Game.Game.Instance.score.ToString();
            }
            
            retryButton.onClick.AddListener(() =>
                {
                    EventCenter.Broadcast(new TransitionEvent
                    {
                        SceneName = "Game",
                        IsFadeEnable = true
                    });
                    Game.Game.Instance.gameStatus = 0;
                }
            );
        }
    }
}