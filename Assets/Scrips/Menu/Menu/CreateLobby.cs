using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Networking;

[AddComponentMenu("Network/NetworkManagerHUD")]
[RequireComponent(typeof(NetworkManager))]
[EditorBrowsable(EditorBrowsableState.Never)]
public class CreateLobby : MonoBehaviour
{
    public string IpAddress;
    public string Port;
    private bool _started;

    // Start is called before the first frame update
    void Start()
    {
        _started = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateServer()
    {
        if (!_started)
        {
            _started = true;
            NetworkManager.singleton.networkAddress = IpAddress;
            NetworkManager.singleton.networkPort = int.Parse(Port);
            NetworkManager.singleton.StartHost();
        }
    }


}
