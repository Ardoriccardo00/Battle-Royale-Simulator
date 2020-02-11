using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class KillFeed : MonoBehaviour
{

    [SerializeField] GameObject killFeedItem;

    #region Singleton
    public static KillFeed Instance { get; private set; }
    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //dont destroy on load if needed
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    public void AddKillFeedItemIdentity(string killer, string victim)
    {
        var item = Instantiate(killFeedItem);
        Destroy(item, 3);
        item.transform.SetParent(transform);
        TextMeshProUGUI text = item.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "<b><color=green>" + killer + "</color></b> <color=red>" + "has killed </color> <b><color=blue>" + victim + "</color></b>";
    }

    public void AddKillFeedItem(string killer, string victim)
    {
        var item = Instantiate(killFeedItem);
        Destroy(item, 3);
        item.transform.SetParent(transform);
        TextMeshProUGUI text = item.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "<b><color=green>" + killer + "</color></b> <color=red>" + "has killed </color> <b><color=blue>" + victim + "</color></b>";
    }
}
