using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class HomeManager : MonoBehaviour {

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
    }
	// Update is called once per frame
	void Update () {

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
    public void SelectLevel()
    {
        SceneManager.LoadScene("LevelScene"); 
    }
    public void Setting()
    {
        SceneManager.LoadScene("SettingScene");
    }

}
