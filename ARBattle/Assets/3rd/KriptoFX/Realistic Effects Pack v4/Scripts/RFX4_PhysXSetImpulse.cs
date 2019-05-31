using UnityEngine;

public class RFX4_PhysXSetImpulse : MonoBehaviour
{
    public float Force = 1;
    public ForceMode ForceMode = ForceMode.Force;

    private Rigidbody rig;
    private Transform t;

    // Use this for initialization
    private void Start()
    {
        rig = GetComponent<Rigidbody>();
        t = transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (rig != null) rig.AddForce(t.forward * Force, ForceMode);
    }

    private void OnDisable()
    {
        if (rig != null)
            rig.velocity = Vector3.zero;
    }
}