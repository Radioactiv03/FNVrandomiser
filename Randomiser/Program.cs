using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Randomiser.Filtering;
using SharpBSABA2;
using SharpBSABA2.BA2Util;

namespace Randomiser
{
    class Program
    {

        static List<IFilterPredicate> _filters = new List<IFilterPredicate>();
        static List<string> files = new List<string>();
        static string path;
        static int count = 0;
        static int line = 0;
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine(@"Open Fallout New Vegas Data Folder eg: D:\Steam\steamapps\common\Fallout New Vegas\Data\");
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            do
            {
                //fbd.SelectedPath = @"D:\Steam\steamapps\common\Fallout New Vegas\Data";
                fbd.ShowDialog();

            } while (fbd.SelectedPath == String.Empty);

            path = fbd.SelectedPath;

            if (!path.Contains(@"\Data"))
            {
                path += @"\Data";
            }
            Console.Clear();
            Console.WriteLine("Do you want to Extract Texture BSA(s)  Y/N");
            string texture = Console.ReadLine();


            if (texture.ToUpper() == "Y")
            {
                files = Directory.GetFiles(path).ToList();
                files.RemoveAll(item => item.EndsWith(".bsa") == false);
                files.RemoveAll(item => item.Contains("Fallout - Tex") == false);

                line = files.Count + 1;
                // Parse arguments. Go to exit if null, errors has occurred and been handled
                foreach (var item in files)
                {
                    Console.WriteLine(item);
                }

                try
                {
                    ExtractFiles(files, path + @"\RandomisedFiles", true);
                }
                catch
                {
                }
            }
            Console.Clear();
            Console.WriteLine("Do you want to Extract Sound BSA(s) - (Voices are separate) Y/N");
            string Sound = Console.ReadLine();
            if (Sound.ToUpper() == "Y")
            {
                files = Directory.GetFiles(path).ToList();
                files.RemoveAll(item => item.EndsWith(".bsa") == false);
                files.RemoveAll(item => item.Contains("Fallout - Sound") == false);

                line = files.Count + 1;
                // Parse arguments. Go to exit if null, errors has occurred and been handled
                foreach (var item in files)
                {
                    Console.WriteLine(item);
                }

                try
                {
                    ExtractFiles(files, path + @"\RandomisedFiles", true);
                }
                catch
                {
                }
            }
            Console.Clear();
            Console.WriteLine("Do you want to Extract Voices BSA  Y/N");
            string Voice = Console.ReadLine();
            if (Voice.ToUpper() == "Y")
            {
                files = Directory.GetFiles(path).ToList();
                files.RemoveAll(item => item.EndsWith(".bsa") == false);
                files.RemoveAll(item => item.Contains("Fallout - Voices1") == false);

                line = files.Count + 1;
                // Parse arguments. Go to exit if null, errors has occurred and been handled
                foreach (var item in files)
                {
                    Console.WriteLine(item);
                }

                try
                {
                    ExtractFiles(files, path + @"\RandomisedFiles", true);
                }
                catch
                {
                }
            }

            Console.Clear();
            //Randomise here!
            Console.WriteLine("Randomise Textures Y/N");
            string randomise = Console.ReadLine();
            if (randomise.ToUpper() == "Y")
            {
                Randomise("textures");
            }
            Console.Clear();
            Console.WriteLine("Randomise Sounds - (Voices are separate) Y/N");
            randomise = Console.ReadLine();
            if (randomise.ToUpper() == "Y")
            {
                Randomise(@"sound\fx");
            }
            Console.Clear();
            Console.WriteLine("Randomise Voices Y/N");
            randomise = Console.ReadLine();
            if (randomise.ToUpper() == "Y")
            {
                Randomise(@"sound\voice");
            }
            Console.Clear();
            Console.WriteLine("\n\n\nRandomizing Complete, Press any key to exit");
            Console.ReadKey();
        }

        public static List<string> Shuffle(List<string> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
        static void Randomise(string type)
        {
            Console.Clear();
            Console.WriteLine("Randomising Files - Please Wait");
            try
            {
                Directory.Delete(path + $@"\RandomisedFiles\{type}\interface\main", true);
            }
            catch { }
            try
            {
                Directory.Delete(path + $@"\RandomisedFiles\Sound\fx\fst", true);
                
            }
            catch { }
            try
            {
                Directory.Delete(path + $@"\RandomisedFiles\Sound\fx\amb", true);
            }
            catch { }
            try
            {

                File.Delete(path + $@"\RandomisedFiles\{type}\pipboy3000\screenglare.dds");
            }
            catch { }
            List<string> files = new List<string>();
            List<string> temp = new List<string>();
            try
            {
                files = Directory.GetFiles(path + $@"\RandomisedFiles\{type}", "*.*", SearchOption.AllDirectories).ToList();
                temp = Directory.GetFiles(path + $@"\RandomisedFiles\{type}", "*.*", SearchOption.AllDirectories).ToList();
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to close");
                Console.ReadKey();
                Application.Exit();
            }
            foreach (var item in files)
            {
                if (type == "textures")
                {
                    if (!item.EndsWith(".dds"))
                    {
                        File.Delete(item);
                    }
                    else if (item.EndsWith("_n.dds") || item.Contains("_n.dds"))
                    {
                        File.Delete(item);
                    }
                }
                if (type == @"sound\fx")
                {
                    
                    
                }

            }
            if (type == "textures")
            {
                files.RemoveAll(item => item.EndsWith("_n.dds") == true);
                files.RemoveAll(item => item.EndsWith("_m.dds") == true);
                files.RemoveAll(item => item.EndsWith(".dds") == false);
                temp.RemoveAll(item => item.EndsWith("_n.dds") == true);
                temp.RemoveAll(item => item.EndsWith("_m.dds") == true);
                temp.RemoveAll(item => item.EndsWith(".dds") == false);
            }
            if(type==@"sound\voice")
            {
                files.RemoveAll(item => item.EndsWith(".lip") == true);
                temp.RemoveAll(item => item.EndsWith(".lip") == true);

            }
            temp = Shuffle(temp);
            for (int i = 0; i < temp.Count; i++)
            {
                File.Move(temp[i], files[i] + ".temp");
            }
            foreach (var file in Directory.GetFiles(path + @"\RandomisedFiles", "*.*", SearchOption.AllDirectories))
            {
                File.Move(file, file.Replace(".temp", ""));
            }

            DirectoryInfo source = new DirectoryInfo(path + $@"\{type}\");
            DirectoryInfo target = new DirectoryInfo(path + $@"\{type}+old\");
            DirectoryInfo tempdir = new DirectoryInfo(path + $@"\RandomisedFiles\{type}\");
            DirectoryInfo newdir = new DirectoryInfo(path + $@"\{type}\");
            CopyAll(tempdir, newdir);

            try
            {
                Directory.Delete(path + $@"\RandomisedFiles\{type}", true);
            }
            catch { }
            Console.WriteLine("Finished Randomising");
        }


        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (source.FullName.ToLower() == target.FullName.ToLower())
            {
                return;
            }

            // Check if the target directory exists, if not, create it.
            if (!Directory.Exists(target.FullName))
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it's new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                //Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }



        static void ExtractFiles(List<string> archives, string destination, bool overwrite)
        {
            archives.ForEach(archivePath =>
            {
                Archive archive = null;
                count = 0;
                archive = OpenArchive(archivePath);


                var files = archive.Files.Where(x => Filter(x.FullPath)).ToList();
                foreach (var file in files)
                {

                    string output = $"Extracting: " + (count++) + "/" + (files.Count() - 1);
                    if (line > -1)
                    {
                            //Console.SetCursorPosition(0, line);
                        Console.SetCursorPosition(0, Console.CursorTop);
                        Console.Write(output.PadRight(10));
                    }
                    try
                    {
                        file.Extract(destination, true);
                    }
                    catch
                    {

                    }
                }
            });
        }

        static bool Filter(string input)
        {
            foreach (var filter in _filters)
            {
                if (filter.Match(input) == false)
                    return false;
            }

            return true;
        }

        static Archive OpenArchive(string file)
        {
            Archive archive;
            archive = new SharpBSABA2.BSAUtil.BSA(file, System.Text.Encoding.UTF7);
            archive.Files.Sort((a, b) => string.CompareOrdinal(a.LowerPath, b.LowerPath));
            return archive;
        }

        static void HandleUnsupportedTextures(List<ArchiveEntry> files)
        {
            for (int i = files.Count; i-- > 0;)
            {
                if (files[i] is BA2TextureEntry tex && tex.IsFormatSupported() == false)
                {
                    files.RemoveAt(i); // Remove unsupported textures to skip them
                }
            }
        }

        static string FormatBytes(long bytes)
        {
            const int scale = 1024;
            string[] orders = new string[] { "GB", "MB", "KB", " B" };
            long max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:#.00} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }

        static string FormatPrefix(ListOptions options, Archive archive)
        {
            string prefix = string.Empty;

            if (options.HasFlag(ListOptions.Archive))
                prefix = Path.GetFileName(archive.FullPath);

            if (options.HasFlag(ListOptions.FullPath))
                prefix = Path.GetFullPath(archive.FullPath);
            return prefix;
        }
    }
}
