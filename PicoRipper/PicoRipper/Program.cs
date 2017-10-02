using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace PicoRipper
{
    class Program
    {
        /// <summary>
        /// Defines the configuration of the app. Loads configuration from command line args.
        /// </summary>
        public class Config
        {
            /// <summary>
            /// The path and file of the P8 file to load.
            /// </summary>
            /// <remarks>REQUIRED!</remarks>
            public string P8FilePath { get; private set; }

            /// <summary>
            /// The path and file where the TMX file will be saved to.
            /// </summary>
            public string TMXFilePath { get; private set; }

            /// <summary>
            /// The path and file where the sprite sheet will be saved to.
            /// </summary>
            public string SpritePath { get; private set; }

            public Config(string [] Args)
            {
                Parse(Args);

                Dump();
            }

            /// <summary>
            /// Takes the array of arguments passed into the application, and populates
            /// config based on keywords.
            /// </summary>
            /// <param name="Args"></param>
            public void Parse(string[] Args)
            {
                // Handle someone just running the exe with no args.
                if (Args.Length <= 0)
                {
                    Console.WriteLine("Fatal: No arguments passed to program. Path to P8 file required.");
                    return;
                }

                // ArgIndex.
                int i = 0;

                // We use the P8 path and filename to drive the defaults for the
                // outputs of the ripper (basically copy the name and drop output in the
                // directory that the P8 file came from).
                Action OnP8Found = () =>
                { 
                    if (TMXFilePath == null)
                    {
                        TMXFilePath = Helper.GetFullPathWithoutExtension(P8FilePath) + ".tmx";
                    }
                    if (SpritePath == null)
                    {
                        SpritePath = Helper.GetFullPathWithoutExtension(P8FilePath) + ".png";
                    }
                };

                // Special case for dragging a p8 file on to the exe.
                // get the file attributes for file or directory
                if (Path.GetExtension(Args[i]) == ".p8")
                {
                    P8FilePath = Args[i++];

                    OnP8Found();
                }

                for (; i < Args.Length; i++)
                {
                    string Str = Args[i];

                    if (string.Compare(Str, "-p8", true) == 0)
                    {
                        P8FilePath = Args[++i];

                        OnP8Found();
                    }
                    else if (string.Compare(Str, "-tmx", true) == 0)
                    {
                        TMXFilePath = Args[++i];
                    }
                    else if (string.Compare(Str, "-img", true) == 0)
                    {
                        SpritePath = Args[++i];
                    }
                }
            }

            public void Dump()
            {
                Console.WriteLine("===========");
                Console.WriteLine("P8FilePath: \t" + P8FilePath);
                Console.WriteLine("TMXFilePath: \t" + TMXFilePath);
                Console.WriteLine("SpritePath: \t" + SpritePath);
                Console.WriteLine("===========");
            }
        }

        public static TmxMap ActiveMap { get; private set; }

        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">
        /// Options:
        /// [filepath]          (Path to the P8 file to rip - used in drag n drop case)
        /// -p8 [filepath]      (Path to the P8 file to rip - overrides above)
        /// -tmx [filepath]     (Output path of TMX Map file - default: same as p8)
        /// -img [filepath]     (Output path of Sprite file - default: same as p8)
        /// </param>
        static void Main(string[] args)
        {
            Console.WriteLine("********STARTING PICO RIPPER********");

            Config Config = new Config(args);

            // Early out in case P8 file was not specified.
            if (Config.P8FilePath == null)
            {
                Console.WriteLine("Fatal: Unable to find P8 file path.");
                goto End;
            }

            TmxMap ActiveMap = new TmxMap();
            P8Scraper P8 = new P8Scraper();

            if (!P8.RipAllData(ActiveMap, Config.P8FilePath, Config.TMXFilePath, Config.SpritePath))
            {
                Console.WriteLine("Fatal: Failed to rip data.");
                goto End;
            }

            End:
            Console.WriteLine("***************DONE*****************");
            Console.ReadKey();
        }
    }
}
