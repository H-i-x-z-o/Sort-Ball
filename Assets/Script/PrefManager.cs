using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefManager
{
    public static void  SetState(string key, state value)
    {
        if(PlayerPrefs.GetInt(key) == state.Completed.GetHashCode())
        {
            return;
        }
        PlayerPrefs.SetInt(key, (int)value);
    }

    public static state GetState(string key)
    {
        if(PlayerPrefs.HasKey(key) == false)
        {
            PlayerPrefs.SetInt(key, 0);
        }
        return (state)PlayerPrefs.GetInt(key);
    }

    public static bool HasKey(string key)
    {
        return PlayerPrefs.HasKey(key);
    }

    public static void SetInt(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public static int GetInt(string key)
    {
        if(PlayerPrefs.HasKey(key) == false)
        {
            PlayerPrefs.SetInt(key, key == "Sound" ? 1 : 0);
        }
        return PlayerPrefs.GetInt(key);
    }

    public static void DeleteData()
    {
        PlayerPrefs.DeleteAll();
    }

    public enum state
    {
        Locked,
        Unlocked,
        Completed
    }

}
