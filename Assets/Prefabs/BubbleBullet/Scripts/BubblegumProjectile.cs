using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubblegumProjectile : PhysicsObject
{
    public GameObject bubblegum;
    public float initialSpeed;
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();

        myRig.velocity = (this.transform.forward*initialSpeed);
    }

    protected override void Update()
    {
        base.Update();

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ground")
        {
            GameObject.Instantiate(bubblegum, this.transform.position + new Vector3(0, 0.15f, 0), Quaternion.identity);
            Destroy(this.gameObject);
        }
        else if(other.gameObject.GetComponent<Entity>() == null)
            Destroy(this.gameObject);
    }
}
