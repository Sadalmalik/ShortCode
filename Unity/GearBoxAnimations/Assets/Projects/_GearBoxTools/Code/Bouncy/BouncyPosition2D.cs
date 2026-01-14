using UnityEngine;

namespace GearBoxTools
{
    public class BouncyPosition2D : MonoBehaviour
    {
        public RectTransform ControllableRect;
        public RectTransform FollowRect;

        [Space]
        public SpringSetting positionSpring;

        public Spring xPositionSpring;
        public Spring yPositionSpring;

        [Space]
        public Vector2 targetPosition;

        private void Awake()
        {
            targetPosition = ControllableRect.anchoredPosition;
            xPositionSpring.value = ControllableRect.anchoredPosition.x;
            yPositionSpring.value = ControllableRect.anchoredPosition.y;
        }

        private void FixedUpdate()
        {
            if (FollowRect != null)
            {
                targetPosition = FollowRect.anchoredPosition;
            }

            var dt = Time.fixedDeltaTime;
            positionSpring.Tick(dt, targetPosition.x, ref xPositionSpring);
            positionSpring.Tick(dt, targetPosition.y, ref yPositionSpring);

            ControllableRect.anchoredPosition = new Vector2(xPositionSpring.value, yPositionSpring.value);
        }

        public void BumpPosition(Vector2 direction)
        {
            xPositionSpring.velocity += direction.x;
            yPositionSpring.velocity += direction.y;
        }

        public void SetupPosition(Vector2 position, Vector2 velocity, Vector2 target)
        {
            xPositionSpring.value = position.x;
            yPositionSpring.value = position.y;
            xPositionSpring.velocity = velocity.x;
            yPositionSpring.velocity = velocity.y;
            targetPosition = target;
        }
    }
}