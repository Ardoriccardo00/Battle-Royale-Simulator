using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public LayerMask movementMask;
    Camera cam;
    [SerializeField] float moveSpeed = 10;
    [SerializeField] Vector3 originalPos;
    GameObject playerTarget;
    bool isFreeView = false;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        SetTarget();
        if (!isFreeView)
        {
            if (playerTarget != null)
            {            
                LookTarget();
                MoveToTarget();
            }
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
        Vector3 newPosition = playerTarget.transform.position - new Vector3(10, -10, 10);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed);
        //transform.position = Vector3.MoveTowards(transform.position, playerTarget.transform.position, moveSpeed);
    }

    public void MoveToOriginalPos()
    {
        playerTarget = null;
        transform.position = originalPos;
    }

    public void SetIsFree(bool value)
    {
        isFreeView = value;
    }
}
