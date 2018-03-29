using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Weather_app
{
    public partial class Form1 : Form
    {

        const string FILENAME = @"C:\Proiect18\Weather_app\weather_test.xml";
        Current current = null;

        public Form1()
        {
            InitializeComponent();

            current = new Current()
            {
                city = new City()
                {
                    name = "London",
                    id = 2643743,
                    coord = new Coord() { lat = 51.51, lon = -0.13 },
                    country = "GB",
                    sun = new Sun() { set = DateTime.Parse("2015-02-09T17:04:38"), rise = DateTime.Parse("2015-02-09T07:24:47") }
                },
                temperature = new Temperature() { value = 275.75, unit = "kelvin", max = 275.75, min = 275.75 }

            };

            XmlSerializer serializer = new XmlSerializer(typeof(Current));

            StreamWriter writer = new StreamWriter(FILENAME);
            serializer.Serialize(writer, current);
            writer.Flush();
            writer.Close();
            writer.Dispose();

            XmlSerializer xs = new XmlSerializer(typeof(Current));
            XmlTextReader reader = new XmlTextReader(FILENAME);
            Current readCurrent = (Current)xs.Deserialize(reader);

        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            string applicationId = "708555ac193da31f51750042dde1a820";
            string sampleUrl = "http://samples.openweathermap.org/data/2.5/forecast?q=" + textBox1.Text + "&mode=xml&appid=b6907d289e10d714a6e88b30761fae22";
            string licenta_url = "http://api.openweathermap.org/data/2.5/forecast?q=" + textBox1.Text + "&mode=xml&appid=" + applicationId;

            string weburl = licenta_url;

            var xml = await new WebClient().DownloadStringTaskAsync(new Uri(weburl));

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlNode forecast = doc.DocumentElement.SelectSingleNode("forecast");

            string szTemp = "00.00";
            foreach (XmlNode time_of_forecast in forecast.SelectNodes("time"))
            {
                if(time_of_forecast.Attributes["from"].Value == "2018-03-29T15:00:00")
                {
                    szTemp = time_of_forecast.SelectSingleNode("temperature ").Attributes["value"].Value;
                }
            }

            //XmlNode time_of_forecast = forecast.SelectSingleNode("time");
            double temp = double.Parse(szTemp) - 272.5;
            label1.Text = temp.ToString("N2") + " Kelvin";

        }

        [Serializable, XmlRoot("Current")]
        public class Current
        {
            public City city { get; set; }
            public Temperature temperature { get; set; }
        }

        [XmlRoot(ElementName = "city")]
        public class City
        {
            [XmlAttribute("name")]
            public string name { get; set; }
            [XmlAttribute("id")]
            public int id { get; set; }
            public Coord coord { get; set; }
            public string country { get; set; }
            public Sun sun { get; set; }
        }

        [XmlRoot(ElementName = "coor")]
        public class Coord
        {
            [XmlAttribute("lat")]
            public double lat { get; set; }
            [XmlAttribute("lon")]
            public double lon { get; set; }
        }

        [XmlRoot(ElementName = "sun")]
        public class Sun
        {
            [XmlAttribute("set")]
            public DateTime set { get; set; }
            [XmlAttribute("rise")]
            public DateTime rise { get; set; }
        }

        [XmlRoot(ElementName = "temperature")]
        public class Temperature
        {
            [XmlAttribute("value")]
            public double value { get; set; }
            [XmlAttribute("unit")]
            public string unit { get; set; }
            [XmlAttribute("max")]
            public double max { get; set; }
            [XmlAttribute("min")]
            public double min { get; set; }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

    }
}
