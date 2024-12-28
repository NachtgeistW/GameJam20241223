using System.Collections.Generic;
using System.Linq;
using Plutono.Util;
using Transition;
using UnityEngine;

namespace Game
{
    public class HairCutter : MonoBehaviour
    {
        private bool isMousePressed = false;
        private Vector2 startCutPoint;
        private Vector2 endCutPoint;

        [SerializeField] private Camera cam;

        [SerializeField] private LineRenderer cutLine;
        private List<Vector2> linePoints = new();
        
        private void Start()
        {
            cam = FindObjectOfType<Camera>();
            
            SetCutLine();
            return;

            void SetCutLine()
            {
                cutLine.startWidth = 0.2f;
                cutLine.endWidth = 0.2f;
                cutLine.positionCount = 2;
    
                // Optional: Set line material and color
                var lineTexture = Resources.Load<Texture2D>("CutLight");
                var lineMaterial = new Material(Shader.Find("Sprites/Default"))
                {
                    mainTexture = lineTexture
                };

                // Apply material to line renderer
                cutLine.material = lineMaterial;
                cutLine.startColor = Color.white;
                cutLine.endColor = Color.white;
            }
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

            DrawLine();
        }

        private void CheckCutting()
        {
            var cutDirection = endCutPoint - startCutPoint;
            //Debug.Log($"{startCutPoint}, {endCutPoint}, {cutDirection}");

            //TODO: what did this raycast do??
            var hit = Physics2D.Raycast(startCutPoint, cutDirection.normalized, cutDirection.magnitude);
            if (hit.collider)
            {
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Hair"))
                {
                    var ratio = CalculateCutRatio(hit.point, out var cutHeight);
                    EventCenter.Broadcast(new HairCutEvent
                    {
                        hair = hit.collider.gameObject,
                        CutHeight = cutHeight,
                        CutRatio = ratio
                    });
                }

                if (hit.collider.CompareTag("Body"))
                {
                    Game.Instance.gameStatus = 2;
                    EventCenter.Broadcast(new TransitionEvent
                    {
                        IsFadeEnable = false,
                        SceneName = "Result"
                    });
                }
                
                float CalculateCutRatio(Vector2 cutPoint, out float cutHeight)
                {
                    // 获取原始sprite的边界
                    SpriteRenderer spriteRenderer = hit.collider.GetComponent<SpriteRenderer>();
                    Bounds spriteBounds = spriteRenderer.bounds;
    
                    // 计算切割点在sprite高度上的相对位置
                    float totalHeight = spriteBounds.size.y;
                    cutHeight = cutPoint.y - spriteBounds.min.y;
    
                    // 返回切割比例（0-1之间）
                    var ratio = cutHeight / totalHeight;
    
                    return ratio;
                }
            }
        }

        private void DrawLine()
        {
            if (isMousePressed)
            {
                Vector2 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
                cutLine.SetPosition(0, startCutPoint);
                cutLine.SetPosition(1, currentPoint);
            }
            else
            {
                cutLine.SetPosition(0, Vector2.zero);
                cutLine.SetPosition(1, Vector2.zero);
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