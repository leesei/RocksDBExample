This sample code writes data to RocksDb and read data from it after disposing the handle.  
While the code works in \*nix environment, we face this exception on Windows environment.

https://github.com/curiosity-ai/rocksdb-sharp/issues/5

```
Fatal error. System.Runtime.InteropServices.SEHException (0x80004005): External component has thrown an exception.
   at RocksDbSharp.Native.rocksdb_get(IntPtr, IntPtr, Byte[], Int64, IntPtr ByRef, RocksDbSharp.ColumnFamilyHandle)
   at RocksDbSharp.Native.rocksdb_get(IntPtr, IntPtr, Byte[], Int64, RocksDbSharp.ColumnFamilyHandle)
   at RocksDbSharp.RocksDb.Get(Byte[], Int64, RocksDbSharp.ColumnFamilyHandle, RocksDbSharp.ReadOptions)
   at RocksDbSharp.RocksDb.Get(Byte[], RocksDbSharp.ColumnFamilyHandle, RocksDbSharp.ReadOptions)
   at RocksDBExample.Program.Main(System.String[])
```

## Usage

RocksDb-Sharp's version seems to matter, change `.csproj`, clean and run.

```
dotnet clean; dotnet run
dotnet clean -c Release; dotnet run -c Release
```
