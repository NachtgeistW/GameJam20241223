using Plutono.Util;
using UnityEngine;

namespace Game
{
    public class FxController : MonoBehaviour
    {
        [SerializeField] private AudioSource fx;

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
            PlayFx();
        }

        private void PlayFx()
        {
            fx.Play();
        }
    }
}