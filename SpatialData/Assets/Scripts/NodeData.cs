using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace KS
{
    public enum Axis { 
        X,
        Y,
        Z
    }

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
            set {
                this.m_bounds = value;
            }
        }

        public bool CompareMinByAxis(NodeData nodeData, Axis axis) {
            switch (axis) {
                case Axis.X:
                    return bounds.min.x < nodeData.bounds.min.x;
                case Axis.Y:
                    return bounds.min.y < nodeData.bounds.min.y;
                case Axis.Z:
                    return bounds.min.z < nodeData.bounds.min.z;
            }
            return false;
        }
        public bool CompareMaxByAxis(NodeData nodeData, Axis axis)
        {
            switch (axis)
            {
                case Axis.X:
                    return bounds.max.x < nodeData.bounds.max.x;
                case Axis.Y:
                    return bounds.max.y < nodeData.bounds.max.y;
                case Axis.Z:
                    return bounds.max.z < nodeData.bounds.max.z;
            }
            return false;
        }

        public NodeData(Vector3 pos, Quaternion quaternion, Vector3 size) {
            this.pos = pos;
            this.quaternion = quaternion;
            this.size = size;
        }
        public NodeData(Bounds bounds) {
            this.pos = bounds.center;
            this.quaternion = Quaternion.identity;
            this.size = bounds.size;
            this.m_bounds = bounds;
        }
    }
}
