using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutputDestroyer : MonoBehaviour
{
    [SerializeField] private OutputHolderScript gateHolder;
    private string[] gateTags = new string[] { "Zero", "One", "NegOne", "PosSuperPosition", "NegSuperPosition" };

    private void OnEnable()
    {
        TimeToLive.OnTTLExpired += HandleTTLExpired;
    }

    private void OnDisable()
    {
        TimeToLive.OnTTLExpired -= HandleTTLExpired;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        CheckAndDestroyGate(other);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        CheckAndDestroyGate(other);
    }

    private void HandleTTLExpired(GameObject expiredGate)
    {
        Collider2D collider = expiredGate.GetComponent<Collider2D>();
        if (collider != null && collider.IsTouching(GetComponent<Collider2D>()))
        {
            CheckAndDestroyGate(collider);
        }
    }

    private void CheckAndDestroyGate(Collider2D other)
    {
        if (other.CompareTag("Zero") || other.CompareTag("One") || other.CompareTag("NegOne") || other.CompareTag("PosSuperPosition") || other.CompareTag("NegSuperPosition"))
        {
            TimeToLive ttl = other.GetComponent<TimeToLive>();
            if (ttl != null && !ttl.IsExpired())
            {
                return;
            }

            string gateTag = other.tag;

            // Check if the other object has a parent with the same tag
            Transform objectToDestroy = other.transform;
            if (other.transform.parent != null && other.transform.parent.CompareTag(gateTag))
            {
                objectToDestroy = other.transform.parent;
            }
            ClickSound();
            gateHolder.AddGateBackToCount(gateTag);
            Destroy(objectToDestroy.gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (Time.timeScale > 0)
        {
            ClickSound();
            DestroyAllGates();
        }
    }

    private void DestroyAllGates()
    {
        foreach (string tag in gateTags)
        {
            GameObject[] gates = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject gate in gates)
            {
                // Check if the gate has a parent and if the parent has the same tag
                if (gate.transform.parent == null || gate.transform.parent.tag != tag)
                {
                    TimeToLive ttl = gate.GetComponent<TimeToLive>();
                    if (ttl == null || ttl.IsExpired())
                    {
                        gateHolder.AddGateBackToCount(gate.tag);
                        Destroy(gate);
                    }
                }
            }
        }
    }

    public void ClickSound()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.Play("ClickingTrashCan");
        }
        else
        {
            Debug.LogError("AudioManager instance not found!");
        }
    }
}
