using UnityEngine;

namespace Scripts.States.Parameters
{
    public class MotionParameters : MonoBehaviour
    {
        public float speed = 5.0f;
        public float jumpHeight = 2.0f;
        public float jumpBufferTime = 0f;
        public float runSpeed = 10.0f;
        public float acceleration = 2.0f;

        public float hangingMoveSpeed = 2.0f;
        public float maxFallingSpeed = -10f;
    }
}