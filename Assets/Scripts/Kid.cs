using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : Human
{

    private float gumSpawnTimer;
    private bool isSpittingGum;
    [Header("Kid Parameters")]
    public BubblegumProjectile bubblegum;
    public float gumSpawnTime;
    public float gumSpawnChance;
    public float spitTime;
    public float spitAngle;
    public AudioClip[] audios;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Move()
    {
        base.Move();
        /*
        if (CheckReachedDestination() && isPaused) // just reached destination
        {
            TrySpit();
        }
        else if (isSpittingGum && !isPaused)
        {
            isSpittingGum = false;
        }
        */
    }
    protected override void UpdateTimers(float time)
    {
        base.UpdateTimers(time);
        if (gumSpawnTimer > 0)
            gumSpawnTimer -= time;
    }

    protected override void UpdateTimedStates()
    {
        if (pauseTimer <= 0 && isSpittingGum == true)
        {
            isSpittingGum = false;
        }
        base.UpdateTimedStates();
    }

    protected override void Unpause()
    {
        TrySpit();
        if (!isSpittingGum)
            base.Unpause();
    }
    private void TrySpit()
    {
        if (gumSpawnTimer <= 0 && Random.value <= gumSpawnChance)
            SpitGum();
    }

    private void SpitGum()
    {
        myAudio.clip = audios[0];
        myAudio.Play();
        isSpittingGum = true;
        gumSpawnTimer = gumSpawnTime;
        Vector3 spawnPoint = transform.position + transform.forward*0.2f + transform.up*1.5f;
        BubblegumProjectile instance = GameObject.Instantiate(bubblegum, spawnPoint,
            this.transform.rotation);
        Pause(spitTime);
    }

    protected override void UpdateAnimator()
    {
        base.UpdateAnimator();
        if (isSpittingGum)
            myAnime.SetInteger("AnimState", 2);
            
    }
}
