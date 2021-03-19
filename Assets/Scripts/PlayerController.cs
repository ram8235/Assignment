using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] Animator unityChanAnimator;
    [SerializeField] NavMeshAgent _navMesh;
    public Joystick joystick;

    [Header("Dummy")]
    [SerializeField] Dummy _dummy;
    public GameObject dummy;

    [Header("Emoji")]
    public GameObject emoji;

    [Header("Walking")]
    public bool isWalking;
    [SerializeField] float distance;
    [SerializeField] Vector3 lastPos;

    [Header("Attack")]
    [SerializeField] bool attacking;


    public void Start()
    {
        lastPos = transform.position;
    }
    void Update()
    {
        //Movement
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;
        Vector2 convertedXY = ConvertWithCamera(Camera.main.transform.position, horizontal, vertical);
        Vector3 direction = new Vector3(convertedXY.x, 0, convertedXY.y).normalized;
        transform.Translate(direction * 0.1f, Space.World);
        Vector3 lookAtPosition = transform.position + direction;
        transform.LookAt(lookAtPosition);

        //Animation
        if(direction.x != 0 || direction.z != 0)
        {
            _navMesh.isStopped = true;
        }

        if (transform.position == lastPos)
        {
            isWalking = false;
        }
        else
        {
            isWalking = true;
        }
        lastPos = transform.position;
        unityChanAnimator.SetBool("Walking", isWalking);

        //AI
        distance = Vector3.Distance(transform.position,dummy.transform.position);
        if (distance < 1.6f)
        {
            _navMesh.isStopped = true;
            if(attacking)
            {
                _dummy.atacked();
                unityChanAnimator.SetTrigger("Attack");
                attacking = false;
            }
        }

        //Emoji
        if(_dummy.hp.fillAmount <= 0)
        {
            emoji.SetActive(true);
        }
        else
        {
            emoji.SetActive(false);
        }
    }

    public void AttackBtn()
    {
        Vector3 dummyVec = new Vector3(dummy.transform.position.x - transform.position.x, 0, dummy.transform.position.z - transform.position.z);
        Quaternion rotation = Quaternion.LookRotation(dummyVec);
        transform.rotation = rotation;

        _navMesh.isStopped = false;
        _navMesh.SetDestination(dummy.transform.position);
        attacking = true;
    }

    private Vector2 ConvertWithCamera(Vector3 cameraPos, float hor, float ver)
    {
        Vector2 joyDirection = new Vector2(hor, ver).normalized;
        Vector2 camera2DPos = new Vector2(cameraPos.x, cameraPos.z);
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 cameraToPlayerDirection = (Vector2.zero - camera2DPos).normalized;
        float angle = Vector2.SignedAngle(cameraToPlayerDirection, new Vector2(0, 1));
        Vector2 finalDirection = RotateVector(joyDirection, -angle);
        return finalDirection;
    }

    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }
}
