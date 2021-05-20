using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{

    protected Rigidbody myRig;
    public bool isGrabbable;

    protected virtual void Start()
    {       
        myRig = this.GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        
    }
}
