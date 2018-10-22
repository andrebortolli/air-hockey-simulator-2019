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
        if (axisNameVertical == "VERTICAL0")
        {
            eS.GetComponent<StandaloneInputModule>().horizontalAxis = "HORIZONTAL0";
            eS.GetComponent<StandaloneInputModule>().verticalAxis = "VERTICAL0";
            eS.GetComponent<StandaloneInputModule>().submitButton = "GREEN0";
            eS.GetComponent<StandaloneInputModule>().cancelButton = "RED0";
        }
        else
        {
            if (axisNameVertical == "VERTICAL1")
            {
                eS.GetComponent<StandaloneInputModule>().horizontalAxis = "HORIZONTAL1";
                eS.GetComponent<StandaloneInputModule>().verticalAxis = "VERTICAL1";
                eS.GetComponent<StandaloneInputModule>().submitButton = "GREEN1";
                eS.GetComponent<StandaloneInputModule>().cancelButton = "RED1";
            }
        }
        isThisObjectSelected = true;
    }
    public void OnDeselect(BaseEventData eventData)
    {
        if (axisNameVertical == "VERTICAL0")
        {
            eS.GetComponent<StandaloneInputModule>().horizontalAxis = "HORIZONTAL1";
            eS.GetComponent<StandaloneInputModule>().verticalAxis = "VERTICAL1";
            eS.GetComponent<StandaloneInputModule>().submitButton = "GREEN1";
            eS.GetComponent<StandaloneInputModule>().cancelButton = "RED1";
        }
        else
        {
            if (axisNameVertical == "VERTICAL1")
            {
                eS.GetComponent<StandaloneInputModule>().horizontalAxis = "HORIZONTAL0";
                eS.GetComponent<StandaloneInputModule>().verticalAxis = "VERTICAL0";
                eS.GetComponent<StandaloneInputModule>().submitButton = "GREEN0";
                eS.GetComponent<StandaloneInputModule>().cancelButton = "RED0";
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
