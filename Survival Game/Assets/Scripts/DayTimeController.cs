using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class DayTimeController : MonoBehaviour
{
    [Range(0f, 1440f)]
    public float dayTime = 360f;

    [SerializeField] float timeScale;
    [SerializeField] float speed;

    [SerializeField] Light sun;
    [SerializeField] Light moon;

    [SerializeField] Volume skyVolume;
    [SerializeField] AnimationCurve starsCurve;

    PhysicallyBasedSky sky;

    public bool isNight;

    public float progress = 0f;

    void Awake()
    {
        skyVolume.profile.TryGet<PhysicallyBasedSky>(out sky);
    }
    
    private void OnValidate()
    {
        skyVolume.profile.TryGet<PhysicallyBasedSky>(out sky);
        UpdateTime();
    }
    
    void Update()
    {
        UpdateTime();
    }

    void UpdateTime()
    {
        dayTime += Time.deltaTime * timeScale * speed;
        if (dayTime > 1440f)
            dayTime = 0f;

        if ((dayTime > 346f && dayTime < 377f) || (dayTime > 1070f && dayTime < 1096f))
        {
            progress += Time.deltaTime;
            if (progress > 1f)
                progress = 1f;
            timeScale = Mathf.Lerp(1f, 10f, progress);
        }
        else
        {
            if (progress > 0f)
            {
                progress -= Time.deltaTime;
                if (progress < 0f)
                    progress = 0f;
                timeScale = Mathf.Lerp(1f, 10f, progress);
            }
        }

        float alpha = dayTime / 1440.0f;
        float timeRotation = Mathf.Lerp(-90f, 270f, alpha);
        transform.rotation = Quaternion.Euler(timeRotation, transform.rotation.y, transform.rotation.z);
        CheckNightDayTransition();
        sky.spaceEmissionMultiplier.value = starsCurve.Evaluate(alpha) * 1f;
    }

    void CheckNightDayTransition()
    {
        if (isNight)
        {
            if (dayTime > 360 && dayTime < 1080)
            {
                StartDay();
            }
        }
        else
        {
            if (dayTime > 1080 || dayTime < 360)
            {
                StartNight();
            }
        }
    }

    void StartDay()
    {
        isNight = false;
        moon.shadows = LightShadows.None;
        sun.shadows = LightShadows.Soft;
    }

    void StartNight()
    { 
        isNight = true;
        sun.shadows = LightShadows.None;
        moon.shadows = LightShadows.Soft;
    }
}
