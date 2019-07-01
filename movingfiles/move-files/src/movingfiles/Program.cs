using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace movingfiles
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length < 2)
                    throw new Exception("obrigatorio ter dois caminhos de pasta: origem e destino");

                string strSource = args[0];
                string strDestination = args[1];

                if(!Directory.Exists(strSource))
                    throw new Exception($"caminho origem nao existe: {strSource}");

                if (!Directory.Exists(strDestination))
                    throw new Exception($"caminho destino nao existe: {strDestination}");

                DirectoryCopy(strSource, strDestination, true);

                //List<string> files = Directory.GetFiles(strSource, "*.*", SearchOption.AllDirectories).ToList();
                //foreach (string file in files)
                //{
                //    try
                //    {
                //        string strDestinationFull = Path.Combine(strDestination, Path.GetRelativePath(Path.GetDirectoryName(file), strSource));
                //        if (!Directory.Exists(strDestinationFull))
                //            Directory.CreateDirectory(strDestinationFull);

                //        File.Move(file, Path.Combine(strDestinationFull, Path.GetFileName(file)));
                //    }
                //    catch (Exception ex)
                //    {
                //        Console.WriteLine($"ERRO: {ex.Message}");
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

#if DEBUG
            Console.ReadLine();
#endif
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {

            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.MoveTo(temppath);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }

    }
}
