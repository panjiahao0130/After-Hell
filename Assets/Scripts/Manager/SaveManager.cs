using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : Singleton<SaveManager>
{
    private string currentScene;

    public string CurrentScene => PlayerPrefs.GetString(currentScene);
    public int NextSceneIndex => SceneManager.GetActiveScene().buildIndex+1;

    private void Save(object data, string key)
    {
        var jsonData = JsonUtility.ToJson(data, true);
        PlayerPrefs.SetString(key,jsonData);
        PlayerPrefs.Save();
    }
    private void Load(object data, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString(key),data);
        }
    }

    public void SaveSceneName()
    {
        //保存当前场景索引
        PlayerPrefs.SetString(currentScene,SceneManager.GetActiveScene().name);
        PlayerPrefs.Save();
    }
}
