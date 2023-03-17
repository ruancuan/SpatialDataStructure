using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.QuadTree
{
    public class CheckNode : MonoBehaviour
    {
        [SerializeField]
        private NodeData m_nodeData;
        public NodeData nodeData
        {
            get {
                m_nodeData.pos = transform.position;
                m_nodeData.size = transform.localScale;
                return m_nodeData;
            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(nodeData.bounds.center, nodeData.bounds.size);
        }
    }
}
