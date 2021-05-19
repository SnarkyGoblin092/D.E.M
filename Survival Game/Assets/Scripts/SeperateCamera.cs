using UnityEngine;

public class SeperateCamera : MonoBehaviour
{
    [SerializeField] GameObject cameraHolder;

    void Start()
    {
        cameraHolder.transform.parent = null;
    }
}
