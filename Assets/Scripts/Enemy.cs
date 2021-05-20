using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Entity
{
    public Player player;

    [Header("Enemy Parameters")]
    public int damage;
    public float timeoutTime;
    public float destinationRadius;
    public float pauseTime;
    public float randomPositionMaxRadius;
    public float randomPositionMinRadius;
    public float randomPositionMaxX;
    public float randomPositionMinX;
    public float randomPositionMaxZ;
    public float randomPositionMinZ;

    public float timeoutTimer;
    public float pauseTimer;
    public bool isPaused;
    protected bool canTimeout;
    public bool pauseAtDestination;
    protected NavMeshAgent myNav;
    

    protected override void Start()
    {
        base.Start();
        myNav = GetComponent<NavMeshAgent>();
        pauseAtDestination = true;
        canTimeout = true;
    }

    protected override void Update()
    {
        base.Update();
        if (!CheckCanMove())
        {
            if (!myNav.isStopped) // if it's moving, stop
                myNav.isStopped = true;
        }
        else
        {
            if (myNav.isStopped) // if it's not, resume
                myNav.isStopped = false;
        }
    }
     
    // make a move or stop moving
    protected override void Move()
    {
        bool isAtDestination = CheckReachedDestination();
        if (isAtDestination && !isPaused && pauseAtDestination) // just reached destination
        {
            Pause(pauseTime);
        }
        else if (timeoutTimer <= 0 && canTimeout || isAtDestination) // Timed out or reached destination
        {
            timeoutTimer = timeoutTime;
            SetNextDestination();
        }
    }

    // check if allowed to move
    protected override bool CheckCanMove()
    {
        bool canMove = base.CheckCanMove();
        if (canMove) // additionally, check if paused
        {
            if (isPaused)
                return false;
            else
                return true;
        }
        else
            return false;
    }

    // deduct time from active timers
    protected override void UpdateTimers(float time)
    {
        base.UpdateTimers(time);
        if (pauseTimer > 0)
        {
            pauseTimer -= time;
        }
        if (timeoutTimer > 0)
        {
            timeoutTimer -= time;
        }
    }
    protected override void UpdateTimedStates()
    {
        base.UpdateTimedStates();
        if (isPaused && pauseTimer <= 0 && pauseAtDestination)
            Unpause();
    }

    // check if reached the destination
    protected virtual bool CheckReachedDestination()
    {
        return (RemainingDistance() <= destinationRadius);
    }

    // stop in place
    protected virtual void Pause(float time)
    {
        myAudio.Stop();
        pauseTimer = time;
        isPaused = true;
    }

    // resume moving
    protected virtual void Unpause()
    {
        pauseTimer = 0f;
        isPaused = false;
        if (canTimeout)
            timeoutTimer = timeoutTime;
        SetNextDestination();
    }

    // update animator paramters
    protected override void UpdateAnimator()
    {
        base.UpdateAnimator();
        if (isPaused)
        {
            myAnime.SetInteger("AnimState", 0);
        }
        else
        {
            myAnime.SetInteger("AnimState", 1);
        }
       
    }

    // set the next place to move to
    protected abstract void SetNextDestination();

    // Sample NavMesh for a random next point nearby
    protected Vector3 FindRandomDestination()
    {
        Vector3 attemptedDest;

        float x = Random.Range(randomPositionMinRadius, randomPositionMaxRadius);
        float z = Random.Range(randomPositionMinRadius, randomPositionMaxRadius);

        if (Random.value >= 0.5)
            x = -x;
        if (Random.value >= 0.5)
            z = -z;
        attemptedDest = transform.position + new Vector3(x, 0, z);


        NavMeshHit hit;
        NavMesh.SamplePosition(attemptedDest, out hit, randomPositionMaxRadius * 2, 1);

        return hit.position;

        
    }

    public float RemainingDistance()
    {
        return (myNav.destination - this.transform.position).magnitude;
    }
}
