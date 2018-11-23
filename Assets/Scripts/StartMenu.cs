using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartMenu : MonoBehaviour {

    public bool easterEggActivated = false;
    public TMP_Dropdown dropdown;
    // Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Space) && Input.GetKey(KeyCode.E) && Input.GetKeyDown(KeyCode.D) && easterEggActivated == false)
        {
            TMP_Dropdown.OptionData demoModeOption = new TMP_Dropdown.OptionData("Demo Mode");
            dropdown.options.Add(demoModeOption);
            easterEggActivated = true;
        }
    }
}
