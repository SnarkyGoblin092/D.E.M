﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMousePositon : MonoBehaviour
{
    void LateUpdate()
    {
        transform.position = Input.mousePosition;
    }
}
