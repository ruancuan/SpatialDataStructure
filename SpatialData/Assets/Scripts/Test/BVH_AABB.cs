using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.BVH
{
    public enum BVHCheckType { 
        Point,
        Bounds
    }
    public class BVH_AABB : MonoBehaviour
    {
        public Bounds bounds;
        public CheckNode checkTransform;
        public BVHCheckType checkType = BVHCheckType.Point;
        public int showDepth = 0;

        private BVHNode tree;
        [SerializeField]
        private int createNum = 10;

        private string parentName = "BVHTree";
        // Start is called before the first frame update
        void Start()
        {
            GameObject parent = GameObject.Find(parentName);
            if (parent == null)
            {
                parent = new GameObject(parentName);
            }
            List<NodeData> listData=new List<NodeData>();
            for (int k = 0; k < createNum; k++)
            {
                Vector3 pos = bounds.center + new Vector3(Random.Range(-bounds.size.x * 0.5f, bounds.size.x * 0.5f), Random.Range(-bounds.size.y * 0.5f, bounds.size.y * 0.5f), Random.Range(-bounds.size.z * 0.5f, bounds.size.z * 0.5f));
                NodeData nodeData = new NodeData(pos, Quaternion.identity, Vector3.one);
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.SetParent(parent.transform);
                cube.transform.position = pos;
                listData.Add(nodeData);
            }
            tree = new BVHNode(listData);
        }

        // Update is called once per frame
        private List<AABB> listNode;
        void Update()
        {
            if (checkTransform != null)
            {
                List<NodeData> listData;
                tree.Query(checkTransform, out listNode, out listData, checkType);
            }
        }


        private void OnDrawGizmos()
        {
            if (tree != null)
            {
                if (checkTransform != null)
                {
                    if (listNode != null)
                    {
                        for (int k = 0; k < listNode.Count; k++)
                        {
                            AABB node = listNode[k];
                            node.Draw(0);
                        }
                    }
                }
                else
                {
                    tree.Draw(0, showDepth);
                }

            }
            else
            {
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
        }
    }

}