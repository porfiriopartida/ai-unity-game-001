using _AIGame.Scripts.States;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
        public IPlayerState CurrentState;
        public Transform groundCheck;
        public CharacterController controller;
        public LayerMask groundLayer;
        
        
        private void Start()
        {
            TransitionToState(new GroundedState(this));
        }


        // private bool isGrounded()
        // {
        //     return controller.isGrounded || _isGrounded;
        // }
        void Update()
        {
            CurrentState.HandleUpdate();
        }
        public void TransitionToState(IPlayerState newState)
        {
            // Call Exit on the previous state before transitioning to new state
            CurrentState?.Exit();

            CurrentState = newState;
        
            // Call Enter on the new state after transitioning from old state
            CurrentState.Enter();
        }

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), $"Player Y position: {transform.position.y}");
        GUI.Label(new Rect(10, 30, 300, 20), $"controller.isGrounded: {controller.isGrounded}");
        // GUI.Label(new Rect(10, 50, 300, 20), $"Current Speed: {currentSpeed}");
    }

    // Drawing Gizmos for ground check
    void OnDrawGizmos() 
    {
        // if (groundCheck != null)
        // {
        //     Gizmos.color = Color.red;
        //     Gizmos.DrawRay(groundCheck.position, -Vector3.up * checkDistance);
        // }
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * 2);
    }

    public void SetMovingStatus(bool moving)
    {
        CurrentState.SetMovingStatus(moving);
    }

    public float GetVerticalVelocity()
    {
        return CurrentState.GetVerticalVelocity();
    }

    public void JumpRequested()
    {
        if (CurrentState is IJumpableState jumpableState)
        {
            jumpableState.Jump();
        }
    }
    public void StopMoving()
    {
        // CurrentState.SetMovingStatus(false);
        CurrentState.StopMoving();
    }
    // Inside PlayerController
    public void Move(float horizontal, float vertical)
    {
        CurrentState.Move(horizontal, vertical);
    }

    public void Run(bool isRunning)
    {
        // Check if currentState type implements IRunnableState
        if (CurrentState is IRunnableState runnableState)
        {
            runnableState.Run(isRunning);
        }
    }

    public bool isGrounded()
    {
        return controller.isGrounded;
    }
}
