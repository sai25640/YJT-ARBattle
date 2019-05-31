using UnityEngine;

public class RFX4_DeactivateRigidbodyByTime : MonoBehaviour
{
    public float TimeDelayToDeactivate = 6;

    private void OnEnable()
    {
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.detectCollisions = true;
        Invoke("Deactivate", TimeDelayToDeactivate);
    }

    private void Deactivate()
    {
        var rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.detectCollisions = false;
    }
}