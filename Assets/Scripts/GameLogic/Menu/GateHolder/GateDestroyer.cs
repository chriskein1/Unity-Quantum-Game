using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateDestroyer : MonoBehaviour
{
    [SerializeField] private GateHolderScript gateHolder;
    [SerializeField] private List<GameObject> prefabsToDestroy = new List<GameObject>();

    private List<string> gateTags = new List<string>();

    private void Awake()
    {
        // Populate gateTags with tags from the prefabs
        foreach (var prefab in prefabsToDestroy)
        {
            if (prefab != null)
            {
                string tag = prefab.tag;
                if (!gateTags.Contains(tag))
                {
                    gateTags.Add(tag);
                }
            }
        }
    }

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
        if (gateTags.Contains(other.tag))
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
