using UnityEngine;

public class Graph : MonoBehaviour
{
    [SerializeField] 
    private Transform pointPrefab;
    
    [SerializeField, Range(10, 100)]
    private int resolution = 10;

    private void Awake()
    {
        float step = 2f / resolution;
        var position = Vector3.zero;
        var scale = Vector3.one * step;
        for (int i = 0; i < resolution; i++) {
            Transform point = Instantiate(pointPrefab, transform);
            position.x = (i + 0.5f) * step - 1f;
            position.y = Mathf.Pow(position.x, 3);
            point.localPosition = position;
            point.localScale = scale;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
