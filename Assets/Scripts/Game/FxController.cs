using Plutono.Util;
using UnityEngine;

namespace Game
{
    public class FxController : MonoBehaviour
    {
        [SerializeField] private AudioSource fx;

        [SerializeField] private Animator flashAnim;
        private static readonly int PlayFlash = Animator.StringToHash("PlayFlash");

        private void OnEnable()
        {
            EventCenter.AddListener<HairCutEvent>(OnHairCut);
            EventCenter.AddListener<GetScoreEvent>(OnGetScore);
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener<HairCutEvent>(OnHairCut);
            EventCenter.RemoveListener<GetScoreEvent>(OnGetScore);
        }

        private void OnHairCut(HairCutEvent evt)
        {
            PlayFx();
        }

        private void PlayFx()
        {
            fx.Play();
        }

        private void OnGetScore(GetScoreEvent evt)
        {
            var pos = evt.EndCutPoint;
            flashAnim.gameObject.transform.position =
                new Vector3(pos.x, pos.y, flashAnim.gameObject.transform.position.z);
            
            flashAnim.SetBool(PlayFlash, true);
        }

        public void ResetFlashing()
        {
            flashAnim.SetBool(PlayFlash, false);
        }
    }
}