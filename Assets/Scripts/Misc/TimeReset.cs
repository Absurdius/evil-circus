using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeReset : MonoBehaviour
{
    UIStateManager stateManager;
    void Start()
    {
        stateManager = GameObject.FindWithTag("StateManager").GetComponent<UIStateManager>();

        if (stateManager.currentState == UIStateManager.UIState.PAUSED)
        {
            stateManager.currentState = UIStateManager.UIState.PLAYING;
        }
    }

}
