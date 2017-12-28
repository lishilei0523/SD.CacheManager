using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace SD.CacheManager.Memcached.Toolkits
{
    /// <summary>
    /// Memcached工具
    /// </summary>
    internal static class MemcachedTool
    {
        /// <summary>
        /// 获取缓存键列表
        /// </summary>
        /// <param name="host">主机名称</param>
        /// <param name="port">端口</param>
        /// <returns>缓存键列表</returns>
        public static IEnumerable<string> GetKeys(string host, int port)
        {
            HashSet<string> keys = new HashSet<string>();

            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                socket.Connect(host, port);

                IEnumerable<string> slabIdIter = QuerySlabId(socket);

                IEnumerable<string> specKeys = QueryKeys(socket, slabIdIter);

                foreach (string key in specKeys)
                {
                    keys.Add(key);
                }

                return keys;
            }
        }

        /// <summary>
        /// 查询键
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="slabIdIter">被查询slabId</param>
        /// <returns>键遍历器</returns>
        private static IEnumerable<String> QueryKeys(Socket socket, IEnumerable<string> slabIdIter)
        {
            var keys = new List<String>();
            var cmdFmt = "stats cachedump {0} 200000 ITEM views.decorators.cache.cache_header..cc7d9 [6 b; 1256056128 s] \r\n";
            var contentAsString = String.Empty;

            foreach (String slabId in slabIdIter)
            {
                contentAsString = ExecuteScalarAsString(socket, string.Format(cmdFmt, slabId));
                keys.AddRange(ParseKeys(contentAsString));
            }

            return keys;
        }

        /// <summary>
        /// 查询片Id
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <returns>slabId遍历器</returns>
        private static IEnumerable<string> QuerySlabId(Socket socket)
        {
            string command = "stats items STAT items:0:number 0 \r\n";
            string contentAsString = ExecuteScalarAsString(socket, command);

            return ParseStatsItems(contentAsString);
        }

        /// <summary>
        /// 执行返回字符串标量
        /// </summary>
        /// <param name="socket">套接字</param>
        /// <param name="command">命令</param>
        /// <returns>执行结果</returns>
        private static string ExecuteScalarAsString(Socket socket, string command)
        {
            socket.Send(Encoding.UTF8.GetBytes(command));
            int bufferSize = 0x1000;
            byte[] buffer = new Byte[bufferSize];

            StringBuilder builder = new StringBuilder();

            while (true)
            {
                int readNumOfBytes = socket.Receive(buffer);
                builder.Append(Encoding.UTF8.GetString(buffer));

                if (readNumOfBytes < bufferSize)
                    break;
            }

            return builder.ToString();
        }

        /// <summary>
        /// 解析stats cachedump返回键
        /// </summary>
        /// <param name="contentAsString">解析内容</param>
        /// <returns>键遍历器</returns>
        private static IEnumerable<string> ParseKeys(string contentAsString)
        {
            List<string> keys = new List<String>();
            string separator = "\r\n";
            char separator2 = ' ';
            string prefix = "ITEM";
            string[] items = contentAsString.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in items)
            {
                string[] itemParts = item.Split(new[] { separator2 }, StringSplitOptions.RemoveEmptyEntries);

                if ((itemParts.Length < 3) || !String.Equals(itemParts.FirstOrDefault(), prefix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                keys.Add(itemParts[1]);
            }

            return keys;
        }

        /// <summary>
        /// 解析STAT items返回slabId
        /// </summary>
        /// <param name="contentAsString">解析内容</param>
        /// <returns>slabId遍历器</returns>
        private static IEnumerable<string> ParseStatsItems(string contentAsString)
        {
            List<string> slabIds = new List<String>();
            string separator = "\r\n";
            char separator2 = ':';
            string[] items = contentAsString.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < items.Length; i += 4)
            {
                string[] itemParts = items[i].Split(new[] { separator2 }, StringSplitOptions.RemoveEmptyEntries);

                if (itemParts.Length < 3)
                    continue;

                slabIds.Add(itemParts[1]);
            }

            return slabIds;
        }
    }
}
