using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;

public class ConnectingToNEAT : MonoBehaviour
{
    [NonSerialized] public UnityEvent OnCreatingConnection = new UnityEvent();
    [field: SerializeField]
    public AlgorithmShell AlgorithmShell { get; private set; }
    [SerializeField] private string _server_address = "127.0.0.1";
    [SerializeField] private int _port = 12345;

    public delegate void ProcessResponse(string data);
    private TcpClient client;
    private NetworkStream stream;

    public void CreateConnection()
    {
        try
        {
            client = new TcpClient(_server_address, _port);
            stream = client.GetStream();
            Debug.Log("Connected to server");

            OnCreatingConnection.Invoke();
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to connect to server: {e}");
        }
    }


    public async void SendData(string data) { SendData(data, null); }
    public async void SendData(string data, ProcessResponse process)
    {
        try
        {
            byte[] bytesToSend = Encoding.UTF8.GetBytes(data);
            await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);
            //Debug.Log("Data sent to server: " + data);

            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            //Debug.Log("Response from server: " + response);
            if (process != null)
                process(response);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to send/receive data to/from server: {e}");
        }
    }
}
