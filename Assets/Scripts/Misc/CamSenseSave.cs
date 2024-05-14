using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSenseSave : MonoBehaviour
{
    public static CamSenseSave instance;

    public float camSense;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
