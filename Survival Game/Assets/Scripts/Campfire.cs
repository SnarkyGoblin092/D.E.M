using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    
    ParticleSystem particles;
    Light lights;

    ParticleSystem smoke;

    public bool playing = false;
    void Start()
    {
        particles = transform.GetChild(3).GetComponent<ParticleSystem>();
        smoke = transform.GetChild(4).GetComponent<ParticleSystem>();
        lights = transform.GetChild(3).GetChild(0).GetComponent<Light>();
    }

    private void Update() {
        if(playing){
            PlayAnimation();
        } else {
            StopAnimation();
        }
    }
    
    void PlayAnimation(){
        particles.enableEmission = true;
        smoke.enableEmission = true;
        lights.enabled = true;
    }

    void StopAnimation(){
        particles.enableEmission = false;
        smoke.enableEmission = false;
        lights.enabled = false;
    }
}
