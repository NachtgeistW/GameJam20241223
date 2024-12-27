using System;
using Plutono.Util;
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
            
            upCutHairTransform.gameObject.GetComponent<Rigidbody2D>()
                .AddForce(new Vector2(Random.Range(-100f, 100f), Random.Range(300f, 500f)));
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