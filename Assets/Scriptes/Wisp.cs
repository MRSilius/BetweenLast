using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wisp : MonoBehaviour
{
    [SerializeField] private Transform _wisp;
    [SerializeField] private GameObject[] _changeEffects;
    [SerializeField] private bool _openedSkill;

    private DimentionObject dObj;

    void Start()
    {
        
    }
    public void OpenSkill()
    {
        _openedSkill = true;
    }

    void Update()
    {
        if (!_openedSkill) return;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _wisp.position = new Vector3(mousePos.x, mousePos.y, 0);


        if (Input.GetMouseButton(0))
        {
            foreach(GameObject g in _changeEffects)
            {
                g.SetActive(true);
            }

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                if(hit.transform.TryGetComponent(out DimentionObject obj))
                {
                    dObj = obj;
                    dObj.AddForce();
                }
            }
            else
            {
                if(dObj != null)
                {
                    dObj.StopAddForce();
                }
            }
        }
        else
        {
            foreach (GameObject g in _changeEffects)
            {
                g.SetActive(false);
            }
            if (dObj != null)
            {
                dObj.StopAddForce();
            }
        }

    }
}
