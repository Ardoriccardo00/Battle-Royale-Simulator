using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoardButton : MonoBehaviour
{
    [SerializeField] PlayerStatsTXT targetPlayer;
    public int orderNumber;
    TextMeshProUGUI buttonText;
    CameraController cam;

    private void Start()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        cam = FindObjectOfType<CameraController>();
    }

    private void LateUpdate()
    {
        if(GameManager.Instance.ReturnMatchHasStarted())
            buttonText.text = targetPlayer.ReturnPlayerName() + " " + targetPlayer.ReturnKills();
    }

    public void SetPlayerToTarget(PlayerStatsTXT player)
    {
        targetPlayer = player;
    }

    public void InvokeSetCameraTargetExternally()
    {
        cam.SetTargetExternally(targetPlayer.gameObject);
    }
}
