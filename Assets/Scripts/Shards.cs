using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shards : PhysicsObject
{
    public int min;
    public int max;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ExplodeShards();
    }

    public void ExplodeShards()
    {
        myRig.AddExplosionForce(Random.Range(min, max), this.transform.position, 1);
    }
}
