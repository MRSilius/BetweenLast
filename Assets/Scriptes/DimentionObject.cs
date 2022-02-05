using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimentionObject : MonoBehaviour
{
    public bool InDarkDimention;
    private bool _addingForce;
    [SerializeField] private float _currentForce;
    private DimentionChanger _changer;

    [SerializeField] private float _needForce;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Sprite _darkSprite;
    [SerializeField] private Sprite _whiteSprite;

    public Rigidbody2D Rigidbody;
    public Collider2D Collider;

    private void Awake()
    {
        _changer = FindObjectOfType<DimentionChanger>();
    }

    private void FixedUpdate()
    {
        
        if (_addingForce)
        {
            if (_currentForce < _needForce)
            {
                _currentForce += Time.fixedDeltaTime;
            }
            else
            {
                ChangeDimention();
                StopAddForce();
            }
        }
    }

    public void ChangeDimention()
    {
        if (InDarkDimention)
        {
            _sprite.sortingLayerName = "White";
            _sprite.sortingOrder = 7;
            _sprite.sprite = _darkSprite;
        }
        else
        {
            _sprite.sortingLayerName = "Dark";
            _sprite.sortingOrder = -3;
            _sprite.sprite = _whiteSprite;
        }
        _changer.ChangeObjectDimention(this);
        InDarkDimention = !InDarkDimention;
    }

    public void AddForce()
    {
        _addingForce = true;
    }

    public void StopAddForce()
    {
        _addingForce = false;
        _currentForce = 0;
    }
}
