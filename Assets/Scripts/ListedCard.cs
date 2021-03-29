using UnityEngine;
using System.Collections;
using System;

using UnityEngine.UI;

public class ListedCard  
{

    public int        color;
    public GameObject gameObject;
    public Image      im;
    public Button     btn;
    public Card       script;
    public ListedCard(int newColor, GameObject newCardInHand, Image newIm, Button newBtn, Card newCardScript)
    {
        color      = newColor;
        gameObject = newCardInHand;
        im         = newIm;
        btn        = newBtn;
        script = newCardScript;
    } 
}
