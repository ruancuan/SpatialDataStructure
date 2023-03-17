using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.QuadTree
{
    public class Node : IShow
    {
        public Bounds bounds;
        public Node[] childrens;
        public int depth;
        public int maxDepth;

        private List<NodeData> list_node;
        private int childNum = 4;

        public Node(Bounds bounds, int depth, int maxDepth)
        {
            this.bounds = bounds;
            this.depth = depth;
            this.maxDepth = maxDepth;

            list_node = new List<NodeData>();
        }

        /// <summary>
        /// 子节点的初始化
        /// </summary>
        private void Split()
        {
            childrens = new Node[this.childNum];
            int idx = 0;
            for (int i = -1; i <= 1; i += 2)
            {
                for (int j = -1; j <= 1; j += 2)
                {
                    Vector3 center = bounds.center + new Vector3(bounds.size.x * 0.25f * i, 0, bounds.size.z * 0.25f * j);
                    Vector3 size = new Vector3(bounds.size.x * 0.5f, bounds.size.y, bounds.size.z * 0.5f);
                    Bounds bound = new Bounds(center, size);
                    childrens[idx++] = new Node(bound, depth + 1, maxDepth);
                }
            }
        }

        /// <summary>
        /// 插入物体
        /// </summary>
        /// <param name="nodeData"></param>
        public void Insert(NodeData nodeData)
        {
            if (depth < maxDepth && childrens == null)
            {
                Split();
            }
            if (childrens != null)
            {
                int num = 0;
                Node selectNode = null;
                for (int i = 0; i < this.childNum; i++)
                {
                    Node node = childrens[i];
                    //如何在和多个子节点相交，则放入自身下面
                    if (node.bounds.Intersects(nodeData.bounds))
                    {
                        num++;
                        selectNode = node;
                    }
                }
                if (num > 1)
                {
                    list_node.Add(nodeData);
                }
                else if (num == 1 && selectNode != null)
                {
                    selectNode.Insert(nodeData);
                }
                else
                {
                    LogManager.Instance.LogWarn("物体不在子节点内");
                }
            }
            else
            {
                list_node.Add(nodeData);
            }
        }

        public void Query(CheckNode checkNode, out List<Node> listNode, out List<NodeData> listData, QuadTreeCheckType type)
        {
            listNode = new List<Node>();
            listData = new List<NodeData>();

            if (childrens != null)
            {
                for (int k = 0; k < childNum; k++)
                {
                    Node node = childrens[k];
                    switch (type) {
                        case QuadTreeCheckType.Point:
                            if (node.bounds.Contains(checkNode.transform.position))
                            {
                                listNode.Add(node);
                                List<Node> nodeList;
                                List<NodeData> dataList;
                                node.Query(checkNode, out nodeList, out dataList,type);
                                listNode.AddRange(nodeList);
                                listData.AddRange(dataList);
                            }
                            break;
                        case QuadTreeCheckType.Bounds:
                            if (node.bounds.Intersects(checkNode.nodeData.bounds))
                            {
                                listNode.Add(node);
                                List<Node> nodeList;
                                List<NodeData> dataList;
                                node.Query(checkNode, out nodeList, out dataList, type);
                                listNode.AddRange(nodeList);
                                listData.AddRange(dataList);
                            }
                            break;
                    }
                }
            }

            for (int i = 0; i < list_node.Count; i++)
            {
                listData.Add(list_node[i]);
            }

        }

        private Color m_color = Color.white;
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
            Gizmos.color = v_Color;
            Gizmos.DrawWireCube(bounds.center, bounds.size * showScale);
        }
    }

}