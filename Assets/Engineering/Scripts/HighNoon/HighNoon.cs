using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Sirenix.OdinInspector;
using DG.Tweening;
public class HighNoon : MonoBehaviour, IInputReceiver
{

    private float _timer;
    [SerializeField][ReadOnly] private float _endTimer = 0;
    [SerializeField][ReadOnly] private bool _haveShoot;
    [SerializeField][ReadOnly] private bool _canShoot;
    [SerializeField] Vector2 timeRange = new Vector2(5,10);

    [SerializeField] TMP_Text indicator;
    [SerializeField] GameObject player1;
    [SerializeField] GameObject player2;

    [Title("Audio Sources")]
    [SerializeField] AudioSource bellToll;
    [SerializeField] AudioSource gunClick;
    [SerializeField] AudioSource gunShot;
    [SerializeField] AudioSource victorySting;
    [SerializeField] AudioSource monkeySounds;
    [SerializeField] AudioSource BGM;

    GameObject _scoreManager;



    // Start is called before the first frame update
    void Start()
    {
        /*_timer = Random.Range(timeRange.x, timeRange.y);
        Debug.Log(_timer);
*/
        _scoreManager = Score.Instance.gameObject;
        Debug.Assert(_scoreManager != null, "ScoreManager is null");

        InputMaster.Instance.RegisterInputReceiver(this);

        _timer = 100f;

        indicator.text = "11:55";

        player2.GetComponent<Animator>().SetBool("PlayerTwo", true);
        player1.GetComponent<Animator>().SetTrigger("Init");
        player2.GetComponent<Animator>().SetTrigger("Init");


        DOTween.Sequence()
            .AppendInterval(1f)
            .AppendCallback(() => indicator.text = "11:56")
            .AppendInterval(1f)
            .AppendCallback(() => indicator.text = "11:57")
            .AppendInterval(1f)
            .AppendCallback(() => indicator.text = "11:58")
            .AppendInterval(1f)
            .AppendCallback(() => indicator.text = "11:59")
            .AppendCallback(() => _endTimer = 0)
            .AppendCallback(() => _timer = Random.Range(timeRange.x, timeRange.y)); 

        DOTween.Sequence()
            .Append(player1.transform.DOBlendableRotateBy(new Vector3(0, 180, 0), 0.5f))
            .Append(player1.transform.DOMoveX(-5f, 1f))
            .Append(player1.transform.DOBlendableRotateBy(new Vector3(0, 180, 0), 0.5f));

        DOTween.Sequence()
            .Append(player2.transform.DOBlendableRotateBy(new Vector3(0, 180, 0), 0.5f))
            .Append(player2.transform.DOMoveX(5f, 1f))
            .Append(player2.transform.DOBlendableRotateBy(new Vector3(0, 180, 0), 0.5f));
    }


    private void OnDestroy() {
        InputMaster.Instance.UnregisterInputReceiver(this);
    }


    [SerializeField] float cooldown = 1f;
    [SerializeField][ReadOnly] float playerOneCooldownTimer = 0;
    [SerializeField][ReadOnly] float playerTwoCooldownTimer = 0;
    [SerializeField] SpriteRenderer playerOneExclaim;
    [SerializeField] SpriteRenderer playerTwoExclaim;

    void CheckIfMiss() {
        if (playerOne.leftPressed || playerOne.middlePressed || playerOne.rightPressed) {
            if (playerOneCooldownTimer > 0) return;
            // play stun animation
            playerOneExclaim.enabled = true;
            playerOneCooldownTimer = cooldown;
            gunClick.PlayOneShot(gunClick.clip);
            // enable exclamation mark over player head
        }

        if (playerTwo.leftPressed || playerTwo.middlePressed || playerTwo.rightPressed) {
            if (playerTwoCooldownTimer > 0) return;

            // play stun animation
            playerTwoCooldownTimer = cooldown;
            gunClick.PlayOneShot(gunClick.clip);
            playerTwoExclaim.enabled = true;
            // enable exclamation mark over player head
        }
    }


    void PlayerOneShoot() {
        if (playerOneCooldownTimer > 0) return;


        if (_canShoot == false) return;
        if (_haveShoot == true) return;

        if (playerOne.leftPressed || playerOne.middlePressed || playerOne.rightPressed) {
            Player1Wins();

            _haveShoot = true;
        }

    }

    void PlayerTwoShoot() {
        if (playerTwoCooldownTimer > 0) return;

        if (_canShoot == false) return;
        if (_haveShoot == true) return;

        if (playerTwo.leftPressed || playerTwo.middlePressed || playerTwo.rightPressed) {
            Player2Wins();

            _haveShoot = true;
        }
    }

    void CheckIfShoot() {
        PlayerOneShoot();
        PlayerTwoShoot();
    }

    void Update()
    {
        if (_haveShoot && _canShoot) return;
        // Timer Logic
        _endTimer += 1f*Time.deltaTime;
        if (_endTimer >= _timer) { _canShoot = true; }

        // Visual/Audio Feedback to SHOOT
        Indicator();

        if (!_canShoot && !_haveShoot) {
            CheckIfMiss();
        }


        CheckIfShoot();
        
        playerOneCooldownTimer -= Time.deltaTime;
        playerTwoCooldownTimer -= Time.deltaTime;


        if (playerOneCooldownTimer <= 0) {
            playerOneExclaim.enabled = false;
        }
        if (playerTwoCooldownTimer <= 0) {
            playerTwoExclaim.enabled = false;
        }
    }

    void Indicator()
    {
        if (_canShoot == true) {
            // play bell toll
            BGM.Stop();
            DOTween.Sequence()
                .AppendCallback(() => bellToll.PlayOneShot(bellToll.clip));
            indicator.SetText("12:00");
            Debug.Log("FIRE!");
        }
    }

    void Player1Wins()
    {
        Debug.Log("Player 1 Wins");
        //gunShot.panStereo = -1;
        /*gunShot.PlayOneShot(gunShot.clip);
        victorySting.PlayOneShot(victorySting.clip);
        _haveShoot = true;
        anim1.SetBool("Win",true);
        anim2.SetBool("Lose", true);*/
        Score.Instance.AddPlayer1Score();

        VictoryLogic(player1, player2, 1);
    }

    void Player2Wins()
    {
        Debug.Log("Player2Wins");
        //gunShot.panStereo = 1;
        /*_haveShoot = true;
        anim2.SetBool("Win", true);
        anim1.SetBool("Lose", true);*/

        Score.Instance.AddPlayer2Score();


        VictoryLogic(player2, player1, 2);
    }

    void Sparks(ParticleSystem[] ps) {
        foreach (ParticleSystem p in ps) { p.Play(); }
    }


    [SerializeField] Sprite deathSprite;
    [SerializeField] Sprite deathSprite2;
    void VictoryLogic(GameObject winnerGameObject, GameObject loserGameObject, int winner) {
        gunShot.Play();

        ParticleSystem[] ps = winnerGameObject.GetComponentsInChildren<ParticleSystem>();
        DOTween.Sequence()
            .AppendInterval(0.4f)
            .AppendCallback(() =>
                Sparks(ps)
            );
        

        //gunShot.Play();
        DOTween.Sequence()
            .AppendInterval(1f)
            .AppendCallback(() => victorySting.PlayOneShot(victorySting.clip))
            .AppendCallback(() => monkeySounds.PlayOneShot(monkeySounds.clip))
            .AppendInterval(5.0f)
            .AppendCallback(() => TransitionManager.instance.LoadScene(SceneReference.Score))
            ;

        winnerGameObject.GetComponent<Animator>().SetBool("Win", true);
        //loserGameObject.GetComponent<Animator>().SetBool("Lose", true);
        loserGameObject.GetComponent<Animator>().enabled = false;
        if (winner == 1) { 
            loserGameObject.GetComponentInChildren<SpriteRenderer>().sprite = deathSprite2;
        }
        else {
            loserGameObject.GetComponentInChildren<SpriteRenderer>().sprite = deathSprite;
        }

        LaunchMonkey(loserGameObject);

        CameraController cc = FindFirstObjectByType<CameraController>();
        cc.UnregisterPlayer(loserGameObject.transform);
        cc.UnregisterPlayer(winnerGameObject.transform);


    }

    void LaunchMonkey(GameObject loser) {
        Rigidbody2D rb = loser.GetComponent<Rigidbody2D>();
        rb.isKinematic = false;
        rb.AddForce(new Vector2(Random.Range(1,2), 10), ForceMode2D.Impulse);
        rb.AddTorque(10, ForceMode2D.Impulse);

        Rigidbody[] bananas = loser.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody banana in bananas) {
            banana.transform.parent = null;
            banana.isKinematic = false;

            banana.AddForce(new Vector3(rr(-1, 1), 5.0f, rr(-1, 1)), ForceMode.Impulse);
            banana.AddTorque(new Vector3(rr(-1, 1), rr(-1, 1), rr(-1, 1)), ForceMode.Impulse);
        }
    }

    float rr(float min, float max) {
        return Random.Range(min, max);
    }


    #region Inputs
    PlayerInputs playerOne;
    PlayerInputs playerTwo;
    public void SetPlayerInputs(int playerId, bool left, bool right, bool middle, bool leftHeld, bool rightHeld, bool middleHeld) {
        switch (playerId) {
            case 0:
                playerOne = new PlayerInputs(playerId, left, right, middle, leftHeld, rightHeld, middleHeld);
                break;
            case 1:
                playerTwo = new PlayerInputs(playerId, left, right, middle, leftHeld, rightHeld, middleHeld);
                break;
        }
    }
    #endregion
}
