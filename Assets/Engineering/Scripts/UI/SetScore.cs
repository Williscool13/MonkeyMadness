using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SetScore : MonoBehaviour
{
    GameObject _scoreManager;
    Score score;

    GameObject _sceneManager;
    RandomScene randomScene;

    [SerializeField] TMP_Text player1ScoreDisplay;
    [SerializeField] TMP_Text player2ScoreDisplay;
    [SerializeField] GameObject win1;
    [SerializeField] GameObject win2;
    [SerializeField] GameObject p1image;
    [SerializeField] GameObject p2image;
    [SerializeField] GameObject dash;
  

    int _player1Score;
    int _player2Score;

    // Start is called before the first frame update
    void Start()
    {
        _scoreManager = GameObject.FindGameObjectWithTag("score");
        score = _scoreManager.GetComponent<Score>();
      
        _player1Score = score.Player1Score;
        _player2Score = score.Player2Score;
    }

    // Update is called once per frame
    void Update()
    {
        player1ScoreDisplay.SetText(_player1Score.ToString());
        player2ScoreDisplay.SetText(_player2Score.ToString());

        if (_player1Score == 2)
        {
            win2.SetActive(true);
            p2image.SetActive(false);
            dash.SetActive(false);
            player1ScoreDisplay.gameObject.SetActive(false);
            player2ScoreDisplay.gameObject.SetActive(false);
        }

        if (_player2Score == 2)
        {
            win1.SetActive(true);
            p1image.SetActive(false);
            dash.SetActive(false);
            player1ScoreDisplay.gameObject.SetActive(false);
            player2ScoreDisplay.gameObject.SetActive(false);

        }
    }


}
