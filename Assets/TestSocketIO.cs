using SocketIOClient;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TestSocketIO : MonoBehaviour
{
    public TMP_InputField message;
    private string serverUrl = "http://stage.socket.medrick.info:3000/stream";

    SocketIOUnity socket;

    void Start()
    {
        Debug.Log("Start to connect");
        var uri = new Uri(serverUrl);
        socket = new SocketIOUnity(uri, new SocketIOOptions
        {
            Query = new Dictionary<string, string>
            {
                {"authorization", "token jwt" }
            }
            ,
            Transport = SocketIOClient.Transport.TransportProtocol.Polling
            
        }) ;

        //socket = new SocketIO(serverUrl);
        socket.OnConnected += Socket_OnConnected;
        socket.Connect();
        // Connect to the server

      //  socket.Connect();
        socket.OnError += Socket_OnError;
        socket.OnReconnectAttempt += Socket_OnReconnectAttempt;
        socket.OnReconnectError += Socket_OnReconnectError;
        socket.OnReconnectFailed += Socket_OnReconnectFailed;
        socket.On("new_user",
            (data) =>
            {
                Debug.Log(data);
            });

        socket.On("chat message",
            callback: (msg) =>
            {
                Debug.Log("message: " + msg);
            }); 
    }

    private void Socket_OnReconnectFailed(object sender, EventArgs e)
    {
        Debug.Log("reconnect failed " + e);
    }

    private void Socket_OnError(object sender, string e)
    {
        Debug.Log(e);
    }

    private void Socket_OnReconnectError(object sender, Exception e)
    {
        Debug.Log("connect error to " + serverUrl + " : " + e);
    }

    private void Socket_OnConnected(object sender, System.EventArgs e)
    {
        Debug.Log("CONNECTEDDDDDDDDDDDD");
        socket.Emit("subscribe", new Dictionary<string,string>(){ { "room", "IDSINGLE" }, { "socketId", "Idris" } });
    }

    private void Socket_OnReconnectAttempt(object sender, int e)
    {
        Debug.Log("retry attemt ");
    }

    void OnDestroy()
    {
        socket.DisconnectAsync();
        Debug.Log("socket close called");

        socket.OnReconnectAttempt -= Socket_OnReconnectAttempt;
        socket.OnReconnectError -= Socket_OnReconnectError;
        socket.OnConnected -= Socket_OnConnected;
        socket.OnError -= Socket_OnError;
        socket.OnReconnectFailed -= Socket_OnReconnectFailed;
    }

    public void OnSendMessage()
    {
        socket.EmitAsync("chat message", message.text);
        Debug.Log("message: " + message.text);
    }
}
