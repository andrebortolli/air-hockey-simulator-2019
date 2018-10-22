using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighscoreOKButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public string axisName;
    public EventSystem eS;

    public void OnSelect(BaseEventData eventData)
    {
        if (axisName == "VERTICAL0")
        {
            eS.GetComponent<StandaloneInputModule>().horizontalAxis = "HORIZONTAL0";
        }
        else
        {
            if (axisName == "VERTICAL1")
            {
                eS.GetComponent<StandaloneInputModule>().horizontalAxis = "HORIZONTAL1";
            }
        }

    }
    public void OnDeselect(BaseEventData eventData)
    {
        if (axisName == "VERTICAL0")
        {
            eS.GetComponent<StandaloneInputModule>().horizontalAxis = "HORIZONTAL0";
        }
        else
        {
            if (axisName == "VERTICAL1")
            {
                eS.GetComponent<StandaloneInputModule>().horizontalAxis = "HORIZONTAL1";
            }
        }
    }
}
