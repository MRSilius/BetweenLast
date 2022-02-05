using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    [SerializeField]private List< Animator> _animators;

    public bool IsMoving { private get; set; }
    public bool IsFlying { private get; set; }
    public bool IsGrounded { private get; set; }
    public bool OnWall { private get; set; }

    private void Start()
    {
        _animators.AddRange(GetComponentsInChildren<Animator>());
    }

    private void Update()
    {
        foreach(Animator animator in _animators)
        {
            animator.SetBool("IsMoving", IsMoving);
            animator.SetBool("IsFlying", IsFlying);
            animator.SetBool("IsGrounded", IsGrounded);
            animator.SetBool("OnWall", OnWall);
        }
    }

    public void Hit()
    {
        foreach (Animator animator in _animators)
        {
            animator.SetTrigger("Hit");
        }
    }

    public void Jump()
    {
        foreach (Animator animator in _animators)
        {
            if (animator.GetBool("IsFlying") == false)
            {
                animator.SetTrigger("Jump");
            }
        }
    }

    public void GetNewSkill()
    {
        foreach (Animator animator in _animators)
        {
            animator.SetTrigger("GetNewSkill");
        }
    }
}
