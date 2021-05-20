using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopper : Human
{
    public Bull myBull;
    public float fleeSpeed;
    public float bullFleeRadius;
    private bool isFleeing;
    public AudioClip[] audios;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        if (myBull != null &&
            !isFleeing && myBull.isCharging && (myBull.transform.position - this.transform.position).magnitude <= bullFleeRadius)
            Flee();
        base.Update();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (other.gameObject.Equals(exit) && isFleeing)
        {
            player.Hurt(damage);
            Destroy(this.gameObject);
        }
    }

    protected override void Move()
    {
        base.Move();
    }
    protected override void SetNextDestination()
    {
        if (!isFleeing)
            base.SetNextDestination();
        else
            this.myNav.destination = exit.transform.position;
    }
    private void Flee()
    {
        this.myNav.destination = exit.transform.position;
        canTimeout = false;
        pauseAtDestination = false;
        if (isPaused)
            Unpause();
        isFleeing = true;
        myNav.speed = fleeSpeed;
        myAudio.clip = audios[0];
        myAudio.Play();
    }

    protected override void UpdateAnimator()
    {
        base.UpdateAnimator();
        if (isFleeing)
        {           
            myAnime.SetInteger("AnimState", 2);
        }          
    }

    protected override void Pause(float time)
    {
        base.Pause(time);
        if (Random.value > .5)
        {
            if (Random.value > .5)
                myAudio.clip = audios[1];
            else
                myAudio.clip = audios[2];
            myAudio.Play();
        }
    }
}
