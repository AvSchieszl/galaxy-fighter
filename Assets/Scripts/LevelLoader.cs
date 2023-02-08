using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] float gameOverLoadDelay = 1.5f;

    public static bool keyboardControlOn;

    public void PickKeyboardControl()
    {
        keyboardControlOn = true;
        LoadGame();
    }
    public void PickMouseControl()
    {
        keyboardControlOn = false;
        LoadGame();
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
        FindObjectOfType<GameSession>().ResetGame();
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitBeforeLoad());
    }
    IEnumerator WaitBeforeLoad()
    {
        yield return new WaitForSeconds(gameOverLoadDelay);
        SceneManager.LoadScene("Game Over");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

        private void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
