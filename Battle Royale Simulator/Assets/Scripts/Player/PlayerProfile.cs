using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Profile", menuName = "Create new profile")]
public class PlayerProfile : ScriptableObject
{
    public string playerName;
    public Sprite playerAvatar;
    public Material playerColor;
}
