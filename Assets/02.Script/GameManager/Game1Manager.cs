using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Game1Manager : MonoBehaviour
{
    public static Game1Manager instance;
    public GameObject PauseMenu;

    private bool pauseFlag = false;
    private bool readyFlag = true;
    private float readycount;
    void Awake() //다른 곳에서도 인스턴스를 접근할 수 있도록 static instance를 만들어 놓습니다.
    {
        if (!instance) //정적으로 자신을 체크합니다.
            instance = this; //정적으로 자신을 저장합니다.
    }
    void Start()
    {
        readycount = 0;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        setGame();
    }

    void Update()
    {
        if(pauseFlag == false)
        {
            readycount += Time.deltaTime;
            if (readycount > 5)
            {
                playGame();
            }
            else
            {
                Debug.Log(Mathf.Ceil(5 - readycount));
            }
        }
        
    }
    private void setGame()
    {
        Debug.Log("set game configuration");
    } 
    private void playGame()
    {
        Debug.Log("game playing");
    }

    public void gameover()
    {
        SceneManager.LoadScene("GameOverScene");
    }
    public void PauseTrue()
    {
        Debug.Log("pause true");
        pauseFlag = true;
        PauseMenu.SetActive(true);
    }
    public void PauseFalse()
    {
        Debug.Log("pause false");
        pauseFlag = false;
        PauseMenu.SetActive(false);
    }
    
    //버튼 리스너
    public void resume()
    {
        PauseFalse();
    }
    public void restart()
    {
        SceneManager.LoadScene("Game1Scene");
    }
    public void home()
    {
        SceneManager.LoadScene("HomeScene");
    }
}
