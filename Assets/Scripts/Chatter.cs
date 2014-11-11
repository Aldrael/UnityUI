using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(NetworkView))]
public class Chatter : MonoBehaviour
{

    MessageWindow messageWindow;
    string chatInput = "";

    float messageDisplayTime = 60;

    public GUISkin chatterSkin;

    Vector2 chatOutputSize = new Vector2(350, 175);
    Vector2 chatOutputPosition = new Vector2(0, Screen.height - 200);

    Vector2 chatInputSize = new Vector2(350, 25);
    Vector2 chatInputPosition = new Vector2(0, Screen.height - 25);

    string myName = "NoName";
    bool nameSet = false;

    bool connected = false;

    Rect chatInputRect;
    Rect chatOutputRect;
    Rect entireChatArea;

    Dictionary<NetworkPlayer, string> usersByID = new Dictionary<NetworkPlayer, string>();

    // Use this for initialization
    void Start()
    {
        chatInputRect = new Rect(chatInputPosition.x, chatInputPosition.y, chatInputSize.x, chatInputSize.y);
        chatOutputRect = new Rect(chatOutputPosition.x, chatOutputPosition.y, chatOutputSize.x, chatOutputSize.y);

        entireChatArea = new Rect(chatInputPosition.x, chatInputPosition.y, Mathf.Max(chatInputSize.x, chatOutputSize.x), chatInputSize.y + chatOutputSize.y);

        //messageWindow = new MessageWindow(chatOutputSize, chatterSkin);
        messageWindow = ScriptableObject.CreateInstance<MessageWindow>();
        messageWindow.setMessageWindow(chatOutputSize, chatterSkin);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GUIUtils.MouseOverRect(entireChatArea))
        {
            messageWindow.pauseAutoScroll = false;
            messageWindow.CountDownTimers();
        }
        else
        {
            messageWindow.pauseAutoScroll = true;
        }
    }

    void SetName(string newName)
    {
        //Only after setting a name will the player be joined to the chat
        nameSet = true;
        if (Network.isServer)
        {
            JoinUser(newName, Network.player);
        }
        else
        {
            networkView.RPC("JoinUser", RPCMode.Server, myName, Network.player);
            networkView.RPC("Server_SendCurrentUsers", RPCMode.Server, Network.player);
        }
    }

    void ProcessInput()
    {
        if (chatInput.Length > 0)
            networkView.RPC("LogMessage", RPCMode.All, chatInput, Network.player);

        chatInput = "";
    }


    void OnConnectedToServer()
    {
        connected = true;
    }

    void OnServerInitialized()
    {
        connected = true;
    }

    void OnDisconnectedFromServer(NetworkDisconnection info)
    {
        if (Network.isServer)
            SystemMessage("Server connection lost!", Network.player);
        else
            if (info == NetworkDisconnection.LostConnection)
                SystemMessage("Lost connection to server!", Network.player);
            else
                SystemMessage("Successfully diconnected from server.", Network.player);
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        RemoveUser(player);
    }


    [RPC]
    void Server_SendCurrentUsers(NetworkPlayer recipient)
    {
        //In case someone else gets this message, only reply if we're the server
        if (Network.isServer)
        {
            foreach (NetworkPlayer user in usersByID.Keys)
            {
                networkView.RPC("JoinUser", recipient, usersByID[user], user);
            }
        }
    }

    [RPC]
    void JoinUser(string name, NetworkPlayer player)
    {
        if (!this.usersByID.ContainsKey(player))
        {
            this.usersByID.Add(player, name);

            if (Network.isServer)
            {
                //Since we're server, let everyone know when someone joins.
                //This includes the someone who joined, so they can add themselves and get the message
                networkView.RPC("JoinUser", RPCMode.Others, name, player);
                networkView.RPC("SystemMessage", RPCMode.All, "Joined chat.", player);
            }
        }
    }


    [RPC]
    void RemoveUser(NetworkPlayer player)
    {
        if (this.usersByID.ContainsKey(player))
        {
            SystemMessage("Has left chat", player);
            //If we're the server, let everyone know when someone leaves.
            if (Network.isServer)
            {
                networkView.RPC("RemoveUser", RPCMode.Others, player);
            }
            this.usersByID.Remove(player);
        }
    }

    [RPC]
    void SystemMessage(string message, NetworkPlayer player)
    {
        if (usersByID.ContainsKey(player))
        {
            message = usersByID[player] + ": " + message;
            messageWindow.AddMessage(messageDisplayTime, message);

        }
        else
        {
            //We don't know that user? Why not? Get the user list again, maybe we'll know for next time.
            networkView.RPC("Server_SendCurrentUsers", RPCMode.Server, Network.player);
        }
    }

    [RPC]
    void LogMessage(string message, NetworkPlayer player)
    {
        if (usersByID.ContainsKey(player))
        {
            //If we didn't say it, enter add some info about who did
            if (player != Network.player)
            {
                message = usersByID[player] + " said: " + message;
            }

            messageWindow.AddMessage(messageDisplayTime, message);
        }
        else
        {
            //We don't know that user? Why not? Get the user list again, maybe we'll know for next time.
            networkView.RPC("Server_SendCurrentUsers", RPCMode.Server, Network.player);
        }
    }


    void OnGUI()
    {
        if (connected)
        {
            if (nameSet)
            {

                messageWindow.Draw(chatOutputPosition);

                if (Event.current.Equals(Event.KeyboardEvent("return")) && GUI.GetNameOfFocusedControl().Equals("TextInput"))
                {
                    ProcessInput();
                }
                GUI.SetNextControlName("TextInput");
                chatInput = GUI.TextField(chatInputRect, chatInput);

            }
            else
            {
                //Require the user set their name before they're fully joined to the chat server.
                GUI.Label(chatOutputRect, "Please enter your name");
                if (Event.current.Equals(Event.KeyboardEvent("return")) && GUI.GetNameOfFocusedControl().Equals("NameInput") && myName.Length > 0)
                {
                    SetName(myName);
                }

                GUI.SetNextControlName("NameInput");
                myName = GUI.TextField(chatInputRect, myName);

                GUI.FocusControl("NameInput");
            }
        }
    }
}
