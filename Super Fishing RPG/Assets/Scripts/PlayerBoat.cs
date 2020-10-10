using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoat : MonoBehaviour, IInteractable
{
    [SerializeField] Material selectedMaterial;
    public PlayerController interactingPlayer;
    Material defaultMaterial;
    SpriteRenderer spriteRenderer;
    bool isDocked = true;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Interact()
    {
        Debug.Log("Activate boat");
    }

    public void Highlight(bool isSelected)
    {
        if (isSelected)
        {
            spriteRenderer.material = selectedMaterial; 
        }
        else
        {
            spriteRenderer.material = defaultMaterial;
        }
    }
}
