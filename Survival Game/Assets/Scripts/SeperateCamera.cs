using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeperateCamera : MonoBehaviour
{
    [SerializeField] GameObject cameraHolder;

    void Start()
    {
        cameraHolder.transform.parent = null;
    }
}
