using Cinemachine;
using DG.Tweening;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LadderClimb
{
    public class LadderController : MonoBehaviour, IInputReceiver
    {
        [Title("Prefabs")]
        [SerializeField] GameObject ladderPrefab;
        [SerializeField] GameObject platformPrefab;
        [SerializeField] GameObject finalPlatform;
        [SerializeField] GameObject platformCeiling;
        [SerializeField] GameObject ladderSideBeams;
        [SerializeField] GameObject playerPrefab;

        [Title("Camera Properties")]
        [SerializeField] CinemachineVirtualCamera cam;
        [SerializeField] IntegerVariable currentCameraPriority;

        [Title("Player Properties")]
        [SerializeField] float playerMoveSpeed = 0.35f;
        [SerializeField] IntegerReference playerCount;

        [Title("Ladder Properties")]
        [SerializeField] int ladderTotalLength = 100;
        [Range(3, 9)][SerializeField] int ladderSegmentMinLength = 5;
        [Range(10, 20)][SerializeField] int ladderSegmentMaxLength = 12;
        [SerializeField] float ladderDistance = 5f;
        [SerializeField] float platformWidth = 4f;

        [SerializeField] bool penalize = false;


        Dictionary<int, PlayerInputs> playerInputMap;
        Dictionary<int, Ladder> playerLadderMap;

        bool receivingInputs = false;
        bool gameOver = false;

        // Start is called before the first frame update
        void Start() {


            playerInputMap = new Dictionary<int, PlayerInputs>();
            playerLadderMap = new Dictionary<int, Ladder>();

            for (int i = 0; i < playerCount.Value; i++) {
                float ladderXpos = i * ladderDistance;

                GameObject player = Instantiate(playerPrefab);
                player.transform.localPosition = new Vector3(ladderXpos, 0, -2);
                if (i == 1) {
                    player.GetComponent<LadderPlayer>().SetAnimationBool("PlayerTwo", true);
                }

                player.GetComponent<LadderPlayer>().SetAnimationTrigger("Init");



                GameObject toilet = Instantiate(toiletPrefab, transform);
                toilet.transform.localPosition = new Vector3(ladderXpos, ladderTotalLength, -0.42f);


                Ladder l = new Ladder(new Vector2(ladderXpos, 0), transform, player.GetComponent<LadderPlayer>(), toilet, i);

            
                PlayerInputs pi = new PlayerInputs(i, false, false, false, false, false, false);


                playerInputMap.Add(i, pi);
                playerLadderMap.Add(i, l);
            }

            for (int i = 0; i < playerCount.Value; i++) {
                
            }


            List<Ladder> ladders = new List<Ladder>();
            foreach (KeyValuePair<int, Ladder> kvp in playerLadderMap) {
                ladders.Add(kvp.Value);
            }

            GenerateAllLadders(ladders.ToArray());
            GenerateTop();


            CameraController cc = cam.GetComponent<CameraController>();
            foreach (KeyValuePair<int, Ladder> kvp in playerLadderMap) {
                Debug.Log("added");
                cc.RegisterPlayer(kvp.Value.Climber.transform);
            }
            StartInputs();
        }

        [SerializeField] GameObject toiletPrefab;
        [SerializeField] AudioSource toiletSound;
        [SerializeField] AudioSource toiletCloseDoorSound;
        [SerializeField] AudioSource monkeySounds;
        private void Update() {
            if (gameOver) return;

            for (int i=0; i < playerCount.Value; i++) {
                if (VictoryCheck(i)) {
                    if (i == 0) {
                        Score.Instance.AddPlayer1Score();
                    }
                    else {
                        Score.Instance.AddPlayer2Score();
                    }


                    gameOver = true;
                    // sequence the orangutan celebrating, the door closing as the orangutan gets in, and a brief wait before moving on to the next scene
                    DOTween.Sequence()
                        .AppendCallback(() => {
                            playerLadderMap[i].Climber.SetAnimationTrigger("Victory");
                            monkeySounds.Play();
                            BGM.Stop();
                            victorySting.Play();
                        })
                        .AppendInterval(1.3f)
                        .AppendCallback(() => {
                            playerLadderMap[i].Climber.transform.DOBlendableMoveBy(new Vector3(0, 0, 1.75f), 1f);

                        })
                        .AppendInterval(0.5f)
                        .AppendCallback(() => {
                            playerLadderMap[i].Toilet.GetComponentInChildren<Animator>().SetTrigger("Close");
                            toiletCloseDoorSound.Play();
                        })
                        .AppendInterval(0.4f)
                        .AppendCallback(() => { toiletSound.Play(); })
                        .AppendInterval(3.0f)
                        .AppendCallback(() => {
                            TransitionManager.instance.LoadScene(SceneReference.Score);

                            //TransitionManager.Instance.GameOverLoadNextScene();
                        });

                    return;
                }
            }


            for (int i = 0; i < playerCount.Value; i++) {
                ResolveInputs(i);
            }
        }

        [SerializeField] AudioSource BGM;
        [SerializeField] AudioSource victorySting;


        bool VictoryCheck(int playerId) {
            if (playerInputMap.TryGetValue(playerId, out PlayerInputs pi)) {
                if (playerLadderMap.TryGetValue(playerId, out Ladder ladder)) {
                    if (ladder.GetSegmentCount() == 0) {
                        Debug.Log("Player " + playerId + " wins!");
                        gameOver = true;
                        StopInputs();

                        return true;
                    }
                    else {
                    }
                }
            }
            return false;
        }

        void ResolveInputs(int playerId) {
            if (!playerInputMap.TryGetValue(playerId, out PlayerInputs pi)){
                return;
            }

            if (!playerLadderMap.TryGetValue(playerId, out Ladder ladder)) {
                return;
            }

            if (ladder.Climber.LockoutTimer > 0) {
                ladder.Climber.ReduceLockoutTimer(Time.deltaTime);
                return;
            }

            LadderSolution s = ladder.PeekSegment();
            if (playerId == 0) {
                Debug.Log("Solution is: " + s);
            }
            switch (s) {
                case LadderSolution.left:
                    if (pi.leftPressed) {
                        Reward(ladder);
                    }
                    else {
                        if (pi.rightPressed || pi.middlePressed) {
                            Punish(ladder);
                        }
                    }
                    break;
                case LadderSolution.right:
                    if (pi.rightPressed) {
                        Reward(ladder);
                    }
                    else {
                        if (pi.leftPressed || pi.middlePressed) {
                            Punish(ladder);
                        }
                    }
                    break;
                case LadderSolution.up:
                    if (pi.middlePressed) {
                        Reward(ladder);
                    }
                    else {
                        if (pi.leftPressed || pi.rightPressed) {
                            Punish(ladder);
                        }
                    }
                    break;
            }
        }

        void Reward(Ladder ladder) {
            LadderSolution sol = ladder.DequeueSegment();

            switch (sol){
                case LadderSolution.left:
                    ladder.Climber.transform.DOBlendableMoveBy(new Vector3(-1, 0, 0), playerMoveSpeed);
                    ladder.Climber.FaceLeft();
                    break;
                case LadderSolution.right:
                    ladder.Climber.transform.DOBlendableMoveBy(new Vector3(1, 0, 0), playerMoveSpeed);
                    ladder.Climber.FaceRight();
                    break;
                case LadderSolution.up:
                    ladder.Climber.transform.DOBlendableMoveBy(new Vector3(0, 1, 0), playerMoveSpeed);
                    ladder.Climber.StarEffect();
                    break;
            }

            if (ladder.PeekSegment() == LadderSolution.up) {
                ladder.Climber.SetAnimationBool("Climb", true);
                ladder.Climber.SetAnimationTrigger("ClimbSingle");
            } else {
                ladder.Climber.SetAnimationBool("Climb", false);

            }
            ladder.Climber.transform.localPosition = new Vector3(ladder.Climber.transform.localPosition.x, ladder.Climber.transform.localPosition.y, -2);
        }

        void Punish(Ladder ladder) {
            if (!penalize) return;
            ladder.Climber.SetLockoutTimer(1f);
            //ladder.Climber.SetAniamtionTrigger("Punish");
        }

        public void StartInputs() {
            // replace with dollie code
            InputMaster.Instance.RegisterInputReceiver(this);
            receivingInputs = true;
        }

        public void StopInputs() {
            InputMaster.Instance.UnregisterInputReceiver(this);
            receivingInputs = false;
        }

        private void OnDestroy() {
            if (receivingInputs) {
                StopInputs();
            }
        }

        void SetCameraPriority() {
            // replace with dollie code
            currentCameraPriority.Value += 1;
            cam.Priority = currentCameraPriority.Value;
        }

        void GenerateLadder(Ladder ladder) {
            LadderPosition currLadderPos = LadderPosition.middle;

            int ladderLeft = ladderTotalLength;

            while (ladderLeft > 0) {
                int segmentLength = Random.Range(ladderSegmentMinLength, ladderSegmentMaxLength);
                segmentLength = Mathf.Min(segmentLength, ladderLeft);
                ladderLeft -= segmentLength;

                ladder.AddSegment(segmentLength, LadderSolution.up);

                LadderPosition nextLadderPos;
                if (ladderLeft == 0) {
                    nextLadderPos = currLadderPos;
                }
                else {
                    do {
                        nextLadderPos = (LadderPosition)Random.Range(0, 3);
                    } while (nextLadderPos == currLadderPos);
                }
                int diff = (int)nextLadderPos - (int)currLadderPos;

                float baseYPos = ladderTotalLength - ladderLeft - segmentLength;
                float xPos = currLadderPos == LadderPosition.left ? -1 : currLadderPos == LadderPosition.right ? 1 : 0;

                for (int i=0;i< segmentLength; i++) {
                    GameObject l = Instantiate(ladderPrefab, ladder.LadderParent);

                    float _yPos = baseYPos + i;

                    l.transform.localPosition = new Vector3(xPos, _yPos, 0);

                }
                /*GameObject l = Instantiate(ladderPrefab, ladder.LadderParent);
                float xPos = currLadderPos == LadderPosition.left ? -1 : currLadderPos == LadderPosition.right ? 1 : 0;

                float yPos = (ladderTotalLength - (ladderLeft + segmentLength));
                yPos += segmentLength / 2f;
                SpriteRenderer sr = l.GetComponent<SpriteRenderer>();
                sr.size = new Vector2(sr.size.x, segmentLength);


                l.transform.localPosition = new Vector3(xPos, yPos, 0);
                */

                float endYPos = ladderTotalLength - ladderLeft;
                if (ladderLeft != 0) {
                    Instantiate(platformPrefab, ladder.LadderParent).transform.localPosition = new Vector3(0, endYPos, -0.42f);
                }

                currLadderPos = nextLadderPos;
                while (diff != 0) {
                    if (diff > 0) {
                        ladder.AddSegment(1, LadderSolution.right);
                        diff--;
                    }
                    else {
                        ladder.AddSegment(1, LadderSolution.left);
                        diff++;
                    }
                }

                if (ladderLeft == 0) {
                    if (nextLadderPos != LadderPosition.middle) {
                        int diff2 = (int)LadderPosition.middle - (int)nextLadderPos;
                        while (diff2 != 0) {
                            if (diff2 > 0) {
                                ladder.AddSegment(1, LadderSolution.right);
                                diff2--;
                            }
                            else {
                                ladder.AddSegment(1, LadderSolution.left);
                                diff2++;
                            }
                        }
                    }
                }


            }

        



        }

        void GenerateTop() {

            GameObject gob = Instantiate(finalPlatform, transform);
            gob.transform.localPosition = new Vector3((playerCount.Value * platformWidth) / 2, ladderTotalLength , -0.42f);

            gob = Instantiate(platformCeiling, transform);
            gob.transform.localPosition = new Vector3((playerCount.Value * platformWidth) / 2, ladderTotalLength, -0.50f);

            for (int i=0;i<ladderTotalLength;i++) {
                GameObject l = Instantiate(ladderSideBeams, transform);
                l.transform.localPosition = new Vector3((platformWidth * playerCount.Value) / 2, i, -0.50f);
            }
        }


        void GenerateAllLadders(Ladder[] ladders) {
            for (int i = 0; i < ladders.Length; i++) {
                GenerateLadder(ladders[i]);
            }

        }



        public void SetPlayerInputs(int playerId, bool left, bool right, bool middle, bool leftHeld, bool rightHeld, bool middleHeld) {
            playerInputMap[playerId] = new PlayerInputs(playerId, left, right, middle, leftHeld, rightHeld, middleHeld);
        }

        enum LadderPosition
        {
            left,
            middle,
            right,
        }
         

    }



}
