using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameOverCanvas;
    public Text uiDistance;
    public GameObject player;


    private Vector2 startPosiion;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        Time.timeScale = 1f;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosiion = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        UpdateUI();
    }

    void UpdateUI()
    {
        Vector2 distance = (Vector2)player.transform.position - startPosiion;
        distance.y = 0;

        if (distance.x < 0)
        {
            distance.x = 0;
        }
        UpdateDistance(distance.x);
    }

    void UpdateDistance(float distanceX)
    {
        uiDistance.text = distanceX.ToString("F0") + "m";
    }

    public void GameOver()
    {
        gameOverCanvas.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
