using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private float collisionSoundEffect = 1f;
    public float audioFootVolume = 1f;
    public float soundEffectPitchRandomness = 0.05f;

    private AudioSource audiosource;
    public AudioClip genericFootSound;
    public AudioClip metalFootSound;


    void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }

    void FootSound(){
        audiosource.volume=collisionSoundEffect*audioFootVolume;
        audiosource.pitch=Random.Range(1.0f+soundEffectPitchRandomness, 1.0f+ soundEffectPitchRandomness);

        if(Random.Range(0,2)>0){
            audiosource.clip=genericFootSound;

        }else{
            audiosource.clip=metalFootSound;
        }
        audiosource.Play();
    }


}
