using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Human : Enemy
{
    [Header("Human Parameters")]
    public float chanceToShelf;

    public Shelf[] shelves;
    public Button[] buttons;
    protected bool isSeekingShelf;
    protected Shelf shelfToFind;

    protected override void Start()
    {   
        base.Start();
        shelves = GameObject.FindObjectsOfType<Shelf>();

    }

    protected override void Update()
    {
        base.Update();
    }

    // set next destination to a sampled point from a shelf
    protected void SeekShelf()
    {
        // pick a shelf
        int shelfIndex = Random.Range(0, shelves.Length);
        shelfToFind = shelves[shelfIndex];

        // find point on NavMesh
        NavMeshHit hit;
        NavMesh.SamplePosition(shelfToFind.transform.position + new Vector3(Random.value, 0, Random.value), 
            out hit, 2 * (shelfToFind.transform.position - this.transform.position).magnitude, 1);
        myNav.destination = hit.position;
        myNav.SetDestination(hit.position);
    }
    
    // set next destination of myNav
    protected override void SetNextDestination()
    {
        if (Random.value > (1-chanceToShelf)) // 50% chance to pick a random shelf
        {
            SeekShelf();
        }
        else // find a random spot
        {
            myNav.destination = base.FindRandomDestination();
        }
    }
    protected override void UpdateTimers(float time)
    {
        base.UpdateTimers(time);
    }

    protected override void Pause(float time)
    {
        base.Pause(time);
        if (isSeekingShelf) // examine the shelf
            this.transform.LookAt(shelfToFind.transform);
    }


    protected override void UpdateAnimator()
    {
        base.UpdateAnimator();
    }
}
