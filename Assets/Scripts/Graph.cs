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
    
    FunctionLibrary.FunctionName transitionFunction;
    
    private Transform[] points;
    
    [SerializeField, Min(0f)]
    float functionDuration = 1f;
    
    [SerializeField, Min(0f)]
    float transitionDuration = 1f;
    
    float duration;
    
    bool transitioning;
    
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
        duration += Time.deltaTime;

        if (transitioning)
        {
            if (duration >= transitionDuration)
            {
                duration -= transitionDuration;
                transitioning = false;
            }
        }
        else
        {
            if (duration >= functionDuration) {
                duration -= functionDuration;
                transitioning = true;
                transitionFunction = function;
                PickNextFunction();
            }
        }
        
        if (transitioning) {
            UpdateFunctionTransition();
        }
        else {
            UpdateFunction();
        }
    }
    
    void PickNextFunction() {
        function = FunctionLibrary.GetNextFunctionName(function);
    }

    void UpdateFunction() {
        if (points.Length != resolution * resolution)
        {
            Resize();
        }
        
        float time = Time.time;
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);
        float step = 2f / resolution;

        float u;
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
    
    void UpdateFunctionTransition () {
        if (points.Length != resolution * resolution)
        {
            Resize();
        }
        
        FunctionLibrary.Function from = FunctionLibrary.GetFunction(transitionFunction);
        FunctionLibrary.Function to = FunctionLibrary.GetFunction(function);
        
        float progress = duration / transitionDuration;
        float time = Time.time;
        float step = 2f / resolution;

        float u;
        float v = 0.5f * step - 1f;
        for (int i = 0, x = 0, z = 0; i < points.Length; i++, x++) {
            if (x == resolution) {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            u = (x + 0.5f) * step - 1f;
            
            points[i].localPosition = FunctionLibrary.Morph(u, v, time, from, to, progress);
        }
    }
}
