using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public LayerMask movementMask;
    Camera cam;
    [SerializeField] float moveSpeed = 10;

    Vector3 originalPos;
    Quaternion originalRot;

    GameObject playerTarget;
    bool isFreeView = false;
    bool canMoveToOriginalPos;
    bool isFirstPerson = true;

    void Start()
    {
        cam = Camera.main;
        originalPos = transform.position;
        originalRot = transform.rotation;
    }

    void Update()
    {
        SetTarget();
        if (!isFreeView && !canMoveToOriginalPos)
        {
            if (playerTarget != null)
            {
                if (!isFirstPerson) LookTarget();
                else transform.rotation = playerTarget.transform.rotation;
                MoveToTarget();
            }
        }
        else if(!isFreeView && canMoveToOriginalPos)
        {
            MoveToOriginalPos();
        }

        else
        {
            GetKeyBoardInput();
        }
    }

    private void GetKeyBoardInput()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * moveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * moveSpeed);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(Vector3.left * moveSpeed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(Vector3.right * moveSpeed);
        }
    }

    private void SetTarget()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, movementMask))
            {
                var newPlayer = hit.transform.GetComponent<PlayerController>().gameObject;
                playerTarget = newPlayer;
            }
        }
    }

    void LookTarget()
    {
        transform.LookAt(playerTarget.transform.position);
    }

    void MoveToTarget()
    {
        Vector3 newPosition;

        if (!isFirstPerson)
        {
            newPosition = playerTarget.transform.position - new Vector3(10, -10, 10);
        }
        else
        {
            newPosition = playerTarget.transform.position;
        }
            
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed);
    }

    public void MoveToOriginalPos()
    {
        transform.rotation = originalRot;
        if(transform.position != originalPos)
        {
            canMoveToOriginalPos = true;
            playerTarget = null;
            transform.position = Vector3.MoveTowards(transform.position, originalPos, moveSpeed);
        }
        else canMoveToOriginalPos = false;
    }

    public void SetIsFree(bool value)
    {
        isFreeView = value;
    }

    public void ToggleFirstPerson(bool value)
    {
        isFirstPerson = value;
    }
}
