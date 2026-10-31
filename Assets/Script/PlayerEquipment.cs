using UnityEngine;
using System;

public class PlayerEquipment : MonoBehaviour
{
    private Animator animator;

    public bool HasPistol {get; private set;}

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void EquipPistol()
    {
        HasPistol = true;
        animator.SetBool("hasPistol", true);
        
    }

    public void UnequipPistol()
    {
        HasPistol = false;
        animator.SetBool("hasPistol", false);
    }
}
