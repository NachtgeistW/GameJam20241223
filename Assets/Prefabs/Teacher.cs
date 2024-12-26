using Game;
using Plutono.Util;
using UnityEngine;
using UnityEngine.Serialization;

namespace Prefabs
{
    public class Teacher : MonoBehaviour
    {
        [SerializeField] private Animator anim;
        private static readonly int IsCutting = Animator.StringToHash("IsCutting");

        private void OnEnable()
        {
            EventCenter.AddListener<HairCutEvent>(OnHairCut);
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener<HairCutEvent>(OnHairCut);
        }

        private void OnHairCut(HairCutEvent obj)
        {
            anim.SetBool(IsCutting, true);
        }
        
        public void ResetCutting()
        {
            anim.SetBool(IsCutting, false);
        }
    }
}