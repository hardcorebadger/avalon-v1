using System.IO;

namespace Improbable.Assets
{
    public class FilePersistenceStrategy : MachineCache<byte[]>.IPersistenceStrategy
    {
        public void WriteToCacheFile(string outputCacheFile, byte[] resource)
        {
            File.WriteAllBytes(outputCacheFile, resource);
        }

        public byte[] ReadFromCacheFile(string inputCacheFile)
        {
            return File.ReadAllBytes(inputCacheFile);
        }
    }
}