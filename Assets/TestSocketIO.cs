using SocketIOClient;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestSocketIO : MonoBehaviour
{
    public TMP_InputField message;
    private string serverUrl = "https://stage.socket.medrick.info/";

    SocketIO socket;

    void Start()
    {
        Debug.Log("Start to connect");
        var uri = new Uri(serverUrl);
        //socket = new SocketIOUnity(uri, new SocketIOOptions
        //{
        //    Query = new Dictionary<string, string>
        //{
        //    {"token", "UNITY" }
        //}
        //    ,
        //    Transport = SocketIOClient.Transport.TransportProtocol.WebSocket
        //});

        socket = new SocketIO(serverUrl);
        socket.OnConnected += Socket_OnConnected;
        socket.ConnectAsync();
        // Connect to the server

      //  socket.Connect();
        socket.OnError += Socket_OnError;
        socket.OnReconnectAttempt += Socket_OnReconnectAttempt;
        socket.OnReconnectError += Socket_OnReconnectError;
        socket.OnConnected += Socket_OnConnected;
        socket.OnReconnectFailed += Socket_OnReconnectFailed;

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

        socket.EmitAsync("chat message", "Hello from Unity!");
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
