using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Bull : Enemy
{
    [Header ("Bull Patrol AI")]
    public float patrolSightAngleRadius;
    public float patrolSpeed;
    public float patrolStoppingDistance;
    public float patrolAcceleration;
    public float patrolTurnSpeed;

    [Header ("Bull Charge AI")]
    public float chargeLosePlayerTime;
    public float chargeSightAngleRadius;
    public float chargeSpeed;
    public float chargeStoppingDistance;
    public float chargeAccleration;
    public float chargeTurnSpeed;
    public float chargeDestinationRadius;
    public float chargeDestinationTime;
    public float chargeRehitDelayTime;

    public bool isCharging;
    private float losePlayerTimer;
    private float chargeDestinationTimer;
    private float chargeRehitDelayTimer;
    private SceneController mySceneController;
    public AudioClip[] audios;

    [Header("DEBUG")]
    public float REMAINING_DISTANCE;
    

    protected override void Start()
    {
        base.Start();
        StopCharging(); // init patrolling state
        mySceneController = FindObjectOfType<SceneController>();
        myAudio = GetComponent<AudioSource>();
    }

    protected override void Update()
    {
        bool playerFound = LookForPlayer();
        if (playerFound && !isCharging) // look for the player, regardless of whether or not the bull moves
            BeginCharging();
        else if (playerFound) // if charging, just reset the timer
            losePlayerTimer = chargeLosePlayerTime;
        base.Update();
        REMAINING_DISTANCE = myNav.remainingDistance;
    }

    // begin charging from patrolling
    private void BeginCharging()
    {
        canTimeout = false;
        isCharging = true;
        pauseAtDestination = false;
        losePlayerTimer = chargeLosePlayerTime;
        myNav.acceleration = chargeAccleration;
        myNav.angularSpeed = chargeTurnSpeed;
        myNav.stoppingDistance = chargeStoppingDistance;
        myNav.speed = chargeSpeed;
        myNav.autoBraking = false;

        if (isPaused)
        {
            Unpause();
        }
        myAudio.clip = audios[1];
        myAudio.Play();
        
        
    }

    // stop charging and resume patrolling
    private void StopCharging()
    {
        isCharging = false;
        canTimeout = true;
        pauseAtDestination = true;
        myNav.acceleration = patrolAcceleration;
        myNav.angularSpeed = patrolTurnSpeed;
        myNav.stoppingDistance = patrolStoppingDistance;
        myNav.speed = patrolSpeed;
        myAudio.clip = audios[0];
        myNav.autoBraking = true;
    }

    protected override void Move()
    {
        if (isCharging)
        {
            SetNextDestination();
        }
        else
            base.Move();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isCharging && collision.gameObject.Equals(player.gameObject) && chargeRehitDelayTimer <= 0)
        {
            chargeRehitDelayTimer = chargeRehitDelayTime;
            player.Hurt(damage);
            if (Random.value > .5)
                player.myAudio.clip = player.audios[1];
            else
                player.myAudio.clip = player.audios[2];
            player.myAudio.Play();
        }
        
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        try // in case exit is null
        {
            if (other.gameObject.Equals(exit))
            {
                UI ui = FindObjectOfType<UI>();
                ui.Win();
                Destroy(this.gameObject);
            }
        }
        catch
        {
            if (other.name == "Exit" || other.tag == "Exit")
            {
                Debug.Log("win - null reference exception caught!"); // win sequence
            }
        }
    }

    protected override bool CheckReachedDestination()
    {
        if (!isCharging)
            return base.CheckReachedDestination();
        else
            return (RemainingDistance() <= chargeDestinationRadius);
    }


    // determine next destination
    protected override void SetNextDestination()
    {
        if (isCharging)
        {
            if (losePlayerTimer <= 0) // lost the player
            {
                StopCharging();
                myNav.destination = base.FindRandomDestination();
                myAudio.Play();
                return;
            }

            // charge AI

            // Aims to the spot just behind the player relative the bull
            if (chargeDestinationTimer <= 0)
            {
                Vector3 chargeDest = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) +
                                (new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z)
                                - this.transform.position).normalized * 4f;
                NavMeshHit hit;
                NavMesh.SamplePosition(chargeDest, out hit, (player.transform.position - this.transform.position).magnitude * 2, 1);
                myNav.destination = hit.position;
                chargeDestinationTimer = chargeDestinationTime;
            }
            
            
        }
        else
        {
            // patrol AI
            myNav.destination = base.FindRandomDestination();
            myAudio.Play();
        }
    }

    // deduct time from active timers
    protected override void UpdateTimers(float time)
    {
        base.UpdateTimers(time);
        if (isCharging)
        {
            losePlayerTimer -= time;
            chargeDestinationTimer -= time;
        }
        
        if (chargeRehitDelayTimer > 0)
            chargeRehitDelayTimer -= time;
        
    }

    // check if player is in sight
    private bool LookForPlayer()
    {
        if (CheckLookingToPlayer() && CheckRaycastToPlayer())
        {
            return true;
        }
        return false;
    }
    
    // check if player is in angle radius
    private bool CheckLookingToPlayer()
    {
        float angle = Vector3.SignedAngle(transform.forward, (player.transform.position - transform.position), transform.up);
        if (Mathf.Abs(angle) <= ((isCharging)? chargeSightAngleRadius : patrolSightAngleRadius))
        {
            return true;
        }
        return false;
    }

    // try raycasting to the player. Returns true if it hits the player
    private bool CheckRaycastToPlayer()
    {
        RaycastHit hit;
        Vector3 toPlayer = new Vector3(0, 1, 0) + player.transform.position - this.transform.position;
        Physics.Raycast(this.transform.position + this.transform.forward, toPlayer, out hit);
        if (hit.transform != null && hit.transform.gameObject.GetComponent<Player>() != null)
            return true;
        return false;

    }
    protected override void UpdateAnimator()
    {
        base.UpdateAnimator();
        if (isCharging)
        {
            myAnime.SetInteger("AnimState", 2);
        }
    }
}
