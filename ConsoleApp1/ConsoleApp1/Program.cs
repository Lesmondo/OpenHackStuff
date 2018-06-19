
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;
using System.Threading;


namespace ConsoleApp1
{

    /*
     * Nuget - HTTPDataCollectorAPI https://github.com/ealsur/HTTPDataCollectorAPI
     * 
     * https://github.com/ldilley/minestat
     */

    class Program
    {
        static void Main(string[] args)
        {

            var HOSTIP ="137.117.83.184";
            var WORKSPACE = "621c5c67-acd6-42b9-9e5f-f1a2c9c602a0";
            var KEY = "8vlY3QiRTPVzVrXqZsH3VZsCMN7sKlXVfQPF60GooMtd9AlXXsBzvtSef9hEkd/rw3cy+rz0/wz6r0MdGfkFsw==";



            while (true)
            {
                var oms = new OMS()
                {
                    Workspace = WORKSPACE,
                    SharedKey = KEY,
                    LogName = "Minecraft"
                };


                var ms = new MineStat(HOSTIP, 25565);

                var json = $"{{\"numplayers\":\"{ms.CurrentPlayers}\",\"maxPlayers\":\"{ms.MaximumPlayers}\"}}";
                oms.Log(json);

                Thread.Sleep(5000);
            }

        }
    }
}

