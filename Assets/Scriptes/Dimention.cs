using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dimention : MonoBehaviour
{
    [SerializeField] private Transform _demention;
    [SerializeField] private float _maxScale;

    void Update()
    {
        if(_demention.localScale.x < _maxScale)
        {
            _demention.localScale += Vector3.one * Time.deltaTime * 5;
        }
    }
}
