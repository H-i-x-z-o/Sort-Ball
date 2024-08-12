using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PopUp : MonoBehaviour
{
    public Transform ExitPopUp;
    public Transform SkipPopUp;
    public Transform LevelComplete;
    public Transform TutorialPopUp;
    public ParticleSystem confetti;

    public void OnCickExitButton()
    {
        gameObject.SetActive(true);
        ExitPopUp.gameObject.SetActive(true);
        SkipPopUp.gameObject.SetActive(false);
        LevelComplete.gameObject.SetActive(false);
        TutorialPopUp.gameObject.SetActive(false);
    }

    public void OnClickSkipButton()
    {
        gameObject.SetActive(true);
        SkipPopUp.gameObject.SetActive(true);
        ExitPopUp.gameObject.SetActive(false);
        LevelComplete.gameObject.SetActive(false);
        TutorialPopUp.gameObject.SetActive(false);

    }

    public void YesSkip()
    {
        LevelManager.Instance.NextLevel();
        HidePopUp();
    }

    public void LevelCompletePopUp()
    {
        gameObject.SetActive(true);
        LevelComplete.gameObject.SetActive(true);
        confetti.gameObject.SetActive(true);
        confetti.Play();
        TutorialPopUp.gameObject.SetActive(false);
        ExitPopUp.gameObject.SetActive(false);
        SkipPopUp.gameObject.SetActive(false);
    }

    public void HidePopUp()
    {
        gameObject.SetActive(false);
        TutorialPopUp.gameObject.SetActive(false);
        ExitPopUp.gameObject.SetActive(false);
        SkipPopUp.gameObject.SetActive(false);
        LevelComplete.gameObject.SetActive(false);
    }

    public void ShowTutorial()
    {
        gameObject.SetActive(true);
        TutorialPopUp.gameObject.SetActive(true);
        ExitPopUp.gameObject.SetActive(false);
        SkipPopUp.gameObject.SetActive(false);
        LevelComplete.gameObject.SetActive(false);
    }
}
