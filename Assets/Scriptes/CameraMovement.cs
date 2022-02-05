using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Transform _target;
    [SerializeField] private float _smooth;
    [SerializeField] private bool _gameStarted;

    public void StartGame()
    {
        _gameStarted = true;
    }

    void FixedUpdate()
    {
        if (!_gameStarted) return;

        Vector3 latePostion = new Vector3(_target.position.x, _target.position.y, transform.position.z);
        Vector3 smoothPosition = Vector3.Lerp(transform.position, latePostion + _offset, _smooth * Time.fixedDeltaTime);
        transform.position = smoothPosition;
    }
}
