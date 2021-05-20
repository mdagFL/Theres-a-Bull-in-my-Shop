using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public GameObject door;
    public GameObject garage;
    public Material myMat;

    public float doorAngle;
    private float currDoorAngle;

    public float garageOpeningSpeed;
    public float garageOpeningTime;
    private float garageOpeningTimer;

    public bool hasGarageDoor;
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        myMat = this.GetComponent<Renderer>().material;
        garageOpeningTimer = garageOpeningTime;
        currDoorAngle = doorAngle;
    }

    private void Update()
    {
        if (!hasGarageDoor)
            RegularDoor();

        else
        {
            RegularDoor();
            GarageDoor();
        }
            
    }

    private void RegularDoor()
    {
        if (isActive)
            OpenRegularDoor();

        else
            CloseRegularDoor();
    }

    private void GarageDoor()
    {
        if (isActive)
            OpenGarageDoor();

        else
            CloseGarageDoor();
    }

    private void OpenRegularDoor()
    {

        if (currDoorAngle >= 0)
        {
            door.transform.Rotate(0,0,1);
            currDoorAngle--;
        }
        

    }

    private void CloseRegularDoor()
    {
        if (currDoorAngle <= doorAngle)
        {
            door.transform.Rotate(0, 0, -1);
            currDoorAngle++;
        }
    }

    private void OpenGarageDoor()
    {
        if(garageOpeningTimer > 0)
        {
            garage.transform.position += new Vector3(0, .2f, 0) * garageOpeningSpeed * Time.deltaTime;
            garageOpeningTimer -= Time.deltaTime;
        }
            
    }

    private void CloseGarageDoor()
    {
        if (garageOpeningTimer < garageOpeningTime)
        {
            garage.transform.position -= new Vector3(0, .2f, 0) * garageOpeningSpeed * Time.deltaTime;
            garageOpeningTimer += Time.deltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Ground")
        {
            myMat.color = Color.green;

            isActive = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Ground")
        {
            myMat.color = Color.red;

            isActive = false;
        }
    }


}
