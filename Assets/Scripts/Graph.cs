using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] 
    private Transform pointPrefab;
    
    [SerializeField, Range(10, 100)]
    private int resolution = 10;
    
    [SerializeField]
    FunctionLibrary.FunctionName function;
    
    private Transform[] points;
    
    void Resize()
    {
        if (points != null && points.Length == resolution * resolution)
        {
            return;
        }
        
        if (points != null)
        {
            foreach (var point in points)
            {
                Destroy(point.gameObject);
            }
        }
        
        points = new Transform[resolution * resolution];
        
        float step = 2f / resolution;
        var scale = Vector3.one * step;
        for (int i = 0; i < points.Length; i++) {
            Transform point = points[i] = Instantiate(pointPrefab, transform);
            point.localScale = scale;
        }
    }

    void Awake()
    {
        Resize();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (points.Length != resolution * resolution)
        {
            Resize();
        }
        
        float time = Time.time;
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        float step = 2f / resolution;
        
        float u = 0.5f * step - 1f;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            if (x == resolution) {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            u = (x + 0.5f) * step - 1f;
            
            points[i].localPosition = f(u, v, time);
        }
    }
}
