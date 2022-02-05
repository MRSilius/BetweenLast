using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    [SerializeField] private Animation _animation;
    [SerializeField] private AnimationClip _clip;

    public void PlayAnimation()
    {
        _animation.clip = _clip;
        _animation.Play();
    }
}
