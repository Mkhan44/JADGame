using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Universal_Dialouge_Box : MonoBehaviour
{
    public static Universal_Dialouge_Box instance;

    [SerializeField] TextMeshProUGUI displayText;
    [SerializeField] TextMeshProUGUI buttonText;
    [SerializeField] GameObject blockerPanel;
    bool isActive;

    private void Awake()
    {
        instance = this;
        isActive = false;
    }

    public void activatePopup(string textToDisplay, string buttonTextToDisplay = "Close")
    {

        buttonText.text = buttonTextToDisplay;
        displayText.text = textToDisplay;
        isActive = true;
        blockerPanel.SetActive(true);
    }

    public void deactivatePopup()
    {
        isActive = false;
        blockerPanel.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
