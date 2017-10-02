using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace PicoRipper
{
    /// <summary>
    /// Methods for scraping different pieces of data out of a *.p8 Pico 8 file.
    /// </summary>
    class P8Scraper
    {
        public bool RipAllData(TmxMap ActiveMap, string P8Path, string TMXPath, string SpritePath)
        {
            if (!File.Exists(P8Path))
            {
                Console.WriteLine("Fatal: P8 file path does not exist.");
                return false;
            }
            string P8Text = File.ReadAllText(P8Path);

            RipMap(P8Text, ActiveMap, TMXPath, SpritePath);

            RipSpriteSheet(P8Text, SpritePath);

            return true;
        }

        void RipMap(string P8Text, TmxMap ActiveMap, string TMXPath, string SpritePath)
        {
            string MapText = P8Text.Substring(P8Text.LastIndexOf("__map__") + "__map__".Length);
            MapText = MapText.Remove(MapText.IndexOf("__"));

            MapText = Regex.Replace(MapText, @"\t|\n|\r", "");

            // Map data in GFX section
            string GfxText = P8Text.Substring(P8Text.LastIndexOf("__gfx__") + "__gfx__".Length);
            GfxText = GfxText.Remove(GfxText.IndexOf("__"));

            GfxText = Regex.Replace(GfxText, @"\t|\n|\r", "");

            // Map data starts half way through gfx data.
            GfxText = GfxText.Remove(0, GfxText.Length / 2);

            string Temp = "";
            // Reverse
            for (int i = 0; i < GfxText.Length; i += 2)
            {
                string str = GfxText.Substring(i, 2);
                char[] charArray = str.ToCharArray();
                Array.Reverse(charArray);
                Temp += new string(charArray);
            }
            GfxText = Temp;

            byte[] MapBytes = StringToByteArray(MapText + GfxText);

            string MapTmxString = "\n";
            for (int i = 0; i < MapBytes.Length; i++)
            {
                // +1 because Tiled uses index 1 to represent sprite 0.
                MapTmxString += (MapBytes[i] + 1).ToString();
                MapTmxString += ',';
                if (i != 0 && i % 128 == 0)
                {
                    MapTmxString += '\n';
                }
            }
            // Remove extra characters.
            ActiveMap.Layer.Data.MapData = MapTmxString.Remove(MapTmxString.LastIndexOf(',')) + "\n";

            ActiveMap.TileSet.Image.Source = SpritePath;

            // Save the map out to file.
            XmlSerializer XmlSerial = new XmlSerializer(ActiveMap.GetType());
            XmlSerializerNamespaces NS = new XmlSerializerNamespaces();
            NS.Add("", ""); // Remove namespace tags: https://stackoverflow.com/questions/625927/omitting-all-xsi-and-xsd-namespaces-when-serializing-an-object-in-net

            var Path = TMXPath;
            FileStream File = System.IO.File.Create(Path);

            using (StreamWriter sw = new StreamWriter(File, Encoding.GetEncoding("UTF-8")))
            {
                XmlSerial.Serialize(sw, ActiveMap, NS);
            }
        }

        void RipSpriteSheet(string P8Text, string SpritePath)
        {
            string GfxText = P8Text.Substring(P8Text.LastIndexOf("__gfx__") + "__gfx__".Length);
            GfxText = GfxText.Remove(GfxText.IndexOf("__"));

            GfxText = Regex.Replace(GfxText, @"\t|\n|\r", "");

            byte[] MapBytes = new byte[128 * 128];
            for(int i = 0; i < GfxText.Length; i++)
            {
                MapBytes[i] = Convert.ToByte(GfxText.Substring(i,1), 16);
            }

            Color[] Colors = new Color[]
            {
                Color.FromArgb(0,0,0),
                Color.FromArgb(29,43,83),    // Dark Blue
                Color.FromArgb(126,37,83),   // Dark Purple
                Color.FromArgb(0,135,81),    // Dark Green
                Color.FromArgb(171,82,54),   // Brown
                Color.FromArgb(95,87,79),    // Dark Grey
                Color.FromArgb(194,195,199), // Light Grey
                Color.FromArgb(255,241,232), // White
                Color.FromArgb(255,0,77),    // Red
                Color.FromArgb(255,163,0),   // Orange
                Color.FromArgb(255,236,39),  // Yellow
                Color.FromArgb(0,228,54),    // Green
                Color.FromArgb(41,173,255),  // Blue
                Color.FromArgb(131,118,156), // Indigo
                Color.FromArgb(255,119,168), // Pink
                Color.FromArgb(255,204,170)  // Peach
            };

            using (Bitmap b = new Bitmap(128, 128))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.Clear(Color.Magenta);

                    for (int i = 0; i < MapBytes.Length; i++)
                    {
                        SolidBrush B = new SolidBrush(Colors[MapBytes[i]]);
                        g.FillRectangle(B, i % 128, (float)Math.Floor((float)i / 128.0f), 1, 1);
                    }
                }
                b.Save(SpritePath, ImageFormat.Png);
            }


            //byte[] MapBytes = StringToByteArray(GfxText);
            // VERSION 1
            //unsafe
            //{
            //    fixed (byte* ptr = MapBytes)
            //    {
            //        using (Bitmap image = new Bitmap(128, 128,128,PixelFormat.Format32bppRgb,new IntPtr(ptr)))
            //        {
            //            image.Save(@"temp_sheet.png");
            //        }
            //    }
            //}

            //// VERSION 2
            //byte[] bitmap = new byte[] { 255, 0, 0, 255 }; //MapBytes;

            //MemoryStream S = new MemoryStream(bitmap);
            //using (Image image = Image.FromStream(S))
            //{
            //    image.Save("temp_sheet.png", ImageFormat.Png);  // Or Png
            //}

            // VERSION 3
            //byte[] RGBBytes = new byte[4] { 0, 0, 0, 0 };

            //System.Drawing.ImageConverter converter = new System.Drawing.ImageConverter();
            //Image img = (Image)converter.ConvertFrom(RGBBytes);
            //img.Save(@"temp_sheet.png", ImageFormat.Png);

        }

        /// <summary>
        /// Converts pairs of characters to bytes (eg. "FF" becomes 255).
        /// </summary>
        /// <param name="Hex">String of Hex in string form.</param>
        /// <returns></returns>
        byte[] StringToByteArray(string Hex)
        {
            int NumberChars = Hex.Length;
            // Divide by 2 because we are converting pairs of characters into a single byte.
            byte[] Bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                Bytes[i / 2] = Convert.ToByte(Hex.Substring(i, 2), 16);
            }
            return Bytes;
        }
    }
}
