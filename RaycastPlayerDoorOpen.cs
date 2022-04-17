using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attach to players head (player on layer "Player")
public class Interact : MonoBehaviour
{
    [SerializeField] KeyCode interactionKey = KeyCode.E;
    [SerializeField] float interactingRange = 2;
    LayerMask layerMask;

    private void Start()
    {
        // layer mask that represents every layer except player
        layerMask = Physics.DefaultRaycastLayers & ~(1 << LayerMask.NameToLayer("Player"));
    }

    void Update()
    {
        if (Input.GetKeyDown(interactionKey))
        {
            AttemptInteraction();
        }
    }

    void AttemptInteraction()
    {
        var ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactingRange, layerMask))
        {
            var interactable = hit.collider.GetComponent<DoorInteract>();

            if (!ReferenceEquals(interactable, null))
            {
                interactable.Interact();
            }
        }
    }
}

DoorInteract.cs
###################################
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// attach to door with collider

[RequireComponent(typeof(Collider))]

public class DoorInteract : MonoBehaviour
{
    Animator anim;
    public bool dooropen = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Interact()
    {
        ToggleDoor();
    }

    void ToggleDoor()
    {
        if (dooropen)
        {
            DoorClose();
        }
        else
        {
            DoorOpen();
        }
    }

    public void DoorOpen()
    {
        dooropen = true;
        anim.Play("dooropen");
    }

    public void DoorClose()
    {
        dooropen = false;
        anim.Play("doorclose");
    }
}
