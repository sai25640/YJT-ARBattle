using System;
using System.Collections.Generic;
using ARBattle;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public delegate void AddScoresDel(int score);

public delegate void DestroyEfect();

public class RFX4_TransformMotion : MonoBehaviour
{
    public event AddScoresDel AddScore;
    public UnityEvent Event;

    public static event DestroyEfect destroyEffect;

    public float Distance = 30;
    public float Speed = 1;
    public float Dampeen = 0;
    public float MinSpeed = 1;
    public float TimeDelay = 0;
    public LayerMask CollidesWith = ~0;

    public GameObject[] EffectsOnCollision;
    public float CollisionOffset = 0;
    public float DestroyTimeDelay = 5;
    public bool CollisionEffectInWorldSpace = true;
    public GameObject[] DeactivatedObjectsOnCollision;
    [HideInInspector] public float HUE = -1;
    [HideInInspector] public List<GameObject> CollidedInstances;

    private Vector3 startPositionLocal;
    private Transform t;
    private Vector3 oldPos;
    private bool isCollided;
    private bool isOutDistance;
    private Quaternion startQuaternion;
    private float currentSpeed;
    private float currentDelay;
    private const float RayCastTolerance = 0.3f;
    private bool isInitialized;
    private bool dropFirstFrameForFixUnityBugWithParticles;
    public float Offset;

    public event EventHandler<RFX4_CollisionInfo> CollisionEnter;

    private void Start()
    {
        t = transform;
        startQuaternion = t.rotation;
        startPositionLocal = t.localPosition;
        oldPos = t.TransformPoint(startPositionLocal);
        Initialize();
        isInitialized = true;
    }

    private void OnEnable()
    {
        if (isInitialized) Initialize();
    }

    private void OnDisable()
    {
        if (isInitialized) Initialize();
    }

    private void Initialize()
    {
        isCollided = false;
        isOutDistance = false;
        currentSpeed = Speed;
        currentDelay = 0;
        startQuaternion = t.rotation;
        t.localPosition = startPositionLocal;
        oldPos = t.TransformPoint(startPositionLocal);
        OnCollisionDeactivateBehaviour(true);
        dropFirstFrameForFixUnityBugWithParticles = true;
    }

    private void Update()
    {
        if (!dropFirstFrameForFixUnityBugWithParticles)
        {
            UpdateWorldPosition();
        }
        else dropFirstFrameForFixUnityBugWithParticles = false;
    }

    private void UpdateWorldPosition()
    {
        bool isServerBool = false;
        bool isClientBool = false;
        currentDelay += Time.deltaTime;
        if (currentDelay < TimeDelay)
            return;

        var frameMoveOffset = Vector3.zero;
        var frameMoveOffsetWorld = Vector3.zero;
        if (!isCollided && !isOutDistance)
        {
            currentSpeed = Mathf.Clamp(currentSpeed - Speed * Dampeen * Time.deltaTime, MinSpeed, Speed);
            var currentForwardVector = Vector3.forward * currentSpeed * Time.deltaTime;
            frameMoveOffset = t.localRotation * currentForwardVector;
            frameMoveOffsetWorld = startQuaternion * currentForwardVector;
        }

        var currentDistance = (t.localPosition + frameMoveOffset - startPositionLocal).magnitude;

        RaycastHit hit;
        if (!isCollided && Physics.Raycast(t.position, t.forward, out hit, 10, CollidesWith))
        {


            if (frameMoveOffset.magnitude + RayCastTolerance > hit.distance)
            {
                isCollided = true;
                t.position = hit.point;
                oldPos = t.position;
                OnCollisionBehaviour(hit);
                OnCollisionDeactivateBehaviour(false);
                return;
            }

        }

        if (!isOutDistance && currentDistance > Distance)
        {
            Debug.Log("hahahahahhaahahahahah");
            isOutDistance = true;
            t.localPosition = startPositionLocal + t.localRotation * Vector3.forward * Distance;
            oldPos = t.position;
            return;
        }

        t.position = oldPos + frameMoveOffsetWorld;
        oldPos = t.position;
    }

    private void OnCollisionBehaviour(RaycastHit hit)
    {
        bool isServerBool = false;
        bool isClientBool = false;
        var handler = CollisionEnter;
        if (handler != null)
            handler(this, new RFX4_CollisionInfo { Hit = hit });
        Destroy(this.GetComponent<SphereCollider>());


        CollidedInstances.Clear();
        //Debug.Log(hit.transform.tag + "----" + hit.transform.name);

        //π÷ŒÔ ‹…À
        if (hit.collider.tag == "Enemy")
        {
            //EventCenter.Broadcast(EventDefine.Hit, 50,hit.collider);
            hit.transform.GetComponent<BaseCharacter>().PerformTransition(Transition.Hit);
        }
        else if (hit.collider.tag == "Head")
        {
            Debug.Log("Hit Head");
            var character =  hit.transform.GetComponentInParent<BaseCharacter>();
            character.HitHead();
            character.PerformTransition(Transition.Hit);      
        }
        else if (hit.collider.tag == "Guide" && StageSystem.Instance.Step1)
        {
            Debug.Log("Guide: Step1");
            hit.transform.GetComponent<GuideEnemy>().UnderAttack();

            StageSystem.Instance.Step1 = false;
        }
        else
        {
            Destroy(this.gameObject, 0.5f);
            return;
        }


        foreach (var effect in EffectsOnCollision)
        {
            var instance = Instantiate(effect, hit.point + hit.normal * CollisionOffset, new Quaternion()) as GameObject;
            instance.tag = this.tag;
            CollidedInstances.Add(instance);
            if (HUE > -0.9f)
            {
                RFX4_ColorHelper.ChangeObjectColorByHUE(instance, HUE);
            }
            instance.transform.LookAt(hit.point + hit.normal + hit.normal * CollisionOffset);
            if (!CollisionEffectInWorldSpace) instance.transform.parent = transform;
            Destroy(instance, 0.5f);
        }

        Destroy(this.gameObject, 0.5f);

    }

    private void OnCollisionDeactivateBehaviour(bool active)
    {
        foreach (var effect in DeactivatedObjectsOnCollision)
        {
            effect.SetActive(active);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying)
            return;

        t = transform;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(t.position, t.position + t.forward * Distance);
    }

    private void OnDestroy()
    {
    }

    public enum RFX4_SimulationSpace
    {
        Local,
        World
    }

    public class RFX4_CollisionInfo : EventArgs
    {
        public RaycastHit Hit;
    }
}