using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorEvents : MonoBehaviour
{
    [SerializeField] private CharacterMovement _movement;

    public void WakeUp()
    {
        _movement.CanMove();
    }
    public void StopMovement()
    {
        _movement.StopMove();
    }
}
