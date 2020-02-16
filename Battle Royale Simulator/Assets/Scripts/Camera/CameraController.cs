using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MouseLook))]
public class CameraController : MonoBehaviour
{
    public LayerMask movementMask;
    Camera cam;
    MouseLook mouseLook;

    [SerializeField] float moveSpeedAuto = 1;
    [SerializeField] float moveSpeedManual = 1;

    Vector3 originalPos;
    Quaternion originalRot;

    GameObject playerTarget;
    public bool isFreeView = false;
    bool canMoveToOriginalPos;
    bool isFirstPerson = false;

    void Start()
    {
        cam = Camera.main;
        mouseLook = GetComponent<MouseLook>();
        originalPos = transform.position;
        originalRot = transform.rotation;
        moveSpeedAuto = moveSpeedAuto * Time.deltaTime;
        moveSpeedManual = moveSpeedManual * Time.deltaTime;
    }

    void Update()
    {
        SetTarget();
        if(!isFreeView)
        {
            Cursor.lockState = CursorLockMode.None;
            mouseLook.enabled = false;
            if(!canMoveToOriginalPos)
            {
                if(playerTarget != null)
                {
                    if(!isFirstPerson) LookTarget();
                    else transform.rotation = playerTarget.transform.rotation;
                    MoveToTarget();
                }
            }
            else MoveToOriginalPos();
        }
        
        else
        {
            mouseLook.enabled = true;           
            GetKeyBoardInput();
        }
    }

    private void GetKeyBoardInput()
    {
        transform.Translate(Input.GetAxis("Horizontal") * moveSpeedManual, 0, Input.GetAxis("Vertical") * moveSpeedManual);
        if(Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
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
                isFreeView = false;
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
            
        transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeedAuto);
    }

    public void MoveToOriginalPos()
    {
        transform.rotation = originalRot;
        if(transform.position != originalPos)
        {
            canMoveToOriginalPos = true;
            playerTarget = null;
            transform.position = Vector3.MoveTowards(transform.position, originalPos, moveSpeedAuto);
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

    public void SetCursorState(bool isLocked)
    {
        if(isLocked) Cursor.lockState = CursorLockMode.Locked;
        else Cursor.lockState = CursorLockMode.None;
    }

    public void SetTargetExternally(GameObject target)
    {
        playerTarget = target;
    }
}
