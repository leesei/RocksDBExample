using System;
using System.Linq;
using System.Text;
using RocksDbSharp;

namespace RocksDBExample
{
    class Program
    {
        public static string ByteArrayToHexString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);

            for (int i = 0; i < ba.Length; i++)       // <-- Use for loop is faster than foreach
                hex.Append(ba[i].ToString("X2"));   // <-- ToString is faster than AppendFormat

            return hex.ToString();
        }

        public static byte[] HexStringToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length)
                             .Where(x => x % 2 == 0)
                             .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                             .ToArray();
        }

        private RocksDb db;

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // string path = Environment.ExpandEnvironmentVariables(Path.Combine(temp, "rocksdb_example"));
            string db_path = "rocksdb_example";

            var options = new DbOptions()
                .SetCreateIfMissing(true)
                .EnableStatistics();

            using (var db = RocksDb.Open(options, db_path))
            {
                {
                    // With strings
                    var keys = new string[] { "1", "2", "3" };
                    var values = new string[] { "a", "b", "c" };

                    for (int i = 0; i < keys.Length; i++)
                    {
                        db.Put(keys[i], values[i]);
                    }

                    foreach (var key in keys)
                    {
                        Console.WriteLine($"key[{key}], value:[{db.Get(key)}]");
                    }
                }


                {
                    // With bytes
                    var keys = new int[] { 1, 2, 3 };
                    var values = new string[] { "abc", "efd", "xyz" };

                    for (int i = 0; i < keys.Length; i++)
                    {
                        db.Put(BitConverter.GetBytes(keys[i]), Encoding.UTF8.GetBytes(values[i]));
                    }

                    foreach (var key in keys)
                    {
                        Console.WriteLine($"key[{key}], value:[{Encoding.UTF8.GetString(db.Get(BitConverter.GetBytes(key)))}]");
                    }
                }


                {
                    // With bytes
                    var keys = new int[] { 4, 5, 6 };
                    var value = "85a2494430a84c6f636174696f6e06a84c656973696f6e739187a158cd040ca159cd01e8a157cc91a14848a54c6162656ca56e706d6c79a553636f7265cb3fe0000000000000a4506174689482a158cd0584a159cd015782a158cd0121a159cd01da82a158ccbca159cd032482a158cd04b6a159ccbdae526570726573656e746174697665c2a7536b6970706564c2";

                    foreach (var key in keys)
                    {
                        db.Put(BitConverter.GetBytes(key), HexStringToByteArray(value));
                    }

                    foreach (var key in keys)
                    {
                        Console.WriteLine($"key[{key}], value:[{ByteArrayToHexString(db.Get(BitConverter.GetBytes(key)))}]");
                    }

                    // With buffers
                    var buffer = new byte[500];
                    foreach (var key in keys)
                    {
                        long length = db.Get(BitConverter.GetBytes(key), buffer, 0, buffer.Length);
                        Console.WriteLine($"key[{key}], length[{length}], buffer value[{ByteArrayToHexString(buffer)}]");
                    }
                }
            }

            Console.WriteLine($"\nClose and Reopen Database.\n");

            using (var db = RocksDb.Open(options, db_path))
            {
                var keys = new int[] { 4, 5, 6 };
                foreach (var key in keys)
                {
                    Console.WriteLine($"key[{key}], value:[{ByteArrayToHexString(db.Get(BitConverter.GetBytes(key)))}]");
                }

                var buffer = new byte[500];
                foreach (var key in keys)
                {
                    long length = db.Get(BitConverter.GetBytes(key), buffer, 0, buffer.Length);
                    Console.WriteLine($"key[{key}], length[{length}], buffer value[{ByteArrayToHexString(buffer)}]");
                }
            }

            if (System.IO.Directory.Exists(db_path)) System.IO.Directory.Delete(db_path, true);
        }
    }
}
