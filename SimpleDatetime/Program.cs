﻿using System;

namespace SimpleDatetime
{
    class Program
    {
        static void Main(string[] args)
        {
            ServerWorker worker = new ServerWorker();
            worker.Start();

            Console.ReadLine();
        }
    }
}
