using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Linq;


namespace MCUserStats
{


    /*
     * https://github.com/ldilley/minestat
     * https://docs.microsoft.com/en-us/azure/log-analytics/log-analytics-data-collector-api
     * 
     */

    public static class LogMinecraftStats
    {
        [FunctionName("LogMinecraftStats")]
        public static void Run([TimerTrigger("* * * * *")]TimerInfo myTimer, TraceWriter log) //Every min
        {
            log.Info($"Function executed at: {DateTime.Now}");


            var HOSTIP = "137.117.83.184";
            var WORKSPACE = "621c5c67-acd6-42b9-9e5f-f1a2c9c602a0";
            var KEY = "8vlY3QiRTPVzVrXqZsH3VZsCMN7sKlXVfQPF60GooMtd9AlXXsBzvtSef9hEkd/rw3cy+rz0/wz6r0MdGfkFsw==";

            var oms = new OMS()
            {
                Workspace = WORKSPACE,
                SharedKey = KEY,
                LogName = "Minecraft"
            };


            var ms = new MineStat(HOSTIP, 25565);

            var json = $"{{\"numplayers\":\"{ms.CurrentPlayers}\",\"maxPlayers\":\"{ms.MaximumPlayers}\"}}";
            log.Info(json);

            oms.Log(json);

        }
    }
}