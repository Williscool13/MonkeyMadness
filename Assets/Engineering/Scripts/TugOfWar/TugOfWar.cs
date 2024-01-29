using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using TMPro;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;

public class TugOfWar : MonoBehaviour, IInputReceiver
{
    [SerializeField] GameObject Player1;
    [SerializeField] GameObject Player2;
    Vector3 player1Position;
    Vector3 player2Position;
    [SerializeField] TMP_Text textPlayer1;
    [SerializeField] TMP_Text textPlayer2;
    [SerializeField] TMP_Text winLoseText;
    private string[] player1Input = { "w", "a", "d" };
    private string[] player2Input = { "up", "left", "right" };

    string _1whatKey;
    string _2whatKey;

    float _keyTimer = 0f;
    float _gTimer = 0f;

    float _changeKeyTimer = 3f;
    float _gameTimer = 15;

    float pullingPower = .2f;

    bool canClick = true;

    [SerializeField] Animator anim1;
    [SerializeField] Animator anim2;


    [SerializeField] ParticleSystem particle1;
    [SerializeField] ParticleSystem particle2;
    [SerializeField] GameObject tali;

    [Title("Audio")]
    [SerializeField] AudioSource stretchSource;
    [SerializeField] AudioSource bgmSource;
    [SerializeField] AudioClip[] stretchClip;

    [SerializeField] Animator keycaps1;
    [SerializeField] Animator keycaps2;

    Vector3 taliPosition;
    Vector3 targetPosition2;

    bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
       
        ChangeKey();
        player1Position = Player1.transform.position;
        player2Position = Player2.transform.position;

        InputMaster.Instance.RegisterInputReceiver(this);
        
    }

    private void OnDestroy() {
        InputMaster.Instance.UnregisterInputReceiver(this);
    }

    bool PlayerOneInputCheck(string whatKey) {
        switch (whatKey) {
            case "w":
                if (p1.middlePressed) {
                    return true;
                }
                break;
            case "a":
                if (p1.leftPressed) {
                    return true;
                }
                break;
            case "d":
                if (p1.rightPressed) {
                    return true;
                }
                break;
        
        }
        return false;
    }

    bool PlayerTwoInputCheck(string whatKey) {
        switch (whatKey) {
            case "up":
                if (p2.middlePressed) {
                    return true;
                }
                break;
            case "left":
                if (p2.leftPressed) {
                    return true;
                }
                break;
            case "right":
                if (p2.rightPressed) {
                    return true;
                }
                break;
        
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        targetPosition2 = new Vector3(1.5f,-9.3f,1.2f);
        _keyTimer += Time.deltaTime;
        _gTimer += Time.deltaTime;
        Player1.transform.position = player1Position;
        Player2.transform.position = player2Position;
        

        if(_keyTimer >= _changeKeyTimer)
        {
            ChangeKey();
            Debug.Log("Change Key");
        }

        if(_gTimer >= _gameTimer)
        {
            GameOver();
            Debug.Log("here");
        }

        if (PlayerOneInputCheck(_1whatKey) && canClick == true)
        {
            particle1.Play();
            stretchSource.PlayOneShot(stretchClip[Random.Range(0, stretchClip.Length)]);
            player1Position.x -= pullingPower;
            player2Position.x -= pullingPower;
            taliPosition.x -= pullingPower;
            
        }

        if (PlayerTwoInputCheck(_2whatKey) && canClick == true)
        {
            stretchSource.PlayOneShot(stretchClip[Random.Range(0, stretchClip.Length)]);
            particle2.Play();
            player1Position.x += pullingPower;
            player2Position.x += pullingPower;
            taliPosition.x += pullingPower;
        }

        if (player2Position.x < 3.73)
        {
            Player1Win();
            Debug.Log(player2Position);  
        }

        if (player1Position.x > -3.49)
        {
            Player2Win();
            Debug.Log(player1Position);
        }

        if (canClick)
        {
            if(_1whatKey == "w")
            {
                keycaps1.SetTrigger("Up");
            }

            if (_1whatKey == "a")
            {
                keycaps1.SetTrigger("Left");
            }

            if (_1whatKey == "d")
            {
                keycaps1.SetTrigger("Right");
            }

            if(_2whatKey == "up")
            {
                keycaps2.SetTrigger("Up");
            }

            if (_2whatKey == "left")
            {
                keycaps2.SetTrigger("Left");
            }

            if (_2whatKey == "right")
            {
                keycaps2.SetTrigger("Right");
            }



            textPlayer1.SetText(_1whatKey);
            textPlayer2.SetText(_2whatKey);
        }



    }

    private void GameOver()
    {
        if (player2Position.x <= 5.04)

        {
            Player1Win();
        }
        else
        {
            Player2Win();
        }
    }

   

    void ChangeKey()
    {
        _keyTimer = 0;
        _1whatKey = player1Input[Random.Range(0, player1Input.Length)];
        _2whatKey = player2Input[Random.Range(0, player2Input.Length)];
    }

    void Player1Win()
    {
        gameOver = true;
        Destroy(tali);
        winLoseText.SetText("Player 1 Won");
        canClick = false;
        textPlayer1.SetText("Win");
        textPlayer2.SetText("Lose");
        anim1.SetBool("Win", true);
        anim2.SetBool("Lose", true);
        anim2.applyRootMotion = false;
        TugAudio._instance.playFall();

        Score.Instance.AddPlayer1Score();
    }

    void Player2Win()
    {
        gameOver = true;
        Destroy(tali);
        winLoseText.SetText("Player 2 Won");
        canClick = false;
        //textPlayer2.SetText("Win");
        //textPlayer1.SetText("Lose");
        anim2.SetBool("Win", true);
        anim1.SetBool("Lose", true);
        anim1.applyRootMotion = false;
        TugAudio._instance.playFall();
       

        Score.Instance.AddPlayer2Score();
    }

    PlayerInputs p1;
    PlayerInputs p2;
    public void SetPlayerInputs(int playerId, bool left, bool right, bool middle, bool leftHeld, bool rightHeld, bool middleHeld) {
        switch (playerId) {
            case 0:
                p1 = new PlayerInputs(0, left, right, middle, leftHeld, rightHeld, middleHeld);
                break;
            case 1:
                p2 = new PlayerInputs(1, left, right, middle, leftHeld, rightHeld, middleHeld);
                break;
        }
    }
}
