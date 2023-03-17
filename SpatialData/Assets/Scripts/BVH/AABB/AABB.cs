using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.BVH
{
    public class AABB:IShow
    {
        public Bounds bounds {

            get
            {
                return nodeData.bounds;
            }

        }

        private NodeData nodeData;
        public NodeData NodeData {
            get {
                return nodeData;
            }
        }

        public AABB(NodeData data) {
            this.nodeData = data;
        }
        public bool Check(Bounds bounds)
        {
            return this.bounds.Intersects(bounds);
        }

        public static AABB SurroundingBox(List<NodeData> nodeDataList)
        {
            Bounds bigBounds = new Bounds();
            if (nodeDataList != null) {
                for (int k = 0; k < nodeDataList.Count; k++) {
                    bigBounds.Encapsulate(nodeDataList[k].bounds);
                }
            }
            return new AABB(new NodeData(bigBounds));
        }

        private Color m_color = Color.red;
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
                Gizmos.DrawWireCube(bounds.center, bounds.size * showScale);
            }
        }

        //public void Query(CheckNode checkNode, out List<AABB> listNode, out List<NodeData> listData, BVHCheckType type)
        //{
        //    listNode = new List<AABB>();
        //    listData = new List<NodeData>();

        //    switch (type)
        //    {
        //        case BVHCheckType.Point:
        //            if (bounds.Contains(checkNode.transform.position))
        //            {
        //                listNode.Add(this);
        //                listData.Add(nodeData);
        //            }
        //            break;
        //        case BVHCheckType.Bounds:
        //            if (bounds.Intersects(checkNode.nodeData.bounds))
        //            {
        //                listNode.Add(this);
        //                listData.Add(nodeData);
        //            }
        //            break;
        //    }
        //}
    }

}