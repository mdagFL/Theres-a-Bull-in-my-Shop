using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomShatterSound : MonoBehaviour
{
    public AudioClip[] sounds;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = this.GetComponent<AudioSource>();
        Random.InitState(Random.Range(int.MinValue, int.MaxValue));
        int randNum = Random.Range(0, sounds.Length);
        source.clip = sounds[randNum];
        source.Play();
    }
}
