using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeReset : MonoBehaviour
{
    public UIStateManager stateManager;
    void Start()
    {
        if (stateManager.currentState == UIStateManager.UIState.PAUSED)
        {
            stateManager.currentState = UIStateManager.UIState.PLAYING;
        }
    }

}
