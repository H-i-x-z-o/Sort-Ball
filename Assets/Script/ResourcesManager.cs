using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    public static ResourcesManager Instance;
    public List<TextAsset> ListGameMode;

    [Serializable] public class GameMode
    {
        public Level[] levels;
    }

    [Serializable] public class Level
    {
        public int no;
        public Data_Bottle[] Bottles;
    }

    [Serializable] public class Data_Bottle
    {
        public int[] values;
    }

    public GameMode[] gameModes;

    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        gameModes = new GameMode[3];
        for (int i = 0; i < 3; i++)
        {
            TextAsset textAsset = ListGameMode[i];
            gameModes[i] = JsonUtility.FromJson<GameMode>(textAsset.text);
        }
        PrefManager.SetInt("Sound", PrefManager.GetInt("Sound") == 1 ? 1 : 0);
        PrefManager.SetState("Mode 0 Level 1", PrefManager.GetState("Mode 0 Level 1") == PrefManager.state.Completed ? PrefManager.state.Completed : PrefManager.state.Unlocked);
        PrefManager.SetState("Mode 1 Level 1", PrefManager.GetState("Mode 1 Level 1") == PrefManager.state.Completed ? PrefManager.state.Completed : PrefManager.state.Unlocked);
        PrefManager.SetState("Mode 2 Level 1", PrefManager.GetState("Mode 2 Level 1") == PrefManager.state.Completed ? PrefManager.state.Completed : PrefManager.state.Unlocked);
    }

    public PrefManager.state GetStateLevel(int mode, int idx)
    {
        string key = "Mode " + mode + " Level " + idx;
        PrefManager.state state = PrefManager.GetState(key);
        return state;
    }

    public Level GetLevel(int mode, int idx)
    {
        return gameModes[mode].levels[idx];
    }

}
