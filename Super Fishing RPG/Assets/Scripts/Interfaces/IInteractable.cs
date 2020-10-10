using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    PlayerController InteractingPlayer { get; set; }

    void Interact();
    void Highlight(bool isSelected);
}
