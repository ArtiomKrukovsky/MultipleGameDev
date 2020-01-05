using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CreateLobby : MonoBehaviour
{
    public void CreateServer()
    {
        try
        {
            GameObject network = this.FindNetworkByTag();
            var manager = network?.GetComponent<NetworkManager>();

            if (manager == null)
            {
                Debug.Log($"Manager is null");
                return;
            }

            if (manager.matchMaker == null)
            {
                manager.StartMatchMaker();
            }

            SceneManager.LoadScene("GameScene");
            manager.matchMaker.CreateMatch(manager.matchName, manager.matchSize, true, "", "", "", 0, 0, manager.OnMatchCreate);
        }
        catch(Exception e)
        {
            Debug.Log($"Error, something went wrong: { e.Message }");
            SceneManager.LoadScene("Menu");
        }
    }

    private GameObject FindNetworkByTag()
    {
        return GameObject.FindGameObjectWithTag("Network");
    }
}
