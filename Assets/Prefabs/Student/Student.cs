using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Game
{
    public class Student : MonoBehaviour
    {
        [SerializeField] private Transform studentTransform;

        [FormerlySerializedAs("bottomHairTransform")] [SerializeField]
        private SpriteRenderer bottomPart;

        [FormerlySerializedAs("middleHairTransform")] [SerializeField]
        private SpriteRenderer middlePart;

        [FormerlySerializedAs("topHairTransform")] [SerializeField]
        private SpriteRenderer topPart;

        public float speed = 10f;

        private void Start()
        {
            var middleScale = middlePart.gameObject.transform.localScale;
            middleScale.y = Random.Range(1f, 7f);
            middlePart.gameObject.transform.localScale = middleScale;

            var topHeight = topPart.bounds.size.y;
            var middleHeight = middlePart.bounds.size.y;
            var bottomHeight = bottomPart.bounds.size.y;

            // 计算中间部分的新位置
            var bottomTopEdge = bottomPart.transform.position + Vector3.up * (bottomHeight / 2);
            var newMiddlePosition = bottomTopEdge + Vector3.up * (middleHeight / 2);
            middlePart.transform.position = new Vector3(middlePart.transform.position.x, newMiddlePosition.y,
                middlePart.transform.position.z);

            // 调整顶部位置
            var middleTopEdge = middlePart.transform.position + Vector3.up * (middleHeight / 2);
            var newTopPosition = middleTopEdge + Vector3.up * (topHeight / 2);
            topPart.transform.position = new Vector3(topPart.transform.position.x, newTopPosition.y,
                topPart.transform.position.z);
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            var position = studentTransform.position;
            position.x += speed * Time.deltaTime;
            studentTransform.position = position;
        }
    }
}