using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class NameInputScript : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    bool isThisObjectSelected;
    public string axisNameHorizontal;
    public string axisNameVertical;
    public GameObject arrows;
    public EventSystem eS;
    float timer;

    public void OnSelect(BaseEventData eventData)
    {
        if (axisNameVertical == "VerticalP1")
        {
            eS.GetComponent<StandaloneInputModule>().horizontalAxis = "HorizontalP1";
            eS.GetComponent<StandaloneInputModule>().verticalAxis = "VerticalP1";
            eS.GetComponent<StandaloneInputModule>().submitButton = "Submit";
            eS.GetComponent<StandaloneInputModule>().cancelButton = "Cancel";
        }
        else
        {
            if (axisNameVertical == "VerticalP2")
            {
                eS.GetComponent<StandaloneInputModule>().horizontalAxis = "HorizontalP2";
                eS.GetComponent<StandaloneInputModule>().verticalAxis = "VerticalP2";
                eS.GetComponent<StandaloneInputModule>().submitButton = "Submit";
                eS.GetComponent<StandaloneInputModule>().cancelButton = "Cancel";
            }
        }
        isThisObjectSelected = true;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if (axisNameVertical == "VerticalP1")
        {
            eS.GetComponent<StandaloneInputModule>().horizontalAxis = "HorizontalP2";
            eS.GetComponent<StandaloneInputModule>().verticalAxis = "VerticalP2";
            eS.GetComponent<StandaloneInputModule>().submitButton = "Submit";
            eS.GetComponent<StandaloneInputModule>().cancelButton = "Cancel";
        }
        else
        {
            if (axisNameVertical == "VerticalP2")
            {
                eS.GetComponent<StandaloneInputModule>().horizontalAxis = "HorizontalP1";
                eS.GetComponent<StandaloneInputModule>().verticalAxis = "VerticalP1";
                eS.GetComponent<StandaloneInputModule>().submitButton = "Submit";
                eS.GetComponent<StandaloneInputModule>().cancelButton = "Cancel";
            }
        }
        isThisObjectSelected = false;
    }

    // Update is called once per frame
    void Update ()
    {
        timer += Time.deltaTime;

	    if (isThisObjectSelected)
        {
            arrows.SetActive(true);
            if(Input.GetAxis(axisNameVertical) < 0 && timer >= 0.15f)
            {
                if (GetComponent<TMP_Dropdown>().value == 25)
                {
                    GetComponent<TMP_Dropdown>().value = 0;
                }
                else
                {
                    GetComponent<TMP_Dropdown>().value += 1;
                }
                timer = 0.0f;
            }
            if(Input.GetAxis(axisNameVertical) > 0 && timer >= 0.15f)
            {
                if (GetComponent<TMP_Dropdown>().value == 0)
                {
                    GetComponent<TMP_Dropdown>().value = 25;
                }
                else
                {
                    GetComponent<TMP_Dropdown>().value -= 1;
                }
                timer = 0.0f;
            }
        }
        else
        {
            arrows.SetActive(false);
        }
	}
}
