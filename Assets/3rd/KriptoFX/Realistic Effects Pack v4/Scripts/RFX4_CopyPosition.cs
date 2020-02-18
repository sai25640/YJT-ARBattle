using UnityEngine;

public class RFX4_CopyPosition : MonoBehaviour
{
    public Transform CopiedTransform;

    private Transform t;

    // Use this for initialization
    private void Start()
    {
        t = transform;
    }

    // Update is called once per frame
    private void Update()
    {
        t.position = CopiedTransform.position;
    }
}