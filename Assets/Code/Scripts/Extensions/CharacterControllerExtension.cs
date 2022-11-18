using UnityEngine;

namespace Code.Extensions
{
    public static class CharacterControllerExtension
    {
        public static void LerpHeight(this CharacterController controller, float height, float speed)
        {
            float center = height / 2f;
            controller.height = Mathf.Lerp(controller.height, height, speed);
            controller.center = Vector3.Lerp(controller.center, new Vector3(0f, center, 0f), speed);
        }

        public static Vector3 GetTopCenterPosition(this CharacterController controller)
        {
            Vector3 position = GetTopPosition(controller);
            position.y -= controller.radius;
            return position;
        }

        public static Vector3 GetBottomCenterPosition(this CharacterController controller)
        {
            Vector3 position = GetBottomPosition(controller);
            position.y += controller.radius;
            return position;
        }

        public static Vector3 GetTopPosition(this CharacterController controller)
        {
            Vector3 position = (controller.transform.position + controller.center) + new Vector3(0f, (controller.height / 2f), 0f);
            return position;
        }

        public static Vector3 GetBottomPosition(this CharacterController controller)
        {
            Vector3 position = (controller.transform.position + controller.center) - new Vector3(0f, (controller.height / 2f), 0f);
            return position;
        }
    }
}