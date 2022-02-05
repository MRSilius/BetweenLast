using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    private Checkpoints _checkpoints;
    private void Start()
    {
        _checkpoints = FindObjectOfType<Checkpoints>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out CharacterEvents e))
        {
            _checkpoints.RespawnPlayer();
        }
    }
    
}
