using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    Text score;
    GameSession gameSession;
    // Start is called before the first frame update
    void Start()
    {
        score = GetComponent<Text>();
        gameSession = FindObjectOfType<GameSession>();
    }

    // Update is called once per frame
    void Update()
    {
        score.text = gameSession.GetScore().ToString();
    }
}
