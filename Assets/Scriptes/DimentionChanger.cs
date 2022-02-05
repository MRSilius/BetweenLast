using System.Collections.Generic;
using UnityEngine;

public class DimentionChanger : MonoBehaviour
{
    [SerializeField] private bool _openedSkill;
    [SerializeField] private bool _isDarkDemention;
    [SerializeField] private GameObject _darkDemention;
    [SerializeField] private GameObject _whiteDemention;
    [SerializeField] private Transform _darkDementionT;
    [SerializeField] private Transform _whiteDementionT;
    [SerializeField] private Transform _player;
    [SerializeField] private float _maxScale;

    [SerializeField] private float _maxDimentionScale;

    [SerializeField] private List<Dimention> _dementions;

    [SerializeField] private List<Collider2D> _darkDimentionColliders;
    [SerializeField] private List<Collider2D> _whiteDimentionColliders;

    [SerializeField] private List<Rigidbody2D> _darkDimentionRigidbodies;
    [SerializeField] private List<Rigidbody2D> _whiteDimentionRigidbodies;

    [SerializeField] private AudioSource _soundAudioSource;
    [SerializeField] private AudioClip _changeDimClip;

    private void Awake()
    {
        _player = FindObjectOfType<CharacterEvents>().transform;

        _darkDimentionColliders.AddRange(_darkDemention.GetComponentsInChildren<Collider2D>());
        _whiteDimentionColliders.AddRange(_whiteDemention.GetComponentsInChildren<Collider2D>());
        _darkDimentionRigidbodies.AddRange(_darkDemention.GetComponentsInChildren<Rigidbody2D>());
        _whiteDimentionRigidbodies.AddRange(_whiteDemention.GetComponentsInChildren<Rigidbody2D>());

        UpdateDimentions();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (!_openedSkill) return;

            ChangeDemention();
        }
    }
    public void OpenSkill()
    {
        _openedSkill = true;
    }

    private void FixedUpdate()
    {
        foreach (Dimention demention in _dementions)
        {
            demention.transform.position = _player.transform.position;
        }
        UpdateDimentions();
    }

    private void UpdateDimentions()
    {
        _darkDementionT.position = _player.transform.position;
        _whiteDementionT.position = _player.transform.position;

        if (_isDarkDemention)
        {
            if (_whiteDementionT.localScale != Vector3.zero)
            {
                //_whiteDementionT.localScale = Vector3.Lerp(_whiteDementionT.localScale, Vector3.zero, Time.deltaTime * 8);
            }
            if (_darkDementionT.localScale != Vector3.one * _maxScale)
            {
                _darkDementionT.localScale = Vector3.Lerp(_darkDementionT.localScale, Vector3.one * _maxScale, Time.deltaTime * 8);
            }
        }
        else
        {
            if (_darkDementionT.localScale != Vector3.zero)
            {
                _darkDementionT.localScale = Vector3.Lerp(_darkDementionT.localScale, Vector3.zero, Time.deltaTime * 8);
            }
            if (_whiteDementionT.localScale != Vector3.one * _maxScale)
            {
                //_whiteDementionT.localScale = Vector3.Lerp(_whiteDementionT.localScale, Vector3.one * _maxScale, Time.deltaTime * 8);
            }
        }
    }

    public void ChangeDemention()
    {
        if (!_isDarkDemention)
        {
            _soundAudioSource.PlayOneShot(_changeDimClip);
        }

        _isDarkDemention = !_isDarkDemention;
        UpdateDimention();
    }

    private void UpdateDimention()
    {
        foreach (Collider2D col in _whiteDimentionColliders)
        {
            col.enabled = _isDarkDemention;
        }
        foreach (Collider2D col in _darkDimentionColliders)
        {
            col.enabled = !_isDarkDemention;
        }

        if (_isDarkDemention)
        {
            foreach (Rigidbody2D rb in _whiteDimentionRigidbodies)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            foreach (Rigidbody2D rb in _darkDimentionRigidbodies)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
        else
        {
            foreach (Rigidbody2D rb in _whiteDimentionRigidbodies)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }
            foreach (Rigidbody2D rb in _darkDimentionRigidbodies)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    public void ChangeObjectDimention(DimentionObject obj)
    {
        if (obj.InDarkDimention)
        {
            _darkDimentionColliders.Remove(obj.Collider);
            _darkDimentionRigidbodies.Remove(obj.Rigidbody);

            _whiteDimentionColliders.Add(obj.Collider);
            _whiteDimentionRigidbodies.Add(obj.Rigidbody);
        }
        else
        {
            _darkDimentionColliders.Add(obj.Collider);
            _darkDimentionRigidbodies.Add(obj.Rigidbody);

            _whiteDimentionColliders.Remove(obj.Collider);
            _whiteDimentionRigidbodies.Remove(obj.Rigidbody);
        }

        UpdateDimention();
    }
}
