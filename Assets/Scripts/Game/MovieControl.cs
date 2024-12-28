using System;
using System.Collections;
using Plutono.Util;
using Transition;
using UnityEngine;
using UnityEngine.Video;

namespace Game
{
    public class MovieControl : MonoBehaviour
    {
        [SerializeField] private GameObject movie;
        [SerializeField] private VideoPlayer videoPlayer;

        private void OnEnable()
        {
            EventCenter.AddListener<GameFailEvent>(OnGameFail);
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener<GameFailEvent>(OnGameFail);
        }

        private void OnGameFail(GameFailEvent evt)
        {
            movie.SetActive(true);
            StartCoroutine(PlayMovie());
        }

        private IEnumerator PlayMovie()
        {
            videoPlayer.Play();
            while (true)
            {
                if (videoPlayer.frame >= (long)(videoPlayer.frameCount - 1))
                {
                    EventCenter.Broadcast(new TransitionEvent
                    {
                        IsFadeEnable = true,
                        SceneName = "Result"
                    });
                    break;
                }

                yield return null;
            }
        }
    }
}