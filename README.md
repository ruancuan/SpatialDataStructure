# SpatialDataStructure

## 介绍
空间数据结构

## 四叉树

<details>
<summary>图片</summary>

![Image text](/Show/QuadTree/show1.png)
![Image text](/Show/QuadTree/show2.png)

</details>


## BVH(层次包围盒树)
### AABB包围盒树
<details>
<summary>根据深度显示范围</summary>

![Image text](/Show/BVH_AABB/show1.jpg)
![Image text](/Show/BVH_AABB/show2.jpg)
![Image text](/Show/BVH_AABB/show3.jpg)
![Image text](/Show/BVH_AABB/show4.jpg)
![Image text](/Show/BVH_AABB/show5.jpg)

</details>

<details>
<summary>得到在哪些AABB内</summary>

![Image text](/Show/BVH_AABB/show6.png)
![Image text](/Show/BVH_AABB/show7.png)

</details>

### 动态AABB包围盒树
Dynamic BVH是BVH的动态版本，可以处理物体的动态移动。在每次物体移动后，需要重新构造BVH树，这可能会导致性能问题。为了解决这个问题，Dynamic BVH采用了一些优化策略，例如局部更新、增量构造等方法，以尽可能减少重构树的次数

#### AABB Tree
AABB Tree是一种基于轴对齐包围盒（AABB）的动态空间数据结构，可以处理物体的动态移动。在每次物体移动后，只需要更新其所在的节点和其父节点的包围盒即可，无需重构整个树。AABB Tree的优点是构造速度快，内存占用小，适用于处理大量物体的场景