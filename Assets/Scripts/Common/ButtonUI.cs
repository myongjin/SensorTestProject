using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUI : MonoBehaviour
{

    public bool isClicked = false;
    public Image image;
    public Text text;
    public Color targetColor;
    private Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        originalColor = image.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleButton()
    {
        isClicked = !isClicked;
        if (isClicked)
        {
            image.color = targetColor;
        }
        else
        {
            image.color = originalColor;
        }

    }


}
