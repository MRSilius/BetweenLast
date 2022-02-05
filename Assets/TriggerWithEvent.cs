using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerWithEvent : MonoBehaviour
{
    public UnityEvent Event;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Event.Invoke();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Interactions"))
        {
            Event.Invoke();
        }
    }
}
