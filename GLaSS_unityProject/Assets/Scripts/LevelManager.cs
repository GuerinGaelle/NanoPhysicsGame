using UnityEngine;
using System.Collections;

public static class LevelManager
{
    public static int UnlockedLevels = 1;

    public static void LoadData()
    {
        UnlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 1);
    }

    public static void SaveData(int _levelUnlocked)
    {
        UnlockedLevels = _levelUnlocked;
        PlayerPrefs.SetInt("UnlockedLevels", _levelUnlocked);
        PlayerPrefs.Save();
    }
}
