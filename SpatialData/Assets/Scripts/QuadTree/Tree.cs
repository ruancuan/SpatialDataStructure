using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.QuadTree
{
    public class Tree : IShow
    {
        public Bounds bounds { get; private set; }

        private int maxDepth = 5;
        private int maxChild = 4;
        private Node root;


        public Tree(Bounds bounds)
        {
            this.bounds = bounds;
            root = new Node(bounds, 0, maxDepth);
        }

        public void Insert(NodeData data)
        {
            root?.Insert(data);
        }


        public void Query(CheckNode checkNode, out List<Node> listNode, out List<NodeData> listData,QuadTreeCheckType type)
        {
            listNode = new List<Node>();
            listData = new List<NodeData>();

            root.Query(checkNode, out listNode,out listData, type);
        }

        public void Draw(int depth = 0, int showDepth = 0)
        {
            root?.Draw(depth);
        }
    }

}