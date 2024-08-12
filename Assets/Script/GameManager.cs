using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private int ModeSelected = 0;
    [SerializeField] private int LevelIndex;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMode(int mode)
    {
        ModeSelected = mode;
    }

    public int GetMode()
    {
        return ModeSelected;
    }

    public void PlayGameWithLevel(int idx)
    {
        string key = "Mode " + ModeSelected + " Level " + idx;
        if(PrefManager.GetState(key) == PrefManager.state.Locked) return;
        //Debug.Log(PrefManager.GetState(key));
        StartCoroutine(PlayGame(idx));
    }

    public IEnumerator PlayGame(int idx)
    {
        LevelIndex = idx;
        // Debug.Log("mode: " + ModeSelected + " level: " + LevelIndex);
        LoadScene(1);
        yield return new WaitUntil(() => LevelManager.Instance != null);
        LevelManager.Instance.PlayLevel(ModeSelected,LevelIndex);
    }

    public void LoadScene(int sceneIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
    }
}
