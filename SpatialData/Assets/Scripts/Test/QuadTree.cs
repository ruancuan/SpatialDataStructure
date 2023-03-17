using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.QuadTree
{
    public enum QuadTreeCheckType { 
        Point,
        Bounds
    }
    public class QuadTree : MonoBehaviour
    {
        public Bounds bounds;
        public CheckNode checkTransform;
        public QuadTreeCheckType checkType=QuadTreeCheckType.Point;

        private Tree tree;
        [SerializeField]
        private int createNum = 10;

        private string parentName = "QuadTree";
        // Start is called before the first frame update
        void Start()
        {
            GameObject parent = GameObject.Find(parentName);
            if (parent == null) { 
                parent= new GameObject(parentName);
            }
            tree = new Tree(bounds);
            for (int k = 0; k < createNum; k++) {
                Vector3 pos = bounds.center + new Vector3(Random.Range(-bounds.size.x * 0.5f, bounds.size.x * 0.5f), 0, Random.Range(-bounds.size.z * 0.5f, bounds.size.z * 0.5f));
                NodeData nodeData=new NodeData(pos, Quaternion.identity,Vector3.one);
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.SetParent(parent.transform);
                cube.transform.position = pos;
                tree.Insert(nodeData);
            }
        }

        // Update is called once per frame
        private List<Node> listNode;
        void Update()
        {
            if (checkTransform != null)
            {
                List<NodeData> listData;
                //List<Node> listNode;
                tree.Query(checkTransform, out listNode, out listData, checkType);
            }
        }

        private void OnDrawGizmos()
        {
            if (tree != null)
            {
                if (listNode != null)
                {
                    for (int k = 0; k < listNode.Count; k++)
                    {
                        Node node = listNode[k];
                        node.Draw(0);
                    }
                }

            }
            else
            {
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
        }
    }

}