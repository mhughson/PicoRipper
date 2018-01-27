using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PicoRipper
{
    [XmlType("map")]
    public class TmxMap
    {
        [XmlAttribute("version")]
        public float Version = 1.0f;

        [XmlAttribute]
        public string tiledversion = "1.0.3";
        [XmlAttribute]
        public string orientation = "orthogonal";
        [XmlAttribute]
        public string renderorder = "right-down";
        [XmlAttribute]
        public int width = 128;
        [XmlAttribute]
        public int height = 64;
        [XmlAttribute]
        public int tilewidth = 8;
        [XmlAttribute]
        public int tileheight = 8;
        [XmlAttribute]
        public int nextobjectid = 1;

        [XmlElement("tileset")]
        public TmxTileSet TileSet = new TmxTileSet();

        [XmlElement("layer")]
        public TmxLayer Layer = new TmxLayer();

        [XmlType("tileset")]
        public class TmxTileSet
        {
            [XmlAttribute("firstgid")]
            public int FirstGid = 1;

            [XmlAttribute("name")]
            public string Name = "SpriteSheet";

            [XmlAttribute("tilewidth")]
            public int TileWidth = 8;

            [XmlAttribute("tileheight")]
            public int TileHeight = 8;

            [XmlAttribute("tilecount")]
            public int TileCount = 256;

            [XmlAttribute("columns")]
            public int Columns = 16;

            [XmlElement("image")]
            public TmxImage Image = new TmxImage();

            [XmlType("image")]
            public class TmxImage
            {
                // TODO: Args
                [XmlAttribute("source")]
                public string Source = "Dev/Game/mono8/Mono8Samples/Content/raw/sheet.png";

                [XmlAttribute("width")]
                public int Width = 128;

                [XmlAttribute("height")]
                public int Height = 128;
            }

            [XmlElement("tile")]
            public List<TmxTile> TileList = new List<TmxTile>();

            [XmlType("tile")]
            public class TmxTile
            {
                [XmlAttribute("id")]
                public int ID = 0;

                [XmlElement("properties")]
                public TmxTileProperties Properties;

                [XmlType("properties")]
                public class TmxTileProperties
                {
                    [XmlElement("property")]
                    public List<TmxTileProperty> PropertyList = new List<TmxTileProperty>();

                    [XmlType("property")]
                    public class TmxTileProperty
                    {
                        [XmlAttribute("name")]
                        public string Name = "Flag";

                        [XmlAttribute("type")]
                        public string Type = "int";

                        [XmlAttribute("value")]
                        public string Value = "";
                    }
                }
            }
        }

        [XmlType("layer")]
        public class TmxLayer
        {
            [XmlAttribute("name")]
            public string Name = "Tile Layer 1";

            [XmlAttribute("width")]
            public int Width = 128;

            [XmlAttribute("height")]
            public int Height = 64;

            [XmlElement(ElementName = "data")]
            public TmxData Data = new TmxData();

            [XmlType("data")]
            public class TmxData
            {
                [XmlAttribute("encoding")]
                public string Encoding = "csv";

                [XmlText]
                public string MapData = "unknown";
            }
        }
    }
}
