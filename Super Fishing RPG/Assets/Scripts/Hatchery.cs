using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Hatchery : MonoBehaviour
{
    public Bounds bounds {  get { return m_Bounds; } set { m_Bounds = value; } }
    public Vector2 Extents { get => extents; set => extents = value; }
    public List<FishController> FishPopulation { get => fishPopulation; }

    [SerializeField]
    Vector2 extents = Vector2.one;
    [SerializeField]
    int maxFish = 5;
    [SerializeField]
    bool useConstantSpawnRate = false;
    [SerializeField]
    float spawnRate = 2.0f;
    [SerializeField] //TODO replace later with Scriptable Objects.
    List<FishController> fishStock = new List<FishController>(); //Possible fish population.
    List<FishController> fishPopulation = new List<FishController>(); //Total Fish population

    Bounds m_Bounds;
    int numberOfFish = 0;

    private void OnEnable()
    {
        m_Bounds = new Bounds(transform.position, extents);
    }

    // Start is called before the first frame update
    void Start()
    {
        bounds = new Bounds(transform.position, extents);
        Debug.Log("Hatchery bounds are: " + bounds);
        for (int i = 0; i < maxFish; i++)
        {
            SpawnNewFish();
        }
        numberOfFish = fishPopulation.Count;
    }
    
    // Update is called once per frame
    void Update()
    {
        if(numberOfFish < maxFish)
        {
            numberOfFish++;
            float nextSpawnTime = Random.Range(0.1f, spawnRate);
            if (!useConstantSpawnRate)
            {
                Invoke("SpawnNewFish", nextSpawnTime); 
            }
            else
            {
                Invoke("SpawnNewFish", spawnRate);
            }
        }
    }

    private void SpawnNewFish()
    {
        var newFish = Instantiate<FishController>
            (
                fishStock[Random.Range(0, fishStock.Count)],
                transform.position, Quaternion.identity
            );
        fishPopulation.Add(newFish);
        newFish.LocalHatchery = this;
        newFish.transform.parent = transform;
    }

    public void RemoveFish(FishController fish)
    {
        fishPopulation.Remove(fish);
        numberOfFish--;
    }
}
