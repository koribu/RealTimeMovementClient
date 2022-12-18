using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static public class NetworkedClientProcessing
{

    #region Send and Receive Data Functions
    static public void ReceivedMessageFromServer(string msg)
    {
        Debug.Log("msg received = " + msg + ".");

        string[] csv = msg.Split(',');
        int signifier = int.Parse(csv[0]);

         if (signifier == ServerToClientSignifiers.UpdatePlayersWithPositionChange)
         {
            Vector2 positionChange = new Vector2(float.Parse(csv[1]), float.Parse(csv[2]));
            gameLogic.ApplyMovementFromServer(positionChange, int.Parse(csv[3]));
         }
         else if (signifier == ServerToClientSignifiers.CreatePlayerCharacter)
         {
            gameLogic.AddMyPlayerIntoGame(int.Parse(csv[1])) ;
         }
        else if (signifier == ServerToClientSignifiers.CreateNewOtherCharacter)
        {
            gameLogic.AddCharacterIntoGame(Vector2.zero, int.Parse(csv[1]));
        }
        else if (signifier == ServerToClientSignifiers.CreateExistOtherCharacter)
        {
            Vector2 pos = new Vector2(int.Parse(csv[1]), int.Parse(csv[2]));
            gameLogic.AddCharacterIntoGame( pos, int.Parse(csv[3]));
        }

        //gameLogic.DoSomething();

    }

    static public void SendMessageToServer(string msg)
    {
        networkedClient.SendMessageToServer(msg);
    }

    static public void SendNewPositionToServer(Vector2 pos)
    {
        string msg = pos.x + "," + pos.y;
        msg = ClientToServerSignifiers.SendPositionChangeSignifier + "," + msg;
        SendMessageToServer(msg);
    }

    #endregion

    #region Connection Related Functions and Events
    static public void ConnectionEvent()
    {
        Debug.Log("Network Connection Event!");
    }
    static public void DisconnectionEvent()
    {
        Debug.Log("Network Disconnection Event!");
    }
    static public bool IsConnectedToServer()
    {
        return networkedClient.IsConnected();
    }
    static public void ConnectToServer()
    {
        networkedClient.Connect();
    }
    static public void DisconnectFromServer()
    {
        networkedClient.Disconnect();
    }

    #endregion

    #region Setup
    static NetworkedClient networkedClient;
    static GameLogic gameLogic;

    static public void SetNetworkedClient(NetworkedClient NetworkedClient)
    {
        networkedClient = NetworkedClient;
    }
    static public NetworkedClient GetNetworkedClient()
    {
        return networkedClient;
    }
    static public void SetGameLogic(GameLogic GameLogic)
    {
        gameLogic = GameLogic;
    }

    #endregion

}

#region Protocol Signifiers
static public class ClientToServerSignifiers
{
    public const int SendPositionChangeSignifier = 1;
}

static public class ServerToClientSignifiers
{
    public const int UpdatePlayersWithPositionChange = 1;
    public const int CreatePlayerCharacter = 2;
    public const int CreateNewOtherCharacter = 3;
    public const int CreateExistOtherCharacter = 4;
}

#endregion

