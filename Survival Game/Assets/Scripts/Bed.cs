using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{

    DayTimeController timeController;
    PlayerMovement playerMovement;
    bool skippingTime = false;

    Animation anima;

    [SerializeField] int timeSkipInMinutes = 480;
    [SerializeField] float waitTime = 2f;
    float tempTime = 0;

    bool playedDarken = false;
    bool playedLighten = false;

    void Start()
    {
        timeController = GameObject.Find("DayController").GetComponent<DayTimeController>();
        playerMovement = GameObject.Find("Player").GetComponent<PlayerMovement>();
        anima = GetComponent<Animation>();
        anima = GameObject.Find("Black Screen").GetComponent<Animation>();

        tempTime = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(skippingTime){
            if(playedDarken && !anima.IsPlaying("darken")){
                tempTime -= Time.deltaTime;

                if(tempTime <= 0){
                    if(!playedLighten){
                        anima.clip = anima.GetClip("lighten");
                        anima.Play();
                        playedLighten = true;

                        float difference = 1440f - timeController.dayTime;
                        if(difference >= timeSkipInMinutes){
                            timeController.dayTime += timeSkipInMinutes;
                        } else {
                            timeController.dayTime = timeSkipInMinutes - difference;
                        }
                        
                    } else {
                        if(!anima.IsPlaying("lighten")){
                            anima.clip = null;
                            skippingTime = false;
                            tempTime = waitTime;
                            playerMovement.canMove = true;
                            playerMovement.canToggleInventory = true;
                            playedDarken = false;
                            playedLighten = false;
                        }
                    }
                }
            }
        }
    }

    public void SkipTime(){
        anima.clip = anima.GetClip("darken");
        anima.Play();
        playedDarken = true;
        skippingTime = true;
        playerMovement.canMove = false;
        playerMovement.canToggleInventory = false;
    }
}
