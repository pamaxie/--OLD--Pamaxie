using System;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Folder
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


            Parallel.ForEach(files, (file) =>
            {
                FileInfo fi = new FileInfo(file);
                try
                {
                    using var img = Image.Load(file);
                    var ratio = (float) img.Height / (float) img.Width;
                    img.Mutate(x => x
                        .Resize(450, (int) ratio * 450));
                    img.Save(folder + "\\" + fi.Name, new JpegEncoder());

                    img.Mutate(x => x
                        .Flip(FlipMode.Horizontal));
                    img.Save(folder + "\\2" + fi.Name, new JpegEncoder());

                    img.Mutate(x => x
                        .Flip(FlipMode.Vertical));
                    img.Save(folder + "\\3" + fi.Name, new JpegEncoder());

                    img.Mutate(x => x
                        .ColorBlindness(ColorBlindnessMode.Achromatomaly));
                    img.Save(folder + "\\4" + fi.Name, new JpegEncoder());
                    Console.WriteLine("Converted and generated" + fi.Name);
                }
                catch
                {
                    Console.WriteLine("Failed to convert" + fi);
                }
            });
            Console.WriteLine("=====================");
            Console.WriteLine("Conversion complete press any key to exit");
            Console.Read();
        }
    }
}
