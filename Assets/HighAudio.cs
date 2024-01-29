using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighAudio : MonoBehaviour
{
    public static HighAudio _instance;
    [SerializeField] AudioClip shoot;
    [SerializeField] AudioSource SoundFx;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void Awake()
    {
        if (_instance == null) _instance = this;
    }

    public void playShoot()
    {
        SoundFx.PlayOneShot(shoot);
    }
}
