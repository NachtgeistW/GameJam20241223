using UnityEngine;

namespace Game
{
    public class StudentMove : MonoBehaviour
    {
        [SerializeField] private Transform studentTransform;

        public float speed = 10f;
        private void Update()
        {
            var position = studentTransform.position;
            position.x += speed * Time.deltaTime;
            studentTransform.position = position;
        }
    }
}