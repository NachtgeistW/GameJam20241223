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
                SplitHair(evt.cutPosition, evt.hair);
        }

        private void SplitHair(Vector2 cutPoint, GameObject hairObject)
        {
            originalHair.SetActive(false);
            cutHair.SetActive(true);

            var scale = hairObject.transform.localScale.y;
            AdjustSplitPiece(scale / 2);
            
            upCutHairTransform.gameObject.GetComponent<Rigidbody2D>()
                .AddForce(new Vector2(Random.Range(-100f, 100f), Random.Range(300f, 500f)));
        }

        private void AdjustSplitPiece(float scale)
        {
            AdjustTop(scale);
            AdjustBottom(scale);
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

        // private void SplitHair(Vector2 cutPoint, GameObject hairObject)
        // {
        //     // Get the original sprite
        //     var originalSprite = hairObject.GetComponent<SpriteRenderer>();
        //     var originalTexture = originalSprite.sprite.texture;
        //
        //     // Create two new sprites
        //     var topHalf = new Texture2D(originalTexture.width, originalTexture.height / 2);
        //     var bottomHalf = new Texture2D(originalTexture.width, originalTexture.height / 2);
        //
        //     // Copy pixels to new textures
        //     CopyPixelsToNewTexture();
        //
        //     // Apply changes
        //     topHalf.Apply();
        //     bottomHalf.Apply();
        //
        //     // Create new GameObjects with split sprites
        //     CreateSplitPiece(topHalf, true);
        //     CreateSplitPiece(bottomHalf, false);
        //
        //     // Destroy original hair
        //     //Destroy(gameObject);
        //     hairObject.SetActive(false);
        //     return;
        //
        //     void CopyPixelsToNewTexture()
        //     {
        //         for (var x = 0; x < originalTexture.width; x++)
        //         {
        //             for (var y = 0; y < originalTexture.height; y++)
        //             {
        //                 var pixel = originalTexture.GetPixel(x, y);
        //                 if (y < originalTexture.height / 2)
        //                 {
        //                     bottomHalf.SetPixel(x, y, pixel);
        //                 }
        //                 else
        //                 {
        //                     topHalf.SetPixel(x, y - originalTexture.height / 2, pixel);
        //                 }
        //             }
        //         }
        //     }
        // }
        //
        // private void CreateSplitPiece(Texture2D texture, bool isTop)
        // {
        //     var newPiece = new GameObject("HairPiece");
        //     var newPieceRenderer = newPiece.AddComponent<SpriteRenderer>();
        //     newPiece.transform.SetParent(studentTransform);
        //
        //     // Get the original sprite's properties
        //     SpriteRenderer originalRenderer = originalHairTransform.GetComponent<SpriteRenderer>();
        //     Vector2 originalPivot = originalRenderer.sprite.pivot;
        //     float pixelsPerUnit = originalRenderer.sprite.pixelsPerUnit;
        //
        //     // Create new sprite from texture
        //     var newSprite = Sprite.Create(
        //         texture,
        //         new Rect(0, 0, texture.width, texture.height),
        //         new Vector2(originalPivot.x / texture.width,
        //             originalPivot.y / texture.height), // Use original pivot ratio
        //         pixelsPerUnit
        //     );
        //
        //     newPieceRenderer.sprite = newSprite;
        //
        //     // Match the original sprite's properties
        //     newPieceRenderer.sortingOrder = originalRenderer.sortingOrder;
        //     newPieceRenderer.sortingLayerID = originalRenderer.sortingLayerID;
        //
        //     newPiece.transform.rotation = transform.rotation;
        //     newPiece.transform.localScale = transform.localScale;
        //
        //     if (isTop)
        //     {
        //         newPiece.transform.position = new Vector3(transform.position.x + 1,
        //             transform.position.y + 3.3f,
        //             transform.position.z);
        //         // Add some physics effects
        //         var rb = newPiece.AddComponent<Rigidbody2D>();
        //         rb.AddForce(new Vector2(Random.Range(-100f, 100f), Random.Range(300f, 500f)));
        //         //TODO: 给它加一个旋转的力
        //         //rb.AddTorque(Random.Range(-100f, 100f), ForceMode2D.Impulse);
        //     }
        //     else
        //     {
        //         newPiece.transform.position =
        //             new Vector3(transform.position.x + 1, transform.position.y + 1.65f, transform.position.z);
        //     }
        // }
    }
}