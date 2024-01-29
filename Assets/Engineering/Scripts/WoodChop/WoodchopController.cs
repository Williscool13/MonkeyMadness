using Cinemachine;
using DG.Tweening;
using ScriptableObjectDependencyInjection;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Woodchop
{
    public class WoodchopController : MonoBehaviour, IInputReceiver
    {

        [Title("Prefabs")]
        [SerializeField] GameObject treeSegment;
        [SerializeField] GameObject treeTop;
        [SerializeField] GameObject playerPrefab;

        [Title("Tree Properties")]
        [Range(5, 50)][SerializeField] int totalTreeCount = 20;
        [SerializeField] int treeDistance = 5;

        [Title("Player Properties")]
        [SerializeField] float distanceFromTree = 1f;
        [SerializeField] IntegerReference playerCount;

        [Title("Tree Launch Properties")]
        [SerializeField] Vector2 treeLaunchUpwardForceRange = new(6, 10);
        [SerializeField] Vector2 treeLaunchHorizontalForceRange = new(6, 10);
        [SerializeField] Vector2 treeLaunchTorqueRange = new(10, 20);
        [SerializeField] float treeFallSpeed = 0.35f;

        [Title("Camera Properties")]
        [SerializeField] CinemachineVirtualCamera cam;
        [SerializeField] IntegerVariable currentCameraPriority;


        [SerializeField] Sprite player2Sprite;


        Dictionary<int, PlayerInputs> playerInputmap;
        Dictionary<int, Tree> playerTreeMap;

        bool receivingInputs = false;
        bool gameOver;

        void Start() {
            playerInputmap = new Dictionary<int, PlayerInputs>();
            playerTreeMap = new Dictionary<int, Tree>();

            float midpoint = playerCount.Value/ 2f;

            for (int i = 0; i < playerCount.Value; i++) {
                float treeXPos = i * treeDistance;

                GameObject player = Instantiate(playerPrefab);
                player.transform.localPosition = new Vector2(i * treeDistance, 0);
                if (i == 1) {
                    player.GetComponentInChildren<SpriteRenderer>().sprite = player2Sprite;
                }


                bool left = i < midpoint;
                // create tree (base) and orient/position it and player
                Tree t = new Tree(new Vector2(treeXPos, 0), transform, player.GetComponent<WoodchopPlayer>(), left);
                cam.GetComponent<CameraController>().RegisterPlayer(player.transform);

                // set up player inputs
                PlayerInputs pi = new PlayerInputs(i, false, false, false, false, false, false);

                // set up dictionaries
                playerInputmap.Add(i, pi);
                playerTreeMap.Add(i, t);
            }
            List<Tree> trees = new();
            foreach (KeyValuePair<int, Tree> kvp in playerTreeMap) {
                trees.Add(kvp.Value);
            }
            GenerateAllTrees(trees.ToArray());
            StartInputs();

        }
        [SerializeField] AudioSource monkeySounds;
        [SerializeField] AudioSource BGM;
        [SerializeField] AudioSource victorySting;
        void Update() {
            if (gameOver) return;

            for (int i = 0; i < playerCount.Value; i++) {
                if (VictoryCheck(i)) {
                    gameOver = true;

                    foreach (KeyValuePair<int, Tree> kvp in playerTreeMap) {
                        cam.GetComponent<CameraController>().UnregisterPlayer(kvp.Value.Chopper.transform);
                    }

                    if (i == 0) {
                        Score.Instance.AddPlayer1Score();
                    }
                    else {
                        Score.Instance.AddPlayer2Score();
                    }

                    playerTreeMap.TryGetValue(i, out Tree tree);



                    DOTween.Sequence()
                        .AppendCallback(() => {
                            monkeySounds.Play();
                            BGM.Stop();
                            victorySting.Play();
                            tree.Chopper.SetAnimationTrigger("Win");
                            for (int j = 0; j < playerCount.Value; j++) {
                                if (i == j) continue;
                                playerTreeMap.TryGetValue(j, out Tree loserTree);
                                GameObject gob = loserTree.Chopper.gameObject;

                                ThrowPlayer(gob);
                            }
                        })
                        .AppendInterval(5f)
                        .AppendCallback(() => TransitionManager.instance.LoadScene(SceneReference.Score));


                    // play happy animation for winnder 
                    // throw loser out



                    return;
                }
            }

            for (int i = 0; i < playerCount.Value; i++) {
                ResolveInputs(i);
            }

        }

        void ThrowPlayer(GameObject player) {
            player.GetComponentInChildren<Animator>().enabled = false;

            Rigidbody[] rbs = player.GetComponentsInChildren<Rigidbody>();
            Rigidbody2D[] rb2ds = player.GetComponentsInChildren<Rigidbody2D>();

            foreach (Rigidbody rb in rbs) {
                rb.transform.parent = null;
                rb.isKinematic = false;
                rb.AddForce(new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 5f), 0) * 10f, ForceMode.Impulse);
                rb.AddTorque(new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), 0) * 10f, ForceMode.Impulse);
            }

            foreach (Rigidbody2D rb in rb2ds) {
                rb.transform.parent = null;
                rb.isKinematic = false;
                rb.AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(0f, 1f)), ForceMode2D.Impulse);
                rb.AddTorque(Random.Range(-1f, 1f) * 10f, ForceMode2D.Impulse);
            }
        }

        #region Game Logic
        bool VictoryCheck(int playerId) {
            if (playerInputmap.TryGetValue(playerId, out PlayerInputs pi)) {
                if (playerTreeMap.TryGetValue(playerId, out Tree tree)) {
                    if (tree.GetSegmentCount() == 0) {
                        Debug.Log("Player " + playerId + " wins!");
                        gameOver = true;
                        StopInputs();
                        return true;
                    } else {
                    }
                }
            }
            return false;
        }

        void ResolveInputs(int playerId) {
            if (!playerInputmap.TryGetValue(playerId, out PlayerInputs pi)) {
                return; }

            if (!playerTreeMap.TryGetValue(playerId, out Tree tree)) {
                return;
            }


            if (tree.Chopper.LockoutTimer > 0) {
                tree.Chopper.ReduceLockoutTimer(Time.deltaTime);
                return;
            } 
            
            TreeSegment s = tree.PeekSegment();
            switch (s.Solution) {
                case TreeSolution.left:
                    if (pi.leftPressed) {
                        Reward(tree);
                    } else {
                        if (pi.rightPressed || pi.middlePressed) {
                            Penalize(tree);
                        }
                    }
                    break;
                case TreeSolution.middle:
                    if (pi.middlePressed) {
                        Reward(tree);
                    } else {
                        if (pi.leftPressed || pi.rightPressed) {
                            Penalize(tree);
                        }
                    }
                    break;
                case TreeSolution.right:
                    if (pi.rightPressed) {
                        Reward(tree);
                    } else {
                        if (pi.leftPressed || pi.middlePressed) {
                            Penalize(tree);
                        }
                    }
                    break;

            }
        }

        void Reward(Tree tree) {
            LaunchTree(tree.DequeueSegment());
            tree.Parent.DOBlendableLocalMoveBy(new Vector3(0, -1, 0), 0.35f);
            tree.Chopper.SetAnimationTrigger("Smash");
            tree.Chopper.PlayHitParticleEffect();
            tree.Chopper.PlaySwingSound();
            tree.Chopper.PlayHitSound();
        }
    

        void LaunchTree(TreeSegment segment) {
            segment.Transform.parent = null;
            Rigidbody rb = segment.Transform.GetComponent<Rigidbody>();
            //Rigidbody2D ts = segment.Transform.GetComponent<Rigidbody2D>();
            //ts.isKinematic = false;
            rb.isKinematic = false;


            float xDir = Random.Range(-1f, 1f);
            float xForce = xDir * Random.Range(treeLaunchHorizontalForceRange.x, treeLaunchHorizontalForceRange.y);
            float yForce = Random.Range(treeLaunchUpwardForceRange.x, treeLaunchUpwardForceRange.y);

            //ts.AddForce(new Vector2(xForce, yForce), ForceMode2D.Impulse);
            rb.AddForce(new Vector2(xForce, yForce), ForceMode.Impulse);

            //float torque = xDir * Random.Range(treeLaunchTorqueRange.x, treeLaunchTorqueRange.x);
            //ts.AddTorque(torque, ForceMode2D.Impulse);
            Vector3 torque = new Vector3(xDir * Random.Range(treeLaunchTorqueRange.x, treeLaunchTorqueRange.x), xDir * Random.Range(treeLaunchTorqueRange.x, treeLaunchTorqueRange.x), xDir * Random.Range(treeLaunchTorqueRange.x, treeLaunchTorqueRange.x));
            rb.AddTorque(torque, ForceMode.Impulse);

            Destroy(segment.Transform.gameObject, 8f);
        }

        void Penalize(Tree tree) {
            tree.Chopper.SetLockoutTimer(0.25f);
            tree.Chopper.SetAnimationTrigger("Miss");
            tree.Chopper.PlayMissParticleEffect();
            Debug.Log("WRONG");
            tree.Chopper.PlaySwingSound();

        }


        #endregion



        #region Generate Trees
        void GenerateAllTrees(Tree[] trees) {
            Debug.Log("Trees length = " + trees.Length);
            for (int i = 0; i < trees.Length; i++) {
                GenerateTree(trees[i]);
            }
        }
        void GenerateTree(Tree tree) {
            int currTreeCount = 0;
            while (currTreeCount < totalTreeCount) {
                TreeSolution sol = (TreeSolution)Random.Range(0, 3);

                TreeSegment tarSeg;

                if (currTreeCount == totalTreeCount - 1) {
                    // add tree top
                    GameObject tt = Instantiate(treeTop);
                    tt.transform.localPosition = new Vector3(0, currTreeCount, 0f);

                    tarSeg = new TreeSegment(sol, tt.transform);
                }
                else {
                    GameObject ts = Instantiate(treeSegment);

                    tarSeg = new TreeSegment(sol, ts.transform);
                }


                tarSeg.Transform.GetChild(0).rotation = sol switch {
                    TreeSolution.left => Quaternion.Euler(0, 0, 90),
                    TreeSolution.middle => Quaternion.Euler(0, 0, 0),
                    TreeSolution.right => Quaternion.Euler(0, 0, -90),
                    _ => Quaternion.Euler(0, 0, 0),
                };

                tree.EnqueueSegment(tarSeg, currTreeCount);


                currTreeCount++;
            }
        }
        #endregion

        #region Camera
        public void SetCameraPriority() {
            // replace with dollie code
            currentCameraPriority.Value += 1;
            cam.Priority = currentCameraPriority.Value;
        }
        #endregion

        #region Inputs
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
        #endregion

        public void SetPlayerInputs(int playerId, bool left, bool right, bool middle, bool leftHeld, bool rightHeld, bool middleHeld) {
            if (playerInputmap.TryGetValue(playerId, out PlayerInputs pi)) {
                pi.SetInputs(left, right, middle, leftHeld, rightHeld, middleHeld);
                playerInputmap[playerId] = pi;
            }
            else {
                Debug.Log("Player " + playerId + " not set up");
            }
        }

        struct TreeInformation
        {
            public Transform treeParent;
            public List<TreeSolution> treeSolution;
            public List<Transform> treeSegments;
            public TreeInformation(Transform treeParent, List<TreeSolution> treeSolution, List<Transform> treeSegments) {
                this.treeParent = treeParent;
                this.treeSolution = treeSolution;
                this.treeSegments = treeSegments;
            }
        }


    }

}