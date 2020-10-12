using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    PlayerController InteractingPlayer { get; set; }
    bool IsHighlighted { get; set; }

    void Interact();
}
