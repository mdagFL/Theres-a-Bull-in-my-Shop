using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempPlayer : MonoBehaviour
{
    public float currSpeed;
    public Rigidbody myRig;
    // Start is called before the first frame update
    void Start()
    {
        myRig = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveHor = Input.GetAxisRaw("Horizontal") * currSpeed;
        float moveVer = Input.GetAxisRaw("Vertical") * currSpeed;

        Vector3 tempVel = ((myRig.transform.right * moveHor + myRig.transform.forward * moveVer).normalized * currSpeed +
            new Vector3(0.0f, myRig.velocity.y, 0.0f));

        myRig.velocity = Vector3.Lerp(myRig.velocity, tempVel, 0.5f);
    }
}
