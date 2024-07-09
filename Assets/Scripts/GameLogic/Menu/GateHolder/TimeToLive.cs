using UnityEngine;
using System.Collections;
public class TimeToLive : MonoBehaviour
{
    public float ttl = 1.0f;
    private bool expired = false;

    public delegate void TTLExpired(GameObject gameObject);
    public static event TTLExpired OnTTLExpired;

    private void Start()
    {
        StartCoroutine(ExpireAfterTime());
    }

     IEnumerator ExpireAfterTime()
    {
        yield return new WaitForSeconds(ttl);
        expired = true;
        OnTTLExpired?.Invoke(gameObject);
    }

    public bool IsExpired()
    {
        return expired;
    }
}
