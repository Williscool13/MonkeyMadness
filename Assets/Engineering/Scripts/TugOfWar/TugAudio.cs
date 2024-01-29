using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TugAudio : MonoBehaviour
{
    public static TugAudio _instance;
    [SerializeField] AudioClip fall;
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

    public void playFall()
    {
        SoundFx.PlayOneShot(fall);
    }
}
