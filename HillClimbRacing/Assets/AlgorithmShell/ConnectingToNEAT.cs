using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class ConnectingToNEAT : MonoBehaviour
{
    [NonSerialized] public UnityEvent OnCreatingConnection = new UnityEvent();
    [field: SerializeField]
    public AlgorithmShell AlgorithmShell { get; private set; }
    [SerializeField] private string _server_address = "127.0.0.1";
    [SerializeField] private int _port = 12345;
    
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


    public async void SendData(RequestData data)
    {
        try
        {
            byte[] bytesToSend = Encoding.UTF8.GetBytes(data.GetJson());
            await stream.WriteAsync(bytesToSend, 0, bytesToSend.Length);

            byte[] buffer = new byte[4096];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            data.ProcessFunction?.Invoke(response);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to send/receive data to/from server: {e}");
        }
    }
}

public class RequestData
{
    public string Command = "";
    public string Data = "";
    public ProcessResponse ProcessFunction = null;
    public delegate void ProcessResponse(string data);

    public string GetJson()
    {
        return $"{{\"command\": \"{Command}\", \"data\": {Data}}}";
    }

    public static Builder GetBuilder() { return new Builder(); }
    public class Builder
    {
        private RequestData _request_data;
        public Builder() { _request_data = new RequestData(); }
        public Builder SetCommand(string command)
        {
            _request_data.Command = command;
            return this;
        }
        public Builder SetData(string data)
        {
            _request_data.Data = data;
            return this;
        }
        public Builder SetProcessFunction(ProcessResponse process_function)
        {
            _request_data.ProcessFunction = process_function;
            return this;
        }
        public RequestData Build() { return _request_data; }
    }
}