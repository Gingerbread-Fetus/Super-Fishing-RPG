using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IVehicle 
{
    IInteractable NearbyInteractable { get; set; }

    void Disembark(Vector3 landPosition);
}
