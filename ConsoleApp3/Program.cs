using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ConsoleApp3
{
    internal class Program
    {
        static void Main()
        {
            StartServer();
            Console.ReadLine();
        }
        public static void StartServer()
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 8888;
            TcpListener listener = new TcpListener(ipAddress, port);

            try
            {
                listener.Start();
                Console.WriteLine("Сервер запущен");


                while (true)
                {
                    TcpClient client = listener.AcceptTcpClient();
                    Console.WriteLine("Новый клиент подключен");

                    int bytes;
                    byte[] userData = new byte[256];
                    StringBuilder responseData = new StringBuilder();
                    NetworkStream stream = client.GetStream();
                    do
                    {
                        bytes = stream.Read(userData, 0, userData.Length);
                        responseData.Append(Encoding.ASCII.GetString(userData, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    string currentTime;
                    byte[] data;
                    if (responseData.ToString().Equals("get current time"))
                    {
                        currentTime = DateTime.Now.ToShortTimeString();
                        data = Encoding.ASCII.GetBytes(currentTime);
                        stream.Write(data, 0, data.Length);
                        Console.WriteLine("Отправлено время-> " + currentTime);
                    }
                    else if (responseData.ToString().Equals("get current date"))
                    {
                        currentTime = DateTime.Now.ToShortDateString();
                        data = Encoding.ASCII.GetBytes(currentTime);
                        stream.Write(data, 0, data.Length);
                        Console.WriteLine("Отправлена дата-> " + currentTime);
                    }
                    client.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                listener.Stop();
            }
        }
    }
}
