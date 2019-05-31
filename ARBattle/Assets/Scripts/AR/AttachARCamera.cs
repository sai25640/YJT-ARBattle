
using UnityEngine;
using EasyAR;
using UniRx;
namespace ARBattle
{
    public class AttachARCamera : MonoBehaviour
    {

        private Transform aRCamera;

        public Transform ARCamera
        {
            get
            {
                return ARManager.Instance.ARCamera;
            }
        }

        void Start()
        {
            Observable.EveryFixedUpdate()
                .Subscribe(_ =>
                {
                    if (ARCamera)
                    {
                        transform.position = ARCamera.position;
                        transform.eulerAngles = new Vector3(transform.eulerAngles.x, ARCamera.eulerAngles.y, transform.eulerAngles.z);
                    }
                })
                .AddTo(this);
        }

    }
}
