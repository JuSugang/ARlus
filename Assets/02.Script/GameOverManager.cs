using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameOverManager : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void restart() //재시작 버튼을 누름
    {
        SceneManager.LoadScene("Game1Scene");
    }
    public void home()
    {
        SceneManager.LoadScene("HomeScene");
    }

}
