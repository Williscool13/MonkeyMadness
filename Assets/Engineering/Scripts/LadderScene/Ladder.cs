using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LadderClimb
{

    public class Ladder
    {
        private Queue<LadderSolution> inputs;
        private Transform ladderParent;
        private LadderPlayer climber;
        private GameObject toilet;
        public Transform LadderParent => ladderParent;
        public LadderPlayer Climber => climber;
        public GameObject Toilet => toilet;
        int playerId = -1;
        public Ladder(Vector2 pos, Transform root, LadderPlayer climber, GameObject toilet, int playerId) {
            ladderParent = new GameObject("Ladder Parent").transform;
            ladderParent.localPosition = Vector2.zero + pos;

            inputs = new Queue<LadderSolution>();
            this.climber = climber;
            ladderParent.parent = root;
            this.toilet = toilet;

            this.playerId = playerId;

        }



        public void AddSegment(int segmentCount, LadderSolution solution) {
            for (int i = 0; i < segmentCount; i++) {
                inputs.Enqueue(solution);
            }
        }

        public LadderSolution DequeueSegment() {
            return inputs.Dequeue();
        }

        public LadderSolution PeekSegment() {
            return inputs.Peek();
        }

        public int GetSegmentCount() {
            return inputs.Count;
        }
    }



    public enum LadderSolution
    {
        up,
        left,
        right,
    }
}