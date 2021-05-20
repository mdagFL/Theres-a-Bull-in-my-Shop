using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tempCam : MonoBehaviour
{
    private float mouseUp;
    private float mouseH;
    public Rigidbody myRig;
    public Player player;
    public GameObject camLoc;
    public float Sensitivity;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        myRig = this.GetComponent<Rigidbody>();

        mouseUp = 0f;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = camLoc.transform.position;

        float horz = Input.GetAxisRaw("Mouse X") * 5;//* Sensitivity * Time.deltaTime;
        float vert = Input.GetAxisRaw("Mouse Y") * 5;//Sensitivity * Time.deltaTime;


        mouseH += horz;
        mouseUp += vert;

        if (mouseUp > 60)
            mouseUp = 60;
        else if (mouseUp < -60)
            mouseUp = -60;  

        myRig.angularVelocity = new Vector3(mouseUp, mouseH, 0);
        this.transform.eulerAngles = new Vector3(mouseUp * -1, mouseH, 0);
        player.transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);


        //myRig.rotation.SetFromToRotation(transform.forward, new Vector3())
    }
}
