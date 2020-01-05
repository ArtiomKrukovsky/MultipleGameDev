using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;

public class JoinLobby : MonoBehaviour
{
    public NetworkManager manager;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void JoinMatch(string matchName)
    {
        foreach (var match in manager.matches)
        {
            if (match.name == matchName)
            {
                manager.matchName = match.name;
                manager.matchSize = (uint)match.currentSize;
                manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
            }
        }
    }
}
