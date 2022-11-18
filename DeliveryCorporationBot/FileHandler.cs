using System.Text;
using System.Text.Json;
using System.Runtime.Serialization;

namespace DeliveryCorporationBot
{
    #region FileHandler
    public class FileHandler
    {
        #region static
        static FileHandler _instance;
        public static FileHandler Instance => _instance ?? (_instance = new FileHandler());
        #endregion static
        #region public
        public async Task<string> ReadTextAsync(string filePath)
        {
            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Open, FileAccess.Read, FileShare.Read,
                bufferSize: 4096, useAsync: true))
            {
                StringBuilder sb = new StringBuilder();

                byte[] buffer = new byte[0x1000];
                int numRead;
                while ((numRead = await sourceStream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string text = Encoding.Unicode.GetString(buffer, 0, numRead);
                    sb.Append(text);
                }

                return sb.ToString();
            }
        }

        public async Task WriteTextAsync(string filePath, string text, FileMode mode = FileMode.OpenOrCreate)
        {
            byte[] encodedText = Encoding.Unicode.GetBytes(text);
            using var sourceStream = new FileStream(filePath,
                mode,
                FileAccess.Write,
                FileShare.None,
                bufferSize: 4096,
                useAsync: true);
            sourceStream.SetLength(0);
            await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
        }

        public async Task<long> GetOrCreate(long key, string path)
        {
            if (!ContainsKey(key, path).Result)
            {
                string dataStr = await ReadTextAsync(path);
                var data = JsonSerializer.Deserialize<Data>(dataStr);
                data?.Chats.Add(key);
                dataStr = JsonSerializer.Serialize(data);
                await WriteTextAsync(path, dataStr);
            }

            return key;
        }

        public async Task<List<long>> GetKeys(string path)
        {
            string dataStr = await ReadTextAsync(path);
            var data = JsonSerializer.Deserialize<Data>(dataStr);

            return data?.Chats;
        }

        public async Task<bool> ContainsKey(long key, string path)
        {
            if (!File.Exists(path))
            {
                await WriteTextAsync(path, JsonSerializer.Serialize(new Data()));

                return false;
            }
            var data = JsonSerializer.Deserialize<Data>(await ReadTextAsync(path));

            return data?.Chats.Contains(key) ?? false;
        }
        #endregion
    }
    #endregion

    #region Data
    [DataContract]
    public class Data
    {
        #region public
        [DataMember]
        public List<long> Chats { get; set; }

        public Data()
        {
            Chats = new List<long>();
        }
        #endregion
    }
    #endregion
}
