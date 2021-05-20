using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shelf : PhysicsObject
{
    public List<GameObject> china;
    public AudioClip[] sounds;
    private AudioSource source;

    protected override void Start()
    {
        base.Start();
        isGrabbable = true;
        source = this.GetComponent<AudioSource>();
        source.playOnAwake = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "MainCamera" || collision.gameObject.name == "BULL")
        {
            Random.InitState(Random.Range(int.MinValue, int.MaxValue));
            int randNum = Random.Range(0, 3);
            source.clip = sounds[randNum];
            source.Play();
        }
        if (transform.parent != null && collision.gameObject.GetComponent<PhysicsObject>() == null && collision.gameObject.tag != "Ground")
        {
            CameraAim cam = transform.parent.GetComponent<CameraAim>();
            if (cam != null)
            {
                cam.player.Release();
            }
        }
    }
}
