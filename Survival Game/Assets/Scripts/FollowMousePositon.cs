using UnityEngine;

public class FollowMousePositon : MonoBehaviour
{
    void LateUpdate()
    {
        transform.position = Input.mousePosition;
    }
}
