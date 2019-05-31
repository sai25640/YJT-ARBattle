using UnityEngine;

public class RFX4_DeactivateByTime : MonoBehaviour
{
    public float DeactivateTime = 3;

    private bool canUpdateState;

    // Use this for initialization
    private void OnEnable()
    {
        canUpdateState = true;
    }

    private void Update()
    {
        if (canUpdateState)
        {
            canUpdateState = false;
            Invoke("DeactivateThis", DeactivateTime);
        }
    }

    private void OnDestroy()
    {
        RFX4_TransformMotion.destroyEffect -= DeactivateThis;
    }

    // Update is called once per frame
    private void DeactivateThis()
    {
        Destroy(this.gameObject);
    }
}