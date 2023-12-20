﻿using UnityEngine;

public interface IPlayerState
{
    void Enter();
    void HandleUpdate();
    void Exit();
    void SetMovingStatus(bool newStatus);
    float GetVerticalVelocity();

    void Move(float horizontal, float vertical);
    void StopMoving();
}