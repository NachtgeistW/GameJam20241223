using System;
using Plutono.Util;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game
{
    public class OnHairCutEvent : MonoBehaviour
    {
        [SerializeField] private Transform studentTransform;

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
            SplitHair(evt.cutPosition, evt.hair);
        }
        
        private void SplitHair(Vector2 cutPoint, GameObject hairObject)
        {
            // Get the original sprite
            var originalSprite = hairObject.GetComponent<SpriteRenderer>();
            var originalTexture = originalSprite.sprite.texture;

            // Create two new sprites
            var topHalf = new Texture2D(originalTexture.width, originalTexture.height / 2);
            var bottomHalf = new Texture2D(originalTexture.width, originalTexture.height / 2);

            // Copy pixels to new textures
            for (var x = 0; x < originalTexture.width; x++)
            {
                for (var y = 0; y < originalTexture.height; y++)
                {
                    var pixel = originalTexture.GetPixel(x, y);
                    if (y < originalTexture.height / 2)
                    {
                        bottomHalf.SetPixel(x, y, pixel);
                    }
                    else
                    {
                        topHalf.SetPixel(x, y - originalTexture.height / 2, pixel);
                    }
                }
            }

            // Apply changes
            topHalf.Apply();
            bottomHalf.Apply();

            // Create new GameObjects with split sprites
            CreateSplitPiece(topHalf, true);
            CreateSplitPiece(bottomHalf, false);

            // Destroy original hair
            //Destroy(gameObject);
            hairObject.SetActive(false);
        }

        private void CreateSplitPiece(Texture2D texture, bool isTop)
        {
            var newPiece = new GameObject("HairPiece");
            var newPieceRenderer = newPiece.AddComponent<SpriteRenderer>();
            newPiece.transform.SetParent(studentTransform);
    
            // Create new sprite from texture
            var newSprite = Sprite.Create(texture, 
                new Rect(0, 0, texture.width, texture.height), 
                new Vector2(0.5f, 0.5f));
    
            newPieceRenderer.sprite = newSprite;
            newPieceRenderer.sortingLayerName = "Hair";
    
            // Position the new piece
            newPiece.transform.position = transform.position + (isTop ? Vector3.up : Vector3.down) * texture.height / 2;
    
            if (isTop)
            {
                // Add some physics effects
                var rb = newPiece.AddComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(Random.Range(-100f, 100f), Random.Range(-50f, 50f)));
            }
        }
    }
}