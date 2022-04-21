using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class startGame : MonoBehaviour
{
    public string LevelName;
    

    public void LoadLevel()
    {
        PlayerPrefs.SetInt("timeAttack", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(LevelName);
       
        
    }
    public void LoadLevelTimer()
    {
        PlayerPrefs.SetInt("timeAttack", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(LevelName);

        
    }


}
