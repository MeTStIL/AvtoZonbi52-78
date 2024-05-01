using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    public bool isStatic = false;
    private int sorterOrderBase = 0;
    private Renderer renderer;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    private void LateUpdate()
    {
        // LateUpdate - чтобы всегда выпонлялось только после передвижения героя
        renderer.sortingOrder = (int)(sorterOrderBase - transform.position.y);
        if (isStatic)
            Destroy(this);
    }
}
