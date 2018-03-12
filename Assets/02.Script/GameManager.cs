using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{

    public GameObject PauseMenu;
    private bool pauseFlag = false;
    // Use this for initialization
    void Start()
    {
        gameStart();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void gameStart()
    {
        Debug.Log("start");
    }
    public void resume()
    {
        PauseFalse();
    }
    public void restart()
    {
        PauseFalse();
        gameStart();
    }
    public void home()
    {
        SceneManager.LoadScene("HomeScene");
    }
    public void gameover()
    {
        SceneManager.LoadScene("GameOverScene");
    }
    public void PauseTrue()
    {
        pauseFlag = true;
        PauseMenu.SetActive(true);
    }
    public void PauseFalse()
    {
        pauseFlag = false;
        PauseMenu.SetActive(false);
    }
}
