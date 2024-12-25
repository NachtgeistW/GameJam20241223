using System;
using UnityEngine;
using UnityEngine.UI;

namespace Result
{
    public class ResultManager : MonoBehaviour
    {
        [SerializeField] private Text resultText;
        [SerializeField] private Text finalScoreText;

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
        }
    }
}