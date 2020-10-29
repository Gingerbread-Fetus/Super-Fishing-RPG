using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hatchery : MonoBehaviour
{
    public Bounds bounds {  get { return m_Bounds; } set { m_Bounds = value; } }
    public Vector2 Extents { get => extents; set => extents = value; }

    [SerializeField]
    Vector2 extents = Vector2.one;
    Bounds m_Bounds = new Bounds(Vector3.zero, Vector3.one);

    // Start is called before the first frame update
    void Start()
    {   
    }

    // Update is called once per frame
    void Update()
    {    
    }
}
