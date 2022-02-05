using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private Transform _point;
    [SerializeField] private BoxCollider2D _col;
    private Checkpoints _checkpoints;
    private void Start()
    {
        _checkpoints = FindObjectOfType<Checkpoints>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _checkpoints.SetSpawnPosition(_point);
        _col.enabled = false;
    }
}
