using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Text;
public class Client
{
    static public void Main(string[] Args)
    {
        var port = 50000;
        var multicastIP = IPAddress.Parse("224.0.0.1");
        var remoteEndPoint = new IPEndPoint(multicastIP, port);
        var localEndPoint = new IPEndPoint(IPAddress.Any, port);
        var udpclient = new UdpClient();

        udpclient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
        udpclient.ExclusiveAddressUse = false;
        udpclient.Client.Bind(localEndPoint);
        udpclient.Client.MulticastLoopback = true;
        udpclient.MulticastLoopback = true;
        udpclient.JoinMulticastGroup(multicastIP, IPAddress.Any);

        Thread thread1 = new Thread(() =>
        {
            Console.WriteLine("Nome de usuario: ");
            string username = Console.ReadLine();
            while (true)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write("Digite a mensagem a ser enviada: ");
                string message = Console.ReadLine();
                var model = new Model(username, message);
                string payload = JsonConvert.SerializeObject(model);
                Byte[] buffer = Encoding.Unicode.GetBytes(payload.ToCharArray());
                udpclient.Send(buffer, buffer.Length, remoteEndPoint);
                Console.SetCursorPosition(55, Console.CursorTop);
            }
        });
        Thread thread2 = new Thread(() =>
        {
            IPEndPoint sender = new IPEndPoint(0, 0);
            while (true)
            {
                var data = udpclient.Receive(ref sender);
                var strData = Encoding.Unicode.GetString(data);
                var deserialized = JsonConvert.DeserializeObject<Model>(strData);
                Console.SetCursorPosition(55, Console.CursorTop);
                Console.WriteLine($"[{deserialized.date}:{deserialized.time}] {deserialized.username}: Mensagem-> {deserialized.message}");
            }
        });

        thread1.Start();
        thread2.Start();
    }
}
