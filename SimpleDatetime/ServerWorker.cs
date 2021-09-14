using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SimpleDatetime
{
    class ServerWorker
    {
        private const int PORT = 3003;
        public ServerWorker()
        {
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback, PORT);
            listener.Start();


            while (true)
            {
                TcpClient socket = listener.AcceptTcpClient();
                Task.Run(
                    () =>
                    {
                        TcpClient tmpSocket = socket;
                        DoClient(tmpSocket);
                    }

                );
            }

        }

        private void DoClient(TcpClient socket)
        {

            using (StreamReader sr = new StreamReader(socket.GetStream()))
            using (StreamWriter sw = new StreamWriter(socket.GetStream()))
            {
                sw.AutoFlush = true;

                String reqLine = sr.ReadLine();

                if (reqLine == null)
                {
                    sw.WriteLine("Bad Request correct request follow 'datetime&2020-09-08 14:46' ");
                    return;
                }
                String[] reqs = reqLine.Split("&");
                if (reqs.Length != 2)
                {
                    sw.WriteLine("Bad Request correct request follow 'datetime&2020-09-08 14:46' ");
                    return;
                }

                try
                {
                    DateTime dt = DateTime.ParseExact(reqs[1], "yyyy-MM-dd HH:mm",
                        CultureInfo.InvariantCulture);

                    sw.WriteLine("Datetime is Valid");
                }
                catch (Exception ex)
                {
                    sw.WriteLine("Datetime is NOT Valid must follow this format yyyy-MM-dd HH:mm");
                }


            }
            socket?.Close();
        }
    }
}