using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddtoCartPanelOpen : MonoBehaviour
{

    public GameObject Panel;

    public void OpenPanel()
    {
        if (Panel != null)
        {
            bool isActive = Panel.activeSelf;
            Panel.SetActive(!isActive);
        }
    }

    public Text Textfield;

    public void SetText(string text)
    {
        Textfield.text = text;
    }


}