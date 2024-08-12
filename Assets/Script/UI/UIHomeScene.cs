using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIHomeScene : MonoBehaviour
{
    [Header("Home UI")]
    public Transform HomeUI;
    public Image SoundImage;
    public Sprite SoundOn;
    public Sprite SoundOff;

    [Header("Select Mode UI")] public Transform GameModeUI;

    [Header("Levels")] 
    public Transform Levels;
    public Transform Content;
    public Transform LevelTileUI;



    private void Start()
    {
        if (HomeUI == null) HomeUI = transform.Find("Home");
        if (GameModeUI == null) GameModeUI = transform.Find("GameMode");
        if (Levels == null) Levels = transform.Find("Levels");
        ShowHomeUI();
    }


    public void OnSoundButtonClicked()
    {
        SoundImage.sprite = SoundImage.sprite == SoundOn ? SoundOff : SoundOn;
        SoundManager.Instance.ChangeStateSound();
        Refreshsound();
    }

    public void Play()
    {
        HomeUI.gameObject.SetActive(false);
        GameModeUI.gameObject.SetActive(true);
        Levels.gameObject.SetActive(false);
    }

    public void PickModeGame(int mode)
    {
        GameManager.Instance.SetMode(mode);
        HomeUI.gameObject.SetActive(false);
        GameModeUI.gameObject.SetActive(false);
        Levels.gameObject.SetActive(true);
        LoadUILevel();
    }

    public void ShowGameModeUI()
    {
        HomeUI.gameObject.SetActive(false);
        GameModeUI.gameObject.SetActive(true);
        Levels.gameObject.SetActive(false);
    }

    public void ShowHomeUI()
    {
        Refreshsound();
        HomeUI.gameObject.SetActive(true);
        GameModeUI.gameObject.SetActive(false);
        Levels.gameObject.SetActive(false);
    }

    public void Refreshsound()
    {
        SoundImage.sprite = PrefManager.GetInt("Sound") == 1 ? SoundOn : SoundOff;
    }

    public void LoadUILevel()
    {
        if(Content.childCount != 100)
        {
            for (int i = 1; i <= 100; i++)
            {
                SpawnUILevel(i);
                // LevelTile tile = SpawnUILevel(i);
                // int mode = LevelManager.Instance.GetMode();
                // PrefManager.state state = ResourcesManager.Instance.GetStateLevel(mode,i);

            }
            // Debug.Log("spawn level");
        }

        UpdateUILevels();

    }

    public void UpdateUILevels()
    {
        for (int i = 1; i <= 100; i++)
        {
            LevelTile tile = Content.GetChild(i - 1).GetComponent<LevelTile>();
            int mode = GameManager.Instance.GetMode();
            PrefManager.state state = ResourcesManager.Instance.GetStateLevel(mode,i);
            SetStateUI(tile,state);
        }
    }

    protected LevelTile SpawnUILevel(int i)
    {
        Transform obj = Instantiate(LevelTileUI);
        obj.SetParent(Content);
        Button button = obj.GetComponent<Button>();
        button.onClick.AddListener(() => GameManager.Instance.PlayGameWithLevel(i));
        TextMeshProUGUI TextMesh = obj.GetComponentInChildren<TextMeshProUGUI>();
        TextMesh.text = i.ToString();
        LevelTile tile = obj.GetComponent<LevelTile>();
        tile.SetIndex(i);
        return tile;
    }

    public void SetStateUI(LevelTile tile, PrefManager.state state)
    {
        if(state == PrefManager.state.Locked) tile.Locked();
        else if(state == PrefManager.state.Unlocked) tile.Unlocked();
        else if(state == PrefManager.state.Completed) tile.Completed();
    }
}
