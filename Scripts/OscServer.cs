﻿using UnityEngine;
using UnityEngine.Events;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace uOSC
{

public class OscServer : MonoBehaviour
{
    [SerializeField]
    int port = 3333;

    OscUdp udp_ = new OscUdp();
    OscParser parser_ = new OscParser();
    OscThread thread_ = new OscThread();

    public class DataReceiveEvent : UnityEvent<OscMessage> {};
    public DataReceiveEvent onDataReceived { get; private set; }

    void Awake()
    {
        onDataReceived = new DataReceiveEvent();
    }

    void OnEnable()
    {
        udp_.StartServer(port);
        thread_.Start(UpdateMessage);
    }

    void OnDisable()
    {
        thread_.Stop();
        udp_.Stop();
    }

    void Update()
    {
        while (parser_.messageCount > 0)
        {
            var message = parser_.Dequeue();
            onDataReceived.Invoke(message);
        }
    }

    void UpdateMessage()
    {
        udp_.UpdateServer();

        while (udp_.messageCount > 0) 
        {
            var buffer = udp_.Receive();
            parser_.Parse(buffer);
        }
    }
}

}