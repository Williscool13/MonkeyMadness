using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
public class Score : MonoBehaviour
{

    [SerializeField][ReadOnly] int _player1Score = 0;
    [SerializeField][ReadOnly] int _player2Score = 0;

    public int Player1Score => _player1Score;
    public int Player2Score => _player2Score;

    public static Score Instance { get; private set; }
    public NullEvent nullEvent;

    private void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == SceneReference.Score) {
            // wait 5 seconds,
            // then change to next
            StartCoroutine(changeScene(5.0f));
        } else if(scene.name == SceneReference.MainMenu) {
            _player1Score = 0;
            _player2Score = 0;
        } else if (scene.name == SceneReference.GameSlot) {
            StartCoroutine(changeScene(6.0f));
        }
    }

    public void AddPlayer1Score()
    {
        _player1Score += 1;
    }

    public void AddPlayer2Score()
    {
        _player2Score += 1;
    }



    void ChangeScene()
    {
        nullEvent.Raise(null);
    }

    IEnumerator changeScene(float delay)
    {
        Debug.Log("Transition to next scene started");
        //wait
        yield return new WaitForSeconds(delay);

        ChangeScene();


        /*if (_player1Score < 2 || _player2Score < 2)
        {
        }

        if (Player1Score == 2 || _player2Score == 2)
        {
            //Change scene to gameover
        }*/


    }

}
