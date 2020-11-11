using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleOverlayGUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Escape()
    {
        //TODO clean up battle info...
        gameObject.SetActive(false);
    }
}
