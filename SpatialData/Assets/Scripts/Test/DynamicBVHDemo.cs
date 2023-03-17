using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KS.DynamicBVH;

public class DynamicBVHDemo : MonoBehaviour
{
    private DynamicBVH bvh;
    [SerializeField]
    private List<GameObject> objects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        bvh = new DynamicBVH();
    }

    public int haveCreateNum = 0;

    public void AddObjToTree()
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
        haveCreateNum++;
        obj.name = haveCreateNum.ToString();
        obj.transform.position = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
        obj.transform.localScale = new Vector3(Random.Range(0.5f, 2f), Random.Range(0.5f, 2f), Random.Range(0.5f, 2f));
        objects.Add(obj);

        bvh.AddObject(obj);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            MoveObjects();
        }
    }

    private void MoveObjects() {
         foreach (GameObject obj in objects)
        {
            obj.transform.position += new Vector3(
                Random.Range(-2f, 2f), 
                Random.Range(-2f, 2f),
                Random.Range(-2f, 2f));
            bvh.UpdateObject(obj);
        }
    }
    private void OnDrawGizmos()
    {
        if (bvh != null && bvh.Root!=null)
        {
            bvh.Root.Draw();
        }
    }
}
