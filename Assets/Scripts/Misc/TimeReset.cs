using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeReset : MonoBehaviour
{
    //UIStateManager stateManager;
    void Start()
    {
        //stateManager = GameObject.FindWithTag("StateManager").GetComponent<UIStateManager>();

        if (UIStateManager.currentState == UIStateManager.UIState.PAUSED)
        {
            UIStateManager.currentState = UIStateManager.UIState.PLAYING;
        }
    }

}
