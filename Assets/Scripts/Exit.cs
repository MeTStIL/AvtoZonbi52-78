using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [CanBeNull][SerializeField] private GameObject exitCollision;
    [SerializeField] private int killsCount;
    [SerializeField] [CanBeNull] private GameObject dialog;

    public void GetAccess()
    {
        if (exitCollision != null)
            exitCollision.SetActive(true);
    }

    private void Update()
    {
        if (PlayerStats.kills >= killsCount)
        {
            if (dialog != null)
                dialog.SetActive(true);
            else
                GetAccess();
            
        }
    }
}
