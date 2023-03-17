using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.DynamicBVH
{
    [System.Serializable]
    public class Node 
    {
        public AABB aabb;
        public Node left;
        public Node right;
        public Node parent;
        public GameObject gameObject;
        public bool isLeaf {

            get {
                return left == null && right == null;
            }
        }
        public Node() { }

        public Node(AABB aabb, GameObject obj) {
            this.aabb = aabb;
            this.gameObject = obj;
        }
        public void Draw()
        {
            if (aabb != null)
            {
                Gizmos.DrawWireCube(aabb.bounds.center, aabb.bounds.size);
            }
            left?.Draw();
            right?.Draw();
        }

        public void UpdateAABB() {
            if (isLeaf)
            {
                return;
            }
            else {
                if (left != null && right != null)
                {
                    aabb = AABB.CreateByAABB(left.aabb, right.aabb);
                }
                else if (left != null)
                {
                    aabb = new AABB(left.aabb.bounds);
                }
                else {
                    aabb = new AABB(right.aabb.bounds);
                }
            }
        }
    }

}