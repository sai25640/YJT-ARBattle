using UnityEngine;

public class RFX4_StartDelay : MonoBehaviour
{
    public GameObject ActivatedGameObject;
    public float Delay = 1;

    // Use this for initialization
    private void OnEnable()
    {
        ActivatedGameObject.SetActive(false);
        Invoke("ActivateGO", Delay);
    }

    // Update is called once per frame
    private void ActivateGO()
    {
        ActivatedGameObject.SetActive(true);
    }

    private void OnDisable()
    {
        CancelInvoke("ActivateGO");
    }
}