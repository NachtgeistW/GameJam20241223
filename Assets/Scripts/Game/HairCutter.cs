using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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
            
            var hit = Physics2D.Raycast(startCutPoint, cutDirection.normalized, cutDirection.magnitude);
            if (hit.collider)
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.CompareTag("Body"))
                {
                    Debug.Log("You cut the body!!!");
                }
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