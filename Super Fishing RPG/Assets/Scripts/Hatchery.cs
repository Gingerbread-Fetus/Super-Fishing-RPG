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
    [SerializeField]
    int maxFish = 5;
    [SerializeField]
    //TODO replace later with Scriptable Objects.
    List<FishController> fishStock; //Possible fish population.
    List<FishController> fishPopulation = new List<FishController>(); //Total Fish population

    Bounds m_Bounds = new Bounds(Vector3.zero, Vector3.one);
    int numberOfFish = 0;

    // Start is called before the first frame update
    void Start()
    {
        var newFish = Instantiate<FishController>(fishStock[0], transform.position, Quaternion.identity);
        newFish.HatcheryBounds = m_Bounds;
        fishPopulation.Add(newFish);
        newFish.LocalHatchery = this;
        newFish.transform.parent = transform;
    }

    // Update is called once per frame
    void Update()
    {    
    }
}
