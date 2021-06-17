using System;
using System.IO;
using System.Linq;
using System.Net.Mime;

namespace ImageRenaming
{
    class Program
    {
        static void Main(string[] args)
        {
            string folder;
            while (true)
            {
                Console.WriteLine("Please select a directory with the Images inside them.");
                folder = Console.ReadLine();
                if (Directory.Exists(folder)) break;
                Console.WriteLine("Folder could not be found");
            }

            if (folder == string.Empty) return;

            var files = Directory.GetFiles(folder).ToList();
            Console.WriteLine("Found " + files.Count() + "Files");

            while (true)
            {
                Console.WriteLine("Please select a target directory where the Images will be copied to");
                folder = Console.ReadLine();
                if (Directory.Exists(folder)) break;
                Console.WriteLine("Folder could not be found");
            }
            if (folder == string.Empty) return;


            foreach (var file in files)
            {
                FileInfo fi = new FileInfo(file);
                Guid guid = Guid.NewGuid();
                try
                {
                    fi.CopyTo(folder + "\\" + guid + fi.Extension);
                    Console.WriteLine("Copied " + fi.Name);
                }
                catch
                {
                    Console.WriteLine("Failed to copy " + fi.Name);
                }

            }
            Console.WriteLine("=====================");
            Console.WriteLine("Copy Process complete press any key to exit");
            Console.ReadKey();
        }
    }
}
