using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace KS.QuadTree
{
    [Serializable]
    public class NodeData
    {
        /// <summary>
        /// 位置
        /// </summary>
        [SerializeField]
        public Vector3 pos;
        /// <summary>
        /// 旋转
        /// </summary>
        [SerializeField]
        public Quaternion quaternion;
        /// <summary>
        /// 面积
        /// </summary>
        [SerializeField]
        public Vector3 size;

        private Bounds m_bounds;
        public Bounds bounds
        {
            get {
                if (m_bounds == null) {
                    m_bounds = new Bounds();
                }
                m_bounds.center = pos;
                m_bounds.size = size;
                return m_bounds;
            }
            set { }
        }

        public NodeData(Vector3 pos, Quaternion quaternion, Vector3 size) {
            this.pos = pos;
            this.quaternion = quaternion;
            this.size = size;
        }
    }
}
