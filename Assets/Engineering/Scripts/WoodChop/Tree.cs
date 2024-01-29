using System.Collections.Generic;
using UnityEngine;

namespace Woodchop
{
    public class Tree
    {
        private Transform treeParent;
        private Queue<TreeSegment> segments;
        private WoodchopPlayer chopper;

        public Transform Parent => treeParent;
        public WoodchopPlayer Chopper => chopper;
        public Tree(Vector2 pos, Transform root, WoodchopPlayer chopper, bool chopperPositionToLeft) {
            treeParent = new GameObject("Tree Parent").transform;
            treeParent.parent = root;
            treeParent.localPosition = Vector2.zero + pos;

            segments = new Queue<TreeSegment>();

            this.chopper = chopper;
            Vector2 chopperPos = root.TransformPoint(pos);

            if (chopperPositionToLeft) {
                chopper.transform.position = new Vector2(chopperPos.x - 0.5f, chopperPos.y);
            }
            else {
                chopper.transform.position = new Vector2(chopperPos.x + 0.5f, chopperPos.y);
                chopper.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }

        public void EnqueueSegment(TreeSegment segment, float segmentHeight) {
            segments.Enqueue(segment);
            segment.Transform.parent = treeParent;
            segment.Transform.localPosition = new Vector3(0, segmentHeight, 0);
        }

        public TreeSegment DequeueSegment() {

            TreeSegment segment = segments.Dequeue();
            segment.Transform.parent = null;
            return segment;
        }

        public TreeSegment PeekSegment() {
            return segments.Peek();
        }

        public int GetSegmentCount() {
            return segments.Count;
        }
    }


    public struct TreeSegment
    {
        public TreeSolution Solution { get; private set; }
        public Transform Transform { get; private set; }
        public TreeSegment(TreeSolution solution, Transform transform) {
            this.Solution = solution;
            this.Transform = transform;
        }
    }

    public enum TreeSolution 
    {
        left,
        middle,
        right,
    }

}
