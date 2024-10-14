using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [Header ("Game Over")]
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioClip gameOverSound;

    [Header ("Pause")]
    [SerializeField] private GameObject pauseScreen;

    private void Awake()
    {
        gameOverScreen.SetActive(false);
        pauseScreen.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            //If the pause screen is already open close it, else open it
            if (pauseScreen.activeInHierarchy)
                PauseGame(false);
            else
                PauseGame(true);
        }
    }

    #region Game Over
    //Activate game over screen
    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    //Game over functions

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit(); //Quits the game (only works in build)

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; //Quits in unity
#endif
    }
    #endregion

    #region Pause
    public void PauseGame(bool status)
    {
        //if status ==true pause. If false unpause
        pauseScreen.SetActive(status);


        //When pause status is true timescale to 0 (time stops)
        //When false change to 1 (time moves normally)
        //Can play with timescale to get slow-mo or super fast too
        if (status)
            Time.timeScale = 0;

        else
            Time.timeScale = 1;
    }

    public void SoundVolume()
    {
        SoundManager.instance.ChangeSoundVolume(.2f);
    }

    public void MusicVolume()
    {
        SoundManager.instance.ChangeMusicVolume(.2f);
    }
    #endregion
}
