using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class BasicAI : MonoBehaviour 
{
    public float speed = 5.0f;
    public float roamAreaRadius = 10.0f;

    private Vector3 originPosition;
    public Transform target; 
    public Vector3 roamPosition;
    public LayerMask targetMask;

    // Radiuses
    public float chaseRadius;
    public float attackRadius;

    private State _currentState;
    public State CurrentState
    {
        get 
        { 
            return _currentState; 
        }
        set 
        {
            _currentState = value; 
            Debug.Log("currentState has been updated to: " + _currentState);
        }
    }

    private void Start() 
    {
        originPosition = transform.position;
        _currentState = State.Roaming;
        FindNewRoamPosition();
    }

    private void Update() 
    {
        switch (CurrentState) 
        {
            case State.Roaming:
                if (Vector3.Distance(transform.position, roamPosition) < 1f)
                {
                    FindNewRoamPosition();
                }
                Roam();
                if (isWithinRadius(chaseRadius)) CurrentState = State.Chasing;
                break;
            case State.Chasing:
                Chase();
                if (isWithinRadius(attackRadius)) CurrentState = State.Attacking;
                else if (!isWithinRadius(chaseRadius))
                {
                    CurrentState = State.Roaming;
                    target = null;
                }

                break;
            case State.Attacking:
                Attack();
                if (!isWithinRadius(attackRadius)) CurrentState = State.Chasing;
                break;
        }
    }

    private void Roam()
    {
        transform.position = Vector3.MoveTowards(transform.position, roamPosition, speed * Time.deltaTime);
    }

    private void Chase() 
    {
        // Chasing the target
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    void FindNewRoamPosition()
    {
        float wanderOffset = Random.Range(-roamAreaRadius + originPosition.x, roamAreaRadius + originPosition.x);
        float wanderOffsetZ = Random.Range(-roamAreaRadius + originPosition.z, roamAreaRadius + originPosition.z);
        roamPosition = new Vector3(wanderOffset, 0, wanderOffsetZ);
    }
    private void Attack() 
    {
        // Attacking logic here
    }

    private bool isWithinRadius(float radius)
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, radius, targetMask);

        if(targetsInViewRadius.Length != 0)
        {
            target = targetsInViewRadius[0].transform;
            return true;
        }
        else
        {
            return false;
        }
    }
    void OnDrawGizmosSelected()
    {
        // For Chasing
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRadius);

        // Attacking
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}