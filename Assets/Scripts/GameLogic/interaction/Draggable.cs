using UnityEngine;

/// <summary>
/// This class allows objects to be dragged within the screen boundaries.
/// </summary>
public class Drag : MonoBehaviour
{
    [SerializeField] private bool disableSnap;
    [SerializeField] private bool canGrab = true;
    private bool hasRigidbody;
    private bool dragging = false;
    private Vector3 offset;
    private Vector3 extents;
    private Quaternion originalRotation;
    private bool snapped = false;
    private Rigidbody2D rb;
    private void Start()
    {
        // Record the size of the sprite so we can limit it to the screen if necessary.
        extents = GetComponent<SpriteRenderer>().sprite.bounds.extents;
        // Store the original rotation of the object.
        originalRotation = transform.rotation;
        rb = GetComponent<Rigidbody2D>();
        // Check if the object has a Rigidbody2D component
        hasRigidbody = rb != null;
    }

    // Update is called once per frame
    void Update()
    {
        if (dragging && Time.deltaTime != 0)
        {
            if (Input.GetMouseButton(0))
            {
                
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
                // Find the screen bounds in world coordinates.
                Vector3 topRight = Camera.main.ViewportToWorldPoint(Vector3.one);
                Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
                // Limit to the screen
                pos.x = Mathf.Clamp(pos.x, bottomLeft.x + extents.x, topRight.x - extents.x);
                pos.y = Mathf.Clamp(pos.y, bottomLeft.y + extents.y, topRight.y - extents.y);

                // Set the object's position.
                transform.position = pos;
            }
            else
            {
                dragging = false;
            }
        }
    }

    private void OnMouseDown()
    {
        if (!canGrab)
        {
            return;
        }
        // Record the difference between the object's center and the clicked point on the camera plane.
        if (Time.deltaTime > 0)
        {
            if (snapped && hasRigidbody)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            snapped = false;
            // Reset the object's rotation to the original rotation.
            transform.rotation = originalRotation;
               // Calculate the offset based on the current position.
                offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
          

            dragging = true;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the collided object is being pushed off screen and correct its position.
        CorrectPosition(collision.collider);
    }

    private void CorrectPosition(Collider2D collider)
    {
        Vector3 pos = collider.transform.position;
        Vector3 topRight = Camera.main.ViewportToWorldPoint(Vector3.one);
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 colExtents = collider.bounds.extents;

        // Limit to the screen
        pos.x = Mathf.Clamp(pos.x, bottomLeft.x + colExtents.x, topRight.x - colExtents.x);
        pos.y = Mathf.Clamp(pos.y, bottomLeft.y + colExtents.y, topRight.y - colExtents.y);

        // Set the object's position.
        collider.transform.position = pos;
    }

    private void OnMouseUp()
    {
        // Stop dragging.
        dragging = false;
        
    }

    public bool IsDragging()
    {
        return dragging;
    }

    public bool IsSnapped()
    {
        return snapped;
    }

    public bool CanSnap()
    {
        return !disableSnap;
    }
    public void Snapping()
    {
        snapped = true;
        if (hasRigidbody)
            rb.bodyType = RigidbodyType2D.Static;
        // Reset the object's rotation to the original rotation.
        transform.rotation = originalRotation;
    }

    public void Unsnap()
    {
        snapped = false;
        if (hasRigidbody)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;
        }
        transform.rotation = originalRotation;
    }
    public void StartDraggingFromSpawn(Vector3 spawnPosition)
    {
        dragging = true;
        offset = spawnPosition - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
 
}
