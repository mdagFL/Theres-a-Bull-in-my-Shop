using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTest : MonoBehaviour
{
    // Start is called before the first frame update
    Camera kam;
    Camera playerCam;
    void Start()
    {
        kam = this.GetComponent<Camera>();
        playerCam = GameObject.FindObjectOfType<Player>().gameObject.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxisRaw("Fire1") == 1)
        {
            playerCam.gameObject.tag = "MainCamera";
            kam.gameObject.tag = "Untagged";
        }
    }
}
