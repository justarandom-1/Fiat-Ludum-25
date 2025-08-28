using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lr_LineController : MonoBehaviour
{

    private LineRenderer lr;
    private Transform[] points;
    // Start is called before the first frame update
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    public void SetUpLine(Transform[] points)
    {
        lr.positionCount = points.Length;

        this.points = points;
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < points.Length; i ++)
        {
            if(points[i] == null){
                Destroy(gameObject);
                return;
            }
            lr.SetPosition(i, points[i].position);
        }
    }
}
