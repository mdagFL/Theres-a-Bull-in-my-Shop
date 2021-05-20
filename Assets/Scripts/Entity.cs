using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : PhysicsObject
{
    protected Animator myAnime;
    public GameObject exit;
    protected float freezeTimer;
    [HideInInspector]
    public AudioSource myAudio;

    protected override void Start()
    {      
        base.Start();
        myAnime = this.GetComponent<Animator>();
        myAudio = this.GetComponent<AudioSource>();
    }
    protected override void Update()
    {
        base.Update();
        UpdateTimers(Time.deltaTime);
        UpdateTimedStates();
        if (CheckCanMove())
        {
            Move();
        }
        
        UpdateAnimator();
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        BubbleGum gum = other.GetComponent<BubbleGum>();
        if (gum != null && !gum.checkActive)
        {
            gum.checkActive = true;
            Freeze(gum.freezeTime);
            gum.despawnTime = gum.freezeTime;
            gum.myAudio.Play();
        }
    }

    // Deduct time from active timers
    protected virtual void UpdateTimers(float time)
    {
        if (freezeTimer > 0)
            freezeTimer -= time;
    }
    
    // Update any state variables that are timer-dependent
    protected virtual void UpdateTimedStates()
    {
       
    }
    
    // Freeze the entity in place
    private void Freeze(float time)
    {
        freezeTimer = time;
    }

    // Check if the entity can move
    protected virtual bool CheckCanMove()
    {
        if (freezeTimer > 0)
            return false;
        else
            return true;
    }

    // Make next move or stop moving
    protected abstract void Move();

    // Update animator component
    protected virtual void UpdateAnimator()
    {
        if (freezeTimer > 0)
        {
            myAnime.speed = 0;
        }
        else
        {
            myAnime.speed = 1;
        }
    }


}
