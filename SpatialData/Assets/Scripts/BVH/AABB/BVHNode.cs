using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.BVH
{
    public class BVHNode:IShow
    {
        public AABB aabb
        {
            get;
            private set;
        }

        public BVHNode left;
        public BVHNode right;

        public BVHNode(List<NodeData> objects) {
            int axis = Random.Range(0, 3);

            objects.Sort((a, b) =>
            {
                return a.CompareMinByAxis(b, (Axis)axis)?-1:1;
            });

            int num = objects.Count;
            if (num <= 4)
            {
                left = right = null;
                aabb = AABB.SurroundingBox(objects);
            }
            else {
                left = new BVHNode(objects.GetRange(0, num / 2));
                right = new BVHNode(objects.GetRange(num / 2, num - num / 2));
                aabb = AABB.SurroundingBox(objects);
            }

        }

        private Color m_color = Color.blue;
        public Color v_Color
        {
            get
            {
                return m_color;
            }
            set
            {
                m_color = value;
            }
        }
        private float showScale = 1f;

        public void Draw(int depth = 0, int showDepth = 0)
        {
            if (depth == showDepth)
            {
                Gizmos.color = v_Color;
                Gizmos.DrawWireCube(aabb.bounds.center, aabb.bounds.size * showScale);
            }
            left?.Draw(depth+1,showDepth);
            right?.Draw(depth+1,showDepth);
        }

        public void Query(CheckNode checkNode, out List<AABB> listNode, out List<NodeData> listData, BVHCheckType type)
        {
            listNode = new List<AABB>();
            listData = new List<NodeData>();

            switch (type)
            {
                case BVHCheckType.Point:
                    if (aabb.bounds.Contains(checkNode.transform.position))
                    {
                        listNode.Add(aabb);
                        listData.Add(aabb.NodeData);
                        List<AABB> nodeList = new List<AABB>();
                        List<NodeData> dataList = new List<NodeData>();

                        if (left != null)
                        {
                            left.Query(checkNode, out nodeList, out dataList, type);
                            listNode.AddRange(nodeList);
                            listData.AddRange(dataList);
                        }
                        if (right != null)
                        {
                            right.Query(checkNode, out nodeList, out dataList, type);
                            listNode.AddRange(nodeList);
                            listData.AddRange(dataList);
                        }
                    }
                    break;
                case BVHCheckType.Bounds:
                    if (aabb.bounds.Intersects(checkNode.nodeData.bounds))
                    {
                        listNode.Add(aabb);
                        listData.Add(aabb.NodeData);
                        List<AABB> nodeList = new List<AABB>();
                        List<NodeData> dataList = new List<NodeData>();

                        if (left!=null)
                        {
                            left.Query(checkNode, out nodeList, out dataList, type);
                            listNode.AddRange(nodeList);
                            listData.AddRange(dataList);
                        }
                        if (right != null)
                        {
                            right.Query(checkNode, out nodeList, out dataList, type);
                            listNode.AddRange(nodeList);
                            listData.AddRange(dataList);
                        }
                    }
                    break;
            }
        }
    }

}