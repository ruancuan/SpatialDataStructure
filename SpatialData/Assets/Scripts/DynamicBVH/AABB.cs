using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KS.DynamicBVH
{
    [System.Serializable]
    public class AABB 
    {
        public Bounds bounds;
        public AABB(Bounds bounds) {
            this.bounds = bounds;
        }
        public AABB(Vector3 pos, Vector3 size) {
            this.bounds = new Bounds(pos, size);
        }

        /// <summary>
        /// 拓张
        /// </summary>
        /// <param name="other"></param>
        public void ExpandToInclude(AABB other)
        {
            bounds.Encapsulate(other.bounds);
        }

        public static AABB CreateByAABB(AABB ab1, AABB ab2) {
            AABB result = new AABB(ab1.bounds);
            result.ExpandToInclude(ab2);
            return result;
        }

        /// <summary>
        /// 计算表面积
        /// </summary>
        /// <returns></returns>
        public float GetSurfaceArea() {
            return 2 * (bounds.size.x * bounds.size.y + bounds.size.y * bounds.size.z + bounds.size.z * bounds.size.x);
        }
    }

}