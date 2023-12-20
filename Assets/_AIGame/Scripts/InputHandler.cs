﻿using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public PlayerController playerController;
    public CombatController combatController;
          
    private bool wasMoving = false;
          
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        bool jump = Input.GetButtonDown("Jump");
          
        if (jump)
        {
            playerController.Jump();
        }
          
        if(horizontal != 0 || vertical != 0)
        {
            playerController.Move(horizontal, vertical);
            wasMoving = true;
        }
        else if (wasMoving) 
        {
            playerController.StopMoving();
            wasMoving = false;
        }
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        playerController.Run(isRunning);
        if (Input.GetMouseButtonDown(0) && combatController != null)
        {
            combatController.Attack();
        }
    }
}