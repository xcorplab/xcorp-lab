using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Printlaser.Infra.Crosscutting.Compression
{
    public class Compact
    {
        public static byte[] DeflateByte(byte[] str)
        {
            if (str == null)
            {
                return null;
            }

            using (var output = new MemoryStream())
            {
                using (
                    var compressor = new Ionic.Zlib.DeflateStream(
                    output, Ionic.Zlib.CompressionMode.Compress,
                    Ionic.Zlib.CompressionLevel.BestSpeed))
                {
                    compressor.Write(str, 0, str.Length);
                }

                return output.ToArray();
            }
        }

        public static byte[] ToGZipByte(byte[] content)
        {
            if (content == null)
            {
                return null;
            }

            using (var output = new MemoryStream())
            {
                using (
                    var compressor = new Ionic.Zlib.GZipStream(
                    output, Ionic.Zlib.CompressionMode.Compress,
                    Ionic.Zlib.CompressionLevel.BestSpeed))
                {
                    compressor.Write(content, 0, content.Length);
                }

                return output.ToArray();
            }
        }

        public static byte[] FromGZipByte(byte[] content)
        {
            if (content == null)
            {
                return null;
            }

            using (Ionic.Zlib.GZipStream stream = new Ionic.Zlib.GZipStream(new MemoryStream(content),
            Ionic.Zlib.CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    return memory.ToArray();
                }
            }
        }

        public void DescompactarArquivoZIP(FileInfo DiretorioArquivo)
        {
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(DiretorioArquivo.FullName))
            {
                zip.ExtractAll(DiretorioArquivo.DirectoryName);
            }
        }

        public static void CompactarArquivoZIP(FileInfo diretorioArquivo)
        {
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {                                                                                   
                zip.AddFile(diretorioArquivo.FullName, "");
                zip.Save(diretorioArquivo.FullName + ".ZIP");
            }
        }

        public static void CompactarArquivosZIP(DirectoryInfo diretorio, string nomeArquivo)
        {
            using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
            {
                zip.AddDirectory(diretorio.FullName, "");
                zip.Save(Path.Combine(diretorio.FullName, nomeArquivo + ".ZIP"));
            }
        }
    }
}
