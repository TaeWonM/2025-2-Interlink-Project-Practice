using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class UdpReceiver : MonoBehaviour
{
    private Thread receiveThread;
    private UdpClient client;
    public int port = 5005; // Python 서버의 포트와 일치해야 함
    public string lastReceivedData = "";

    void Start()
    {
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);
                lastReceivedData = text;
                // Debug.Log("Received: " + lastReceivedData); // 테스트용
            }
            catch (System.Exception err)
            {
                Debug.LogError(err.ToString());
            }
        }
    }

    void OnApplicationQuit()
    {
        if (receiveThread != null)
        {
            receiveThread.Abort();
        }
        if (client != null)
        {
            client.Close();
        }
    }
}