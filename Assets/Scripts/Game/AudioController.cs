using Plutono.Util;
using UnityEngine;

namespace Game
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource bgm;
        private void OnEnable()
        {
            EventCenter.AddListener<GameFailEvent>(OnGameFail);
        }

        private void OnGameFail(GameFailEvent obj)
        {
            bgm.Stop();
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener<GameFailEvent>(OnGameFail);
        }
    }
}