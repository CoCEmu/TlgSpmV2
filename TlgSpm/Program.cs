using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Messages;
using TLSharp.Core;

namespace TlgSpm
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("==================Powered by IapI.ir==================");
            Thread.Sleep(1000);
            Console.WriteLine("==================More information==================");
            Thread.Sleep(1000);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("https://github.com/CoCEmu/iapi");
            Console.ForegroundColor = ConsoleColor.White;
            Worker wk = new Worker();
            ThreadStart ts = new ThreadStart(wk.Start);
            Thread th = new Thread(ts);
            th.Start();



            /*      while (true)
                 {
            //         string message = Console.ReadLine();
                    switch (message.ToLower())
                     {
                         case "start":
                             {
                                 th = new Thread(ts);
                                 th.Start();
                             }
                             break;
                         case "stop":
                             {
                                 if (th.ThreadState == ThreadState.Running)
                                     th.Abort();
                             }
                             break;
                         case "delete":
                             {
                                 File.Delete("session.dat");
                                 Console.ForegroundColor = ConsoleColor.Green;
                                 Console.WriteLine("Session Deleted");
                                 Console.ForegroundColor = ConsoleColor.White;
                             }
                             break;
                         default:
                             {
                                 Console.WriteLine("What?");
                             }
                             break;
                     }
                 }*/

        }
    }
}