using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoints : MonoBehaviour
{
    [SerializeField] private Vector2 _spawnPosition;

    public void SetSpawnPosition(Transform transform)
    {
        _spawnPosition = transform.position;
    }

    public void RespawnPlayer()
    {
        transform.position = _spawnPosition;
    }
}
