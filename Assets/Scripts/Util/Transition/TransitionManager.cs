using System;
using System.Collections;
using System.Collections.Generic;
using Plutono.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Util.Attribute;

namespace Transition
{
    public struct TransitionEvent : IEvent
    {
        public string SceneName;

        public bool IsFadeEnable;
        //public Vector3 Pos;
    }

    public struct FadeEvent : IEvent
    {
        public float Alpha;
    }

    public class TransitionManager : MonoBehaviour
    {
#if UNITY_EDITOR
        [SceneName] public string startSceneName = string.Empty;
#else
        public string startSceneName = "Start";
#endif
        [SerializeField] private CanvasGroup fadeCanvasGroup;

        private bool isFade;

        private void OnEnable()
        {
            EventCenter.AddListener<TransitionEvent>(OnTransitionEvent);
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener<TransitionEvent>(OnTransitionEvent);
        }

        private IEnumerator Start()
        {
            fadeCanvasGroup = GameObject.Find("Fade Canvas/Fade Panel").GetComponent<CanvasGroup>();
            yield return LoadSceneAndActivate(startSceneName);
            EventCenter.Broadcast(new AfterSceneLoadedEvent());
        }

        private void OnTransitionEvent(TransitionEvent evt)
        {
            if (evt.IsFadeEnable)
            {
                if (!isFade)
                    StartCoroutine(TransitionWithFade(evt.SceneName));
            }
            else
            {
                StartCoroutine(Transition(evt.SceneName));
            }
        }

        /// <summary>
        /// Load scene and set it as activate
        /// </summary>
        /// <param name="sceneName">scene to be loaded</param>
        /// <returns></returns>
        private IEnumerator LoadSceneAndActivate(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            var newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
            SceneManager.SetActiveScene(newScene);
        }

        /// <summary>
        /// Unload the current scene and switch to another scene
        /// </summary>
        /// <param name="sceneName">scene to be loaded</param>
        /// <returns></returns>
        private IEnumerator Transition(string sceneName)
        {
            //EventCenter.Broadcast(new BeforeSceneLoadedEvent());
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return LoadSceneAndActivate(sceneName);
            //EventHandler.CallMoveToPositionEvent(targetPos);
            //EventCenter.Broadcast(new AfterSceneLoadedEvent());
        }

        private IEnumerator TransitionWithFade(string sceneName)
        {
            //EventCenter.Broadcast(new BeforeSceneLoadedEvent());
            yield return Fade(1);
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());

            yield return LoadSceneAndActivate(sceneName);
            //EventHandler.CallMoveToPositionEvent(targetPos);
            //EventCenter.Broadcast(new AfterSceneLoadedEvent());
            yield return Fade(0);
        }

        /// <summary>
        /// Scene fade in or fade out
        /// </summary>
        /// <param name="targetAlpha">1 stands for fully show, 0 for hidden</param>
        /// <returns></returns>
        private IEnumerator Fade(float targetAlpha)
        {
            isFade = true;
            fadeCanvasGroup.blocksRaycasts = true;

            //TODO: replace with DOTween
            var speed = Mathf.Abs(fadeCanvasGroup.alpha - targetAlpha) / Settings.SceneFadeDuration;
            while (!Mathf.Approximately(fadeCanvasGroup.alpha, targetAlpha))
            {
                fadeCanvasGroup.alpha = Mathf.MoveTowards(fadeCanvasGroup.alpha, targetAlpha, speed * Time.deltaTime);
                yield return null;
            }

            fadeCanvasGroup.blocksRaycasts = false;
            isFade = false;
        }
    }
}