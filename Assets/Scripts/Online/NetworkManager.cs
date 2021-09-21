using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private byte MaxPlayerRoom = 2;
    private string _gameVersion = "1.0";
    public void Start()
    {
        Login();
    }
    private void Login()
    {
        if(!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVersion;
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("[Network Manager: Connected to "+PhotonNetwork.CloudRegion + " server");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("[Network Manager: Disconnected wit reason: " + cause);
    }
    public void QuickPlay()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            Debug.LogError("[Network Manager]: Not connected");
            Login();
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("[Network Manager]: Join random failed. No random room available, so we create one");
        PhotonNetwork.CreateRoom(null, new RoomOptions {MaxPlayers = MaxPlayerRoom});
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("[Network Manager]: Joined room "+PhotonNetwork.CurrentRoom.Name);
    }
}
