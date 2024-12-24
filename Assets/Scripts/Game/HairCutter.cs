using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Game
{
    public class HairCutter : MonoBehaviour
    {
        private bool isMousePressed = false;
        private Vector2 startCutPoint;
        private Vector2 endCutPoint;

        [SerializeField] private Camera cam;

        private void Start()
        {
            cam = FindObjectOfType<Camera>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                isMousePressed = true;

                var mousePos = Input.mousePosition;
                mousePos.z = Mathf.Abs(cam.transform.position.z);
                startCutPoint = cam.ScreenToWorldPoint(mousePos);
            }

            if (Input.GetMouseButtonUp(0))
            {
                isMousePressed = false;
                
                var mousePos = Input.mousePosition;
                mousePos.z = Mathf.Abs(cam.transform.position.z);
                endCutPoint = cam.ScreenToWorldPoint(mousePos);
                CheckCutting();
            }
            
            DrawLine4Debug();
        }

        private void CheckCutting()
        {
            var cutDirection = endCutPoint - startCutPoint;
            //Debug.Log($"{startCutPoint}, {endCutPoint}, {cutDirection}");
            
            //TODO: what did this raycast do??
            var hit = Physics2D.Raycast(startCutPoint, cutDirection.normalized, cutDirection.magnitude);
            if (hit.collider)
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Hair"))
                {
                    SplitHair(hit.point, hit.collider.gameObject);
                }
                if (hit.collider.CompareTag("Body"))
                {
                    Debug.Log("You cut the body!!!");
                }
            }
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
    
            // Create new sprite from texture
            var newSprite = Sprite.Create(texture, 
                new Rect(0, 0, texture.width, texture.height), 
                new Vector2(0.5f, 0.5f));
    
            newPieceRenderer.sprite = newSprite;
    
            // Position the new piece
            newPiece.transform.position = transform.position + (isTop ? Vector3.up : Vector3.down) * texture.height / 2;
    
            if (isTop)
            {
                // Add some physics effects
                var rb = newPiece.AddComponent<Rigidbody2D>();
                rb.AddForce(new Vector2(Random.Range(-100f, 100f), Random.Range(-50f, 50f)));
            }
        }
        
        private void DrawLine4Debug()
        {
            if (isMousePressed)
            {
                var mousePos = Input.mousePosition;
                mousePos.z = Mathf.Abs(cam.transform.position.z);
                var worldPos = cam.ScreenToWorldPoint(mousePos);
                Debug.DrawLine(startCutPoint, worldPos, Color.red);
            }
        }
    }
}