using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.DynamicBVH
{
    [System.Serializable]
    public class DynamicBVH
    {
        private Node root;
        public Node Root
        {
            get
            {
                return root;
            }
        }
        public DynamicBVH()
        {
            root = null;
        }

        private void InsertNode(Node node)
        {
            if (root == null)
            {
                root = node;
                return;
            }
            InsertNode(root, node);
        }

        private void InsertNode(Node node, Node newNode)
        {
            if (node.isLeaf)
            {
                Node newParent = new Node();
                newParent.parent = node.parent;

                newNode.parent = newParent;
                if (root == node)
                {
                    root = newParent;
                }
                else
                {
                    if (node.parent.left == node)
                    {
                        node.parent.left = newParent;
                    }
                    else if (node.parent.right == node)
                    {
                        node.parent.right = newParent;
                    }
                }
                node.parent = newParent;

                newParent.left = node;
                newParent.right = newNode;

                newParent.aabb = AABB.CreateByAABB(node.aabb, newNode.aabb);
                //更新父节点的aabb
                Node parent = newParent.parent;
                while (parent != null)
                {
                    parent.UpdateAABB();
                    parent = parent.parent;
                }

            }
            else if (node.left == null)
            {
                node.left = newNode;
                newNode.parent = node;
                //更新父节点的aabb
                Node parent = newNode.parent;
                while (parent != null)
                {
                    parent.UpdateAABB();
                    parent = parent.parent;
                }
            }
            else if (node.right == null) {
                node.right = newNode;
                newNode.parent = node;
                //更新父节点的aabb
                Node parent = newNode.parent;
                while (parent != null)
                {
                    parent.UpdateAABB();
                    parent = parent.parent;
                }
            }
            else
            {
                AABB combinedAABB = AABB.CreateByAABB(node.aabb, newNode.aabb);
                //float currentCost = 2 * node.aabb.GetSurfaceArea();
                float combinedCost = 2 * combinedAABB.GetSurfaceArea();

                float leftCost = AABB.CreateByAABB(node.left.aabb, newNode.aabb).GetSurfaceArea() + combinedCost - node.left.aabb.GetSurfaceArea();
                float rightCost = AABB.CreateByAABB(node.right.aabb, newNode.aabb).GetSurfaceArea() + combinedCost - node.right.aabb.GetSurfaceArea();

                if (leftCost < rightCost)
                {
                    InsertNode(node.left, newNode);
                }
                else
                {
                    InsertNode(node.right, newNode);
                }
            }
        }

        private void RemoveNode(Node node, GameObject obj)
        {
            if (node == null)
            {
                return;
            }
            if (node.gameObject == obj)
            {
                if (node.isLeaf)
                {
                    if (node == root)
                    {
                        root = null;
                        node.parent = null;
                        return;
                    }
                    else if (node.parent.left == node)
                    {
                        node.parent.left = null;
                    }
                    else
                    {
                        node.parent.right = null;
                    }
                }
                //更新AABB
                Node parent = node.parent;
                while (parent != null)
                {
                    if (parent.left != null && parent.right != null)
                    {
                        parent.aabb = AABB.CreateByAABB(parent.left.aabb, parent.right.aabb);
                    }
                    else if (parent.left != null)
                    {
                        parent.aabb = new AABB(parent.left.aabb.bounds);
                    }
                    else if (parent.right != null)
                    {
                        parent.aabb = new AABB(parent.right.aabb.bounds);
                    }
                    else {
                        //移除不包含GameObject的叶子节点
                        if (parent.parent!=null) {
                            if (parent.parent.left == parent)
                            {
                                parent.parent.left = null;
                            }
                            else {
                                parent.parent.right = null;
                            }
                        }
                    }
                    parent = parent.parent;
                }
            }
            else
            {
                if (node.isLeaf == false)
                {
                    RemoveNode(node.left, obj);
                    RemoveNode(node.right, obj);
                }
            }
        }

        private Node FindNode(Node node, GameObject obj)
        {
            if (node == null)
            {
                return null;
            }
            if (node.gameObject = obj)
            {
                return node;
            }
            Node left = FindNode(node.left, obj);
            if (left != null)
            {
                return left;
            }
            Node right = FindNode(node.right, obj);
            if (right != null)
            {
                return right;
            }
            return null;
        }

        private Node UpdateNode(Node node, Node updateNode)
        {
            if (node == null)
            {
                return null;
            }
            if (node == updateNode)
            {
                return node;
            }
            if (node.left == null && node.right == null)
            {
                return node;
            }
            if (node.left == updateNode || node.right == updateNode)
            {
                node.aabb.ExpandToInclude(updateNode.aabb);
                if (node.left != null)
                {
                    node.left = UpdateNode(node.left, updateNode);
                    node.aabb.ExpandToInclude(node.left.aabb);
                }
                if (node.right != null)
                {
                    node.right = UpdateNode(node.right, updateNode);
                    node.aabb.ExpandToInclude(node.right.aabb);
                }
            }
            else
            {
                if (node.left.aabb.bounds.Intersects(updateNode.aabb.bounds))
                {
                    node.left = UpdateNode(node.left, updateNode);
                    node.aabb.ExpandToInclude(node.left.aabb);
                }
                else if (node.right.aabb.bounds.Intersects(updateNode.aabb.bounds))
                {
                    node.right = UpdateNode(node.right, updateNode);
                    node.aabb.ExpandToInclude(node.right.aabb);
                }
                else
                {
                    node.left = UpdateNode(node.left, updateNode);
                    node.right = UpdateNode(node.right, updateNode);
                    node.aabb.ExpandToInclude(node.left.aabb);
                    node.aabb.ExpandToInclude(node.right.aabb);
                }
            }

            return node;
        }

        public void AddObject(GameObject obj)
        {
            AABB aabb = new AABB(obj.transform.position, obj.transform.localScale);
            Node newNode = new Node(aabb, obj);
            InsertNode(newNode);
        }

        public void RemoveObject(GameObject obj)
        {
            RemoveNode(root, obj);
        }

        public void UpdateObject(GameObject obj)
        {

            RemoveObject(obj);
            AddObject(obj);
            return;
            //Node node = FindNode(root, obj);
            //if (node != null)
            //{
            //    AABB aabb = new AABB(obj.transform.position, obj.transform.localScale);
            //    node.aabb = aabb;
            //    root = UpdateNode(root, node);
            //}
        }

        private void FindCollisions(Node node, Node checkNode, List<GameObject> collidingObjects)
        {
            if (node == null)
            {
                return;
            }
            if (node.aabb.bounds.Intersects(checkNode.aabb.bounds))
            {
                if (node.left == null && node.right == null)
                {
                    if (node.gameObject != null && node.gameObject != checkNode.gameObject)
                    {
                        collidingObjects.Add(node.gameObject);
                    }
                }
                else
                {
                    FindCollisions(node.left, checkNode, collidingObjects);
                    FindCollisions(node.right, checkNode, collidingObjects);
                }
            }
        }

        public List<GameObject> GetCollidingObjects(GameObject obj)
        {
            List<GameObject> collidingObjects = new List<GameObject>();
            AABB aabb = new AABB(obj.transform.position, obj.transform.localScale);
            Node checkNode = new Node(aabb, obj);
            FindCollisions(root, checkNode, collidingObjects);
            return collidingObjects;
        }

        public void Draw()
        {
            root?.Draw();
        }
    }

}