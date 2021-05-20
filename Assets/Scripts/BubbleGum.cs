using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGum : MonoBehaviour
{
    public float despawnTime;
    public float freezeTime;
    public bool checkActive;
    public AudioSource myAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        myAudio = this.GetComponent<AudioSource>();
        checkActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        despawnTime -= Time.deltaTime;
        CheckDespawn();
    }

    private void CheckDespawn()
    {
        if (despawnTime <= 0)
            Destroy(this.gameObject);
    }
}
