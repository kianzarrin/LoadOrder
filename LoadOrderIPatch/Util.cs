using Mono.Cecil;
using System.IO;

namespace LoadOrderIPatch
{
    internal static class Util
    {
        internal static AssemblyDefinition GetAssemblyDefinition(string dirPath, string fileName)
        {
            DefaultAssemblyResolver resolver = new DefaultAssemblyResolver();
            resolver.AddSearchDirectory(dirPath);
            var dllPath = Path.Combine(dirPath, fileName);
            var readerParams = new ReaderParameters { AssemblyResolver = resolver };
            return AssemblyDefinition.ReadAssembly(dllPath, readerParams);
        }
    }
}
