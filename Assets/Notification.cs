using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Notification : MonoBehaviour
{
    [SerializeField] private float _timeToDissapear;

    [SerializeField] private Color _white;
    [SerializeField] private Color _transparent;

    [SerializeField] private float _dissapearSpeed;
    [SerializeField] private float _currentTimeToDissapear;
    [SerializeField] private bool _isActive;

    [SerializeField] private List<TextMeshPro> _text;
    [SerializeField] private List<SpriteRenderer> _sprites;

    [SerializeField] public UnityEvent Event;

    private void Start()
    {
        _text.AddRange(GetComponentsInChildren<TextMeshPro>());
        _sprites.AddRange(GetComponentsInChildren<SpriteRenderer>());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _isActive = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        _isActive = true;
    }

    private void Update()
    {
        if (_isActive)
        {
            _currentTimeToDissapear = _timeToDissapear;
            foreach (TextMeshPro tmp in _text)
            {
                tmp.color = Color.Lerp(tmp.color, _white, Time.deltaTime * _dissapearSpeed * 2);
            }
            foreach (SpriteRenderer spr in _sprites)
            {
                spr.color = Color.Lerp(spr.color, _white, Time.deltaTime * _dissapearSpeed * 2);
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                Event.Invoke();
            }
        }
        else
        {
            if (_currentTimeToDissapear < 0)
            {
                foreach (TextMeshPro tmp in _text)
                {
                    tmp.color = Color.Lerp(tmp.color, _transparent, Time.deltaTime * _dissapearSpeed);
                }
                foreach (SpriteRenderer spr in _sprites)
                {
                    spr.color = Color.Lerp(spr.color, _transparent, Time.deltaTime * _dissapearSpeed);
                }
            }
            else
            {
                _currentTimeToDissapear -= Time.deltaTime;
            }
        }
    }

}
