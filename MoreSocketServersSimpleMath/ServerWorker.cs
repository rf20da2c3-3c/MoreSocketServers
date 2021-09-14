using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MoreSocketServersSimpleMath
{
     class ServerWorker
     {
         private const int PORT = 3001;
        public ServerWorker()
        {
        }

        public void Start()
        {
            TcpListener listener = new TcpListener(IPAddress.Loopback,PORT);
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

                String ReqLine = sr.ReadLine();
                if (ReqLine == null)
                {
                    sw.WriteLine("Bad Request 'Operator number number' ");
                    return;
                }

                String[] reqs = ReqLine.Split(" ");
                if (reqs.Length != 3)
                {
                    sw.WriteLine("Bad Request 'Operator number number' ");
                    return;
                }

                int number1 = 0;
                int number2 = 0;

                try
                {
                    number1 = Int32.Parse(reqs[1]);
                    number2 = Int32.Parse(reqs[2]);
                }
                catch (FormatException fe)
                {
                    sw.WriteLine("Bad Request 'Operator number number' ");
                    return;
                }
                switch (reqs[0].ToLower())
                {
                    case "add":
                        sw.WriteLine($"{number1} + {number2} = {(number1 + number2)}");
                        break;

                    case "min":
                        sw.WriteLine($"{number1} - {number2} = {(number1 - number2)}");
                        break;

                    case "mul":
                        sw.WriteLine($"{number1} * {number2} = {(number1 * number2)}");
                        break;

                    case "div":
                        // todo check if number2 == 0
                        sw.WriteLine($"{number1} / {number2} = {(number1 / number2)}");
                        break;

                    default:
                        sw.WriteLine("Bad Request 'Operator number number' ");
                        break;
                }

            }
            socket?.Close();
        }
    }
}