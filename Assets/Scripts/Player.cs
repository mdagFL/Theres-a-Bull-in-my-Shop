using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player : Entity
{
    public float grabDistance;
    private bool isGrabbing;
    private GameObject objectGrabbing;
    private Rigidbody objGrabRig;

    public int HP;
    public int MaxHP;

    public float timeLimit;
    private float timeElapsed;

    //public CharacterController playerCon;

    public float runSpeed;
    public float walkSpeed;
    private float currSpeed;

    //public float pushForce;

    private Bull myBull;
    private bool canPush;
    public float pushTime;
    private float pushTimer;

    private Camera mainCam;
    private CameraAim mainCamAim;
    public UI playerUI;
    public AudioClip[] audios;
    private SceneController myController;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        myController = FindObjectOfType<SceneController>();
        InitHealthAndTime(myController.myLevel, myController.difficulty);
        Physics.IgnoreLayerCollision(9, 8);
        //playerCon = this.GetComponent<CharacterController>();
        currSpeed = runSpeed;
        canPush = false;
        pushTimer = pushTime;       
        HP = MaxHP;
        mainCam = Camera.main;
        mainCamAim = mainCam.GetComponent<CameraAim>();

       
    }

    private void InitHealthAndTime(int level, int difficulty)
    {
        switch (level)
        {
            case 1:
                switch (difficulty)
                {
                    case 0:
                        MaxHP = 75;
                        timeLimit = 180;
                        break;
                    case 1:
                        MaxHP = 50;
                        timeLimit = 120;
                        break;
                    case 2:
                        MaxHP = 20;
                        timeLimit = 40;
                        break;
                }
                break;
            case 2:
                switch (difficulty)
                {
                    case 0:
                        MaxHP = 150;
                        timeLimit = 260;
                        break;
                    case 1:
                        MaxHP = 100;
                        timeLimit = 180;
                        break;
                    case 2:
                        MaxHP = 60;
                        timeLimit = 100;
                        break;
                }
                break;
            case 3:
                switch (difficulty)
                {
                    case 0:
                        MaxHP = 150;
                        timeLimit = 260;
                        break;
                    case 1:
                        MaxHP = 100;
                        timeLimit = 200;
                        break;
                    case 2:
                        MaxHP = 65;
                        timeLimit = 100;
                        break;
                }
                break;
                
        }
        playerUI.timeCounter = timeLimit;
        playerUI.hp.text = "HP: " + MaxHP.ToString();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        TryGrab();
        CheckPause();
        if (isGrabbing)
        {
            objGrabRig.velocity = Vector3.zero;
        }
    }

    public void Hurt(int damage)
    {
        HP -= damage;
        if (HP <= 0)
            Die();
        playerUI.UpdateHP(HP);
    }

    private void Die()
    {
        myAudio.clip = audios[3];
        myAudio.Play();
        playerUI.Lose();
    }

    protected override void Move()
    {
        float moveHor = Input.GetAxisRaw("Horizontal") * currSpeed;
        float moveVer = Input.GetAxisRaw("Vertical") * currSpeed;

        Vector3 tempVel = ((myRig.transform.right * moveHor + myRig.transform.forward * moveVer).normalized * currSpeed +
            new Vector3(0.0f, myRig.velocity.y, 0.0f));

        if (!canPush)
            myRig.velocity = Vector3.Lerp(myRig.velocity, tempVel, 0.5f);

        else
            BullPushPlayer();
    }

    
    private void BullPushPlayer()
    {
        freezeTimer = 0;
        //Vector3 pushTo = (myBull.transform.forward);
        if (myBull != null)
        {
            myRig.velocity += myBull.transform.forward * myBull.chargeSpeed * 15 * Time.deltaTime;
        }

        if (pushTimer > 0)
            pushTimer -= Time.deltaTime;
        else
        {
            
            pushTimer = pushTime;
            canPush = false;
            
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponent<Bull>() != null && collision.gameObject.GetComponent<Bull>().isCharging)
        {
            myBull = collision.gameObject.GetComponent<Bull>();

            canPush = true;
        }
    }


    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<BubbleGum>() != null)
        {
            myRig.velocity = new Vector3(0, 0, 0);
        }
            
        base.OnTriggerEnter(other);
        
    }

    public void Grab(RaycastHit hitRay)
    {
        if (hitRay.collider.gameObject.transform.parent == null)
        {
            if (hitRay.collider.gameObject.GetComponent<PhysicsObject>() != null && 
                hitRay.collider.gameObject.GetComponent<PhysicsObject>().isGrabbable)
            {
                objectGrabbing = hitRay.collider.gameObject;
                isGrabbing = true;
                currSpeed = walkSpeed;
            }
        }
        else
        {
            if (hitRay.collider.gameObject.transform.parent.gameObject.GetComponent<PhysicsObject>() != null && 
                hitRay.collider.gameObject.transform.parent.gameObject.GetComponent<PhysicsObject>().isGrabbable)
            {
                objectGrabbing = hitRay.collider.gameObject.transform.parent.gameObject;
                isGrabbing = true;
                currSpeed = walkSpeed;
            }
        }
        Shelf shelf;
        
        if (objectGrabbing.GetComponent<Shelf>() != null)
        {
            shelf = objectGrabbing.GetComponent<Shelf>();
            if (shelf != null)
            {
                for (int i = 0; i < shelf.china.Count; i++)
                {
                    shelf.china[i].GetComponent<Rigidbody>().isKinematic = true;
                }
            }
        }
         
        objGrabRig = objectGrabbing.GetComponent<Rigidbody>();
        objGrabRig.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;

        playerUI.ChangeCrosshair(new Color32(0,255,255,255));
        playerUI.crossLock = true;
        objectGrabbing.transform.parent = Camera.main.transform;
        mainCamAim.grabLock = true;
    }

    public void TryGrab()
    {
        RaycastHit hitRay;
        //Current mouse position
        //Ray useRay = Camera.main.ScreenPointToRay(Input.mousePosition);
       
        if (isGrabbing == false && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hitRay, grabDistance))
        {
            
            GameObject objHit = hitRay.collider.gameObject;
            // get the greatest grandparent
            while (objHit.transform.parent != null)
                objHit = objHit.transform.parent.gameObject;

            PhysicsObject physicsHit = objHit.GetComponent<PhysicsObject>();
            
            if (isGrabbing == false)
            {
                
                if ( physicsHit != null && physicsHit.isGrabbable)
                {
                    playerUI.ChangeCrosshair(new Color32(0, 255, 120, 255));
                    if (Input.GetMouseButtonDown(0) && isGrabbing == false)
                    {
                        Grab(hitRay);
                    }
                }
                else
                {
                    playerUI.ChangeCrosshair(new Color32(99, 99, 99, 255));
                }
            }
        }
        else
        {
            playerUI.ChangeCrosshair(new Color32(99, 99, 99, 255));
        }

        if (Input.GetMouseButtonUp(0) && isGrabbing == true)
        {
            Release();
        }
    }

    public void Release()
    {
        Shelf shelf;
        if (objectGrabbing.GetComponent<Shelf>() != null)
        {
            shelf = objectGrabbing.GetComponent<Shelf>();
            if (shelf != null)
            {
                for (int i = 0; i < shelf.china.Count; i++)
                {
                    shelf.china[i].GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }


        objGrabRig = objectGrabbing.GetComponent<Rigidbody>();

        objGrabRig.isKinematic = false;
        objGrabRig.constraints = RigidbodyConstraints.None;
        objGrabRig.velocity = myRig.velocity;

        currSpeed = runSpeed;
        objectGrabbing.transform.parent = null;
        playerUI.crossLock = false;
        playerUI.ChangeCrosshair(new Color32(99,99,99,255));
        isGrabbing = false;
        mainCamAim.grabLock = false;
    }

    public int CalculateScore()
    {
        return (HP*3 + Mathf.RoundToInt(playerUI.timeCounter*2));
    }

    protected override void UpdateAnimator()
    {
        base.UpdateAnimator();
        float horz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        if (horz == 0 && vert == 0)//Idle
        {
            myAnime.SetInteger("AnimState", 0);
        }
        else if (vert < 0)//Backwards
        {
            myAnime.SetInteger("AnimState", 2);
        }
        else if (vert > 0)//Forwards
        {
            myAnime.SetInteger("AnimState", 1);
        }
        else if (horz < 0)//Left
        {
            myAnime.SetInteger("AnimState", 4);
        }
        else if (horz > 0)//Right
        {
            myAnime.SetInteger("AnimState", 3);
        }
    }

    public void CheckPause()
    {
        if((Input.GetKeyDown("escape") || Input.GetKeyDown(KeyCode.Return))&& SceneManager.GetSceneByBuildIndex(0) != SceneManager.GetActiveScene() && !playerUI.levelEnding)
        {
            Pause();
        }
    }

    public void Pause()
    {
        playerUI.PauseToggle(); 
    }
}
