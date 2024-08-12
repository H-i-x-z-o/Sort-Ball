using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    public PopUp popUp;


    public void OnClickMenuButton()
    {
        popUp.OnCickExitButton();
    }

    public void OnClickSkipButton()
    {
        popUp.OnClickSkipButton();
    }


}
