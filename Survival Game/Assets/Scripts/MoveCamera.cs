using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] Transform cameraPostion;

    private void Update()
    {
        transform.position = cameraPostion.position;
    }
}
