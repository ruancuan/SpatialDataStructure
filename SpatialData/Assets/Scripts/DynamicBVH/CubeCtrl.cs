using KS.DynamicBVH;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCtrl : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    public DynamicBVH tree;
    public float moveSpeed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        dir= dir.normalized;
        this.transform.position = Vector3.Lerp(transform.position, transform.position + dir, Time.deltaTime * this.moveSpeed);
        if (tree != null) {
            tree.UpdateObject(this.gameObject);
        }
    }
}
