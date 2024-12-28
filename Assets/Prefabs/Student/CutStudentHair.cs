using System;
using Plutono.Util;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game
{
    public class CutStudentHair : MonoBehaviour
    {
        [SerializeField] private Transform studentTransform;
        [SerializeField] private GameObject originalHair;
        //[SerializeField] private GameObject originalMiddleHair;

        [SerializeField] private GameObject cutHair;
        [SerializeField] private Transform upCutHairTransform;
        [SerializeField] private Transform downCutHairTransform;

        [SerializeField] private SpriteRenderer topSprite;
        [SerializeField] private SpriteRenderer middleBottomSprite;
        [SerializeField] private SpriteRenderer topMiddleSprite;
        [SerializeField] private SpriteRenderer bottomSprite;

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
            var middle = originalHair.transform.GetChild(1).gameObject;
            if (middle.Equals(evt.hair.gameObject))
                SplitHair(evt.hair, evt.CutRatio);
        }

        private void SplitHair(GameObject hairObject, float cutRatio)
        {
            originalHair.SetActive(false);
            cutHair.SetActive(true);
            
            AdjustSplitPiece(hairObject, cutRatio);
            
            upCutHairTransform.gameObject.GetComponent<Rigidbody2D>()
                .AddForce(new Vector2(Random.Range(-100f, 100f), Random.Range(300f, 500f)));
        }

        private void AdjustSplitPiece(GameObject hairObject, float ratio)
        {
            var scale = hairObject.transform.localScale.y;
            
            AdjustTop(scale * (1 - ratio));
            AdjustBottom(scale * ratio);
            return;

            void AdjustTop(float scale)
            {
                var middleScale = middleBottomSprite.gameObject.transform.localScale;
                middleScale.y = scale;
                middleBottomSprite.gameObject.transform.localScale = middleScale;

                var topHeight = topSprite.bounds.size.y;
                var middleHeight = middleBottomSprite.bounds.size.y;

                var topBottomEdge = topSprite.transform.position.y - topHeight / 2;
                var newMiddlePos = topBottomEdge - middleHeight / 2;
                middleBottomSprite.transform.position = new Vector3(middleBottomSprite.transform.position.x,
                    newMiddlePos,
                    middleBottomSprite.transform.position.z);

                var pos = upCutHairTransform.position;
                pos.y = pos.y + topHeight + middleHeight;
                upCutHairTransform.position = pos;
            }

            void AdjustBottom(float scale)
            {
                var middleScale = topMiddleSprite.gameObject.transform.localScale;
                middleScale.y = scale;
                topMiddleSprite.gameObject.transform.localScale = middleScale;
                
                var bottomHeight = bottomSprite.bounds.size.y;
                var middleHeight = topMiddleSprite.bounds.size.y;
                
                var bottomTopEdge = bottomSprite.transform.position.y + bottomHeight / 2;
                var newMiddlePos = bottomTopEdge + middleHeight / 2;
                topMiddleSprite.transform.position = new Vector3(topMiddleSprite.transform.position.x,
                    newMiddlePos,
                    topMiddleSprite.transform.position.z);
            }
        }
    }
}