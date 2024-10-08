﻿using System;
using System.IO;
using System.Xml;
using System.Drawing;
using Microsoft.Win32;
using System.Data.SQLite;
using System.Collections;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace DetectionZones
{
    public partial class Ui : Form
    {
        public Ui()
        {
            InitializeComponent();
        }

        string installDir = @"C:\Vocord\Vocord.Traffic Crossroads\";
        string screenshotDir = @"E:\Screenshots";

        class CarFilePoint
        {
            public string file;
            public Int16 x;
            public Int16 y;
            public Int16 width;
            public Int16 height;
        }

        class CarTrackPoint
        {
            public Int16 x;
            public Int16 y;
        }

        class Carfile
        {
            public string channelId;
            public string patchfile;
            public CarTrackPoint[] point { get; set; } = new CarTrackPoint[200];
        }

        class ChannelZone
        {
            public string name;
            public Int16 type;
            public Int16 x1;
            public Int16 y1;
            public Int16 x2;
            public Int16 y2;
            public Int16 x3;
            public Int16 y3;
            public Int16 x4;
            public Int16 y4;
        }

        class ChannelNameZone
        {
            public string channelName;
            public Int16 count;
            public ChannelZone[] zones { get; set; } = new ChannelZone[100];
        }

        Hashtable channel = new Hashtable(); // камеры на съемнике 
        Hashtable cars = new Hashtable(); // найденые проезды
        Hashtable imagesCar = new Hashtable(); // найденые картинки проезда
        Hashtable imageNames = new Hashtable(); // имена найденых картинок проезда

        string NameCreation(string name)
        {
            Regex regex = new Regex(@"-\d{1}$");
            if (regex.IsMatch(name))
            {
                int number = (int.Parse(name.Substring(name.IndexOf("-") + 1))) + 1;
                name = name.Remove(name.LastIndexOf("-") + 1) + number.ToString("0");
            }
            else
            {
                name = name + "-2";
            }

            if (imagesCar.ContainsKey(name))
            {
                name = NameCreation(name);
            }
            return name;
        }

        void Ui_Load(object sender, EventArgs e)
        {
            carsBox.Enabled = false;
            imagesBox.Enabled = false;
            save.Enabled = false;
            copy.Enabled = false;
            folder.Enabled = false;

            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Vocord\VOCORD Traffic CrossRoads Server"))
            {
                if (key != null)
                {
                    if (key.GetValue("InstallDir") != null)
                    {
                        installDir = key.GetValue("InstallDir").ToString();
                    }
                    if (key.GetValue("ScreenshotDir") != null)
                    {
                        screenshotDir = key.GetValue("ScreenshotDir").ToString();
                    }
                }
            }
            if (File.Exists(installDir + @"Database\vtsettingsdb.sqlite"))
            {
                string sqlChannel = "SELECT CHANNEL_ID, NAME FROM CHANNELS";
                using (var connection = new SQLiteConnection($@"URI=file:{installDir}Database\vtsettingsdb.sqlite"))
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand(sqlChannel, connection);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                LoadZone(reader.GetString(0), reader.GetString(1));
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show($"There is no database file \n{installDir}Database\\vtsettingsdb.sqlite \nor it is in a different folder.", "No database file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            imageNames.Add("VideoDetection/ObjectBeginImage", "Video Begin"); // первая фиксация
            imageNames.Add("VideoDetection/ObjectImage", "Video Best"); // первая фиксация
            imageNames.Add("VideoDetection/ObjectEndImage", "Video End");     // последняя фиксация

            imageNames.Add("WrongWay/Episodes/Episode/DetectionBeginning", "Wrong Way Begin");
            imageNames.Add("WrongWay/Episodes/Episode/DetectionEnd", "Wrong Way End");

            imageNames.Add("VideoDetection/ObjectStopLineImage", "Stop Line");
            imageNames.Add("RedLightBeforeLine/Episodes/Episode/DetectionBeginning", "Red Light Before Line");
            imageNames.Add("RedLightAfterLine/Episodes/Episode/DetectionBeginning", "Red Light After Line Begin");
            imageNames.Add("RedLightAfterLine/Episodes/Episode/DetectionEnd", "Red Light After Line End");

            imageNames.Add("RedLightBeforeLineLeft/Episodes/Episode/DetectionBeginning", "Red Light Before Line Left Begin");
            imageNames.Add("RedLightBeforeLineRight/Episodes/Episode/DetectionBeginning", "Red Light Before Line Right Begin");

            imageNames.Add("RedLightCross/Episodes/Episode/DetectionBeginning", "Red Light Cross Begin");
            imageNames.Add("RedLightCross/Episodes/Episode/DetectionEnd", "Red Light Cross End");

            imageNames.Add("WrongCross/Episodes/Episode/DetectionBeginning", "Wrong Cross Begin");
            imageNames.Add("WrongCross/Episodes/Episode/DetectionEnd", "Wrong Cross End");

            imageNames.Add("BeforeZebraWithPedestrian/Episodes/Episode/RedLightState", "Zebra Begin");
            imageNames.Add("AfterZebraWithPedestrian/Episodes/Episode/DetectionEnd", "Zebra End");
        }

        void LoadZone(string id, string name)
        {
            if (File.Exists(installDir + @"Database\bpm.db"))
            {
                string sqlzones = $"SELECT Type, Name, X1, Y1, X2, Y2, X3, Y3, X4, Y4 FROM Zone WHERE ChannelId = \"{id}\""; // AND Type < 11";
                using (var connection = new SQLiteConnection($@"URI=file:{installDir}Database\bpm.db"))
                {
                    connection.Open();
                    SQLiteCommand command = new SQLiteCommand(sqlzones, connection);
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            int indexZona = 0;
                            ChannelNameZone nameZone = new ChannelNameZone();
                            while (reader.Read())
                            {
                                ChannelZone zone = new ChannelZone();
                                zone.name = reader.GetString(1);
                                zone.type = reader.GetInt16(0);
                                zone.x1 = Convert.ToInt16(reader.GetFloat(2));
                                zone.y1 = Convert.ToInt16(reader.GetFloat(3));
                                zone.x2 = Convert.ToInt16(reader.GetFloat(4));
                                zone.y2 = Convert.ToInt16(reader.GetFloat(5));
                                zone.x3 = Convert.ToInt16(reader.GetFloat(6));
                                zone.y3 = Convert.ToInt16(reader.GetFloat(7));
                                zone.x4 = Convert.ToInt16(reader.GetFloat(8));
                                zone.y4 = Convert.ToInt16(reader.GetFloat(9));

                                nameZone.zones[indexZona++] = zone;
                            }
                            nameZone.channelName = name;
                            nameZone.count = Convert.ToInt16(indexZona);
                            channel.Add(id, nameZone);
                        }
                    }
                }
            }
        }

        void drawingPolygons()
        {
            if (imagesBox.Items.Count > 0)
            {
                Carfile carfile = (Carfile)cars[carsBox.SelectedItem.ToString()];
                CarFilePoint imgCar = (CarFilePoint)imagesCar[imagesBox.SelectedItem.ToString()];

                if (imgCar.file != "")
                {
                    if (File.Exists(screenshotDir + "\\" + carfile.patchfile + imgCar.file))
                    {
                        imageBox.Image = Image.FromFile(screenshotDir + "\\" + carfile.patchfile + imgCar.file);

                        Graphics imageBoximg = Graphics.FromImage(imageBox.Image);
                        Pen pen = new Pen(Color.Red, 5);

                        ChannelNameZone channelZones = (ChannelNameZone)channel[carfile.channelId];

                        for (Int16 indexZone = 0; indexZone < channelZones.count; indexZone++)
                        {
                            var color = Color.LightGray;
                            bool paint = true;
                            switch (channelZones.zones[indexZone].type)
                            {
                                case 0:
                                    color = Color.Green;  // Зона поиска встречного движения
                                    break;
                                case 2:
                                    color = Color.SkyBlue;  // Зона до стоп-линии
                                    break;
                                case 3:
                                    color = Color.Blue;  // Зона после стоп-линии
                                    break;
                                case 4:
                                    color = Color.Red;  // Зона проезда перекрестка на красный свет
                                    break;
                                case 6:
                                    color = Color.Yellow;  // Зона начала маневра
                                    break;
                                case 10:
                                    color = Color.Orange;  // Зона распознавания номеров
                                    break;
                                case 11:
                                    color = Color.LightGreen;  // Полосы
                                    break;
                                default:
                                    color = Color.LightGray;  // Полосы
                                    break;
                            }
                            if (channelZones.zones[indexZone].type == 0 & !checkManeuvers.Checked) { paint = false; } // Зона поиска встречного движения
                            if (channelZones.zones[indexZone].type == 2 & !checkLights.Checked) { paint = false; } // Зона до стоп-линии
                            if (channelZones.zones[indexZone].type == 3 & !checkLights.Checked) { paint = false; } // Зона после стоп-линии
                            if (channelZones.zones[indexZone].type == 4 & !checkLights.Checked) { paint = false; } // Зона проезда перекрестка на красный свет
                            if (channelZones.zones[indexZone].type == 6 & !checkManeuvers.Checked) { paint = false; } // Зона начала маневра 
                            if (channelZones.zones[indexZone].type == 11 & !checkLanes.Checked) { paint = false; } // Полосы

                            if (paint) {
                                Point point1 = new Point(channelZones.zones[indexZone].x1, channelZones.zones[indexZone].y1);
                                Point point2 = new Point(channelZones.zones[indexZone].x2, channelZones.zones[indexZone].y2);
                                Point point3 = new Point(channelZones.zones[indexZone].x3, channelZones.zones[indexZone].y3);
                                Point point4 = new Point(channelZones.zones[indexZone].x4, channelZones.zones[indexZone].y4);
                                Point[] curvePoints = { point1, point2, point3, point4 };
                                pen = new Pen(color, 5);
                                SolidBrush brush = new SolidBrush(Color.FromArgb(trackBar.Value * 10, color));
                                imageBoximg.FillPolygon(brush, curvePoints);
                                imageBoximg.DrawPolygon(pen, curvePoints);
                            }
                        }

                        pen = new Pen(Color.Red, 5);
                        imageBoximg.DrawRectangle(pen, (imgCar.x - imgCar.width / 2), (imgCar.y - imgCar.height / 2), imgCar.width, imgCar.height);
                        imageBoximg.DrawRectangle(pen, imgCar.x, imgCar.y, 5, 5);

                        for (Int16 indexPoints = 0; indexPoints < carfile.point.Length; indexPoints++)
                        {
                            if (carfile.point[indexPoints] != null)
                            {
                                if (carfile.point[indexPoints].x != 0 & carfile.point[indexPoints].y != 0)
                                {
                                    imageBoximg.DrawEllipse(pen, carfile.point[indexPoints].x - 10, carfile.point[indexPoints].y - 10, 20, 20);
                                    if (indexPoints > 0)
                                    {
                                        if (carfile.point[indexPoints - 1].x != 0 & carfile.point[indexPoints - 1].y != 0)
                                        {
                                            imageBoximg.DrawLine(pen, carfile.point[indexPoints - 1].x, carfile.point[indexPoints - 1].y, carfile.point[indexPoints].x, carfile.point[indexPoints].y);
                                        }
                                    }
                                }
                            }
                        }

                        imageBoximg.Dispose();
                        imageBox.Refresh();
                    }
                    else
                    {
                        MessageBox.Show($"In Data.xml \n {screenshotDir + "\\" + carfile.patchfile + imgCar.file} \n\nthere is a link to the file, but there is no file itself.", "No file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        void readXmlfile()
        {
            XmlDocument dataXmlFile = new XmlDocument();
            if (carsBox.Items.Count > 0)
            {
                imagesBox.Items.Clear();
                imagesCar.Clear();

                Carfile carfile = (Carfile)cars[carsBox.SelectedItem.ToString()];

                if (File.Exists(screenshotDir + "\\" + carfile.patchfile + "Data.xml"))
                {
                    dataXmlFile.Load(screenshotDir + "\\" + carfile.patchfile + "Data.xml");

                    ICollection keys = imageNames.Keys;
                    foreach (String nameImages in keys)
                    {
                        XmlNodeList nodeList = dataXmlFile.SelectNodes($"//{nameImages}");
                        if (nodeList != null)
                        {
                            CarFilePoint imagesXml = new CarFilePoint();
                            foreach (XmlNode xnode in nodeList)
                            {
                                foreach (XmlNode vavueNode in xnode.ChildNodes)
                                {
                                    if (vavueNode.Name == "Image") { imagesXml.file = vavueNode.InnerText; }
                                    if (vavueNode.Name == "X") { imagesXml.x = Int16.Parse(vavueNode.InnerText); }
                                    if (vavueNode.Name == "Y") { imagesXml.y = Int16.Parse(vavueNode.InnerText); }
                                    if (vavueNode.Name == "Width") { imagesXml.width = Int16.Parse(vavueNode.InnerText); }
                                    if (vavueNode.Name == "Height") { imagesXml.height = Int16.Parse(vavueNode.InnerText); }
                                }

                                if (imagesXml.file != "" || imagesXml.file == null)
                                {
                                    string names = (string)imageNames[nameImages];
                                    if (imagesCar.ContainsKey(names))
                                    {
                                        names = NameCreation(names);
                                    }
                                    imagesBox.Items.Add(names);
                                    imagesCar.Add(names, imagesXml);
                                }
                            }
                        }
                    }

                    XmlNodeList statusPoints = dataXmlFile.GetElementsByTagName($"Interpolated");
                    if (statusPoints.Count > 0)
                    {
                        XmlNodeList nodePoints = dataXmlFile.SelectNodes("//TrackPoints/TrackPoint");
                        if (nodePoints != null)
                        {
                            int indexPoint = 0;
                            foreach (XmlNode points in nodePoints)
                            {
                                CarTrackPoint trackPoint = new CarTrackPoint();
                                foreach (XmlNode pointsName in points.ChildNodes)
                                {
                                    if (pointsName.Name == "RecognitionNumber")
                                    {
                                        foreach (XmlNode point in pointsName.ChildNodes)
                                        {
                                            if (point.Name == "X") { trackPoint.x = Int16.Parse(point.InnerText); }
                                            if (point.Name == "Y") { trackPoint.y = Int16.Parse(point.InnerText); }
                                        }
                                    }
                                    if (pointsName.Name == "ImageNumber")
                                    {
                                        foreach (XmlNode point in pointsName.ChildNodes)
                                        {
                                            if (point.Name == "X") { trackPoint.x = Int16.Parse(point.InnerText); }
                                            if (point.Name == "Y") { trackPoint.y = Int16.Parse(point.InnerText); }
                                        }
                                    }
                                    if (pointsName.Name == "NumberArea")
                                    {
                                        foreach (XmlNode point in pointsName.ChildNodes)
                                        {
                                            if (point.Name == "X") { trackPoint.x = Int16.Parse(point.InnerText); }
                                            if (point.Name == "Y") { trackPoint.y = Int16.Parse(point.InnerText); }
                                        }
                                    }
                                }
                                carfile.point[indexPoint++] = trackPoint;
                            }
                        }
                    }
                    if (imagesBox.Items.Count > 0)
                    {
                        imagesBox.SelectedIndex = 0;
                        save.Enabled = true;
                        drawingPolygons();
                    }
                }
                else
                {
                    MessageBox.Show($"There is a record with number {numberBox.Text} in the database, but there is no Data.xml file on the path \n{screenshotDir + "\\" + carfile.patchfile} Data.xml", "No file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        void dateAndTimeBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            readXmlfile();
        }

        void imagesBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawingPolygons();
        }

        private void copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetImage(imageBox.Image);
        }

        void folder_Click(object sender, EventArgs e)
        {
            if (carsBox.Items.Count > 0)
            {
                Carfile carfile = (Carfile)cars[carsBox.SelectedItem.ToString()];
                Process.Start("explorer.exe", screenshotDir + "\\" + carfile.patchfile);
            }
        }

        void save_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.Filter = "Jpeg|*.jpg";
            saveFileDialog.FileName = $"{carsBox.SelectedItem.ToString().Replace(':', '.')} - {numberBox.Text.Replace('*', '.')} - {imagesBox.SelectedItem.ToString()}.jpg";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                imageBox.Image.Save(saveFileDialog.FileName, ImageFormat.Jpeg);
            }
        }

        void numberBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (char)Keys.Enter)
            {
                search.PerformClick();
            }
        }

        async void search_Click(object sender, EventArgs e)
        {
            carsBox.Items.Clear();
            imagesBox.Items.Clear();
            cars.Clear();
            carsBox.Enabled = false;
            imagesBox.Enabled = false;
            carsBox.Text = string.Empty;
            imagesBox.Text = string.Empty;
            numberBox.Enabled = false;
            search.Enabled = false;
            save.Enabled = false;
            copy.Enabled = false;
            folder.Enabled = false;

            if (File.Exists(installDir + @"Database\vtvehicledb.sqlite"))
            {
                imageBox.Image = Properties.Resources.Searchimg;
                string sqlcar = $"SELECT CHECKTIME, CHANNEL_ID, SCREENSHOT, CARS_ID FROM CARS WHERE FULLGRNNUMBER LIKE \"{numberBox.Text.Replace('*', '_').ToUpper()}\" ORDER BY CARS_ID DESC";
                await Task.Run(() =>
                {
                    using (var connection = new SQLiteConnection($@"URI=file:{installDir}Database\vtvehicledb.sqlite"))
                    {
                        connection.Open();

                        SQLiteCommand command = new SQLiteCommand(sqlcar, connection);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                string datetime;
                                while (reader.Read())
                                {
                                    Carfile carfile = new Carfile();
                                    ChannelNameZone channelName = (ChannelNameZone)channel[reader.GetString(1)];
                                    datetime = reader.GetInt64(3).ToString() + " - " + DateTime.FromFileTime(reader.GetInt64(0)).ToString() + " - " + channelName.channelName;
                                    carfile.channelId = reader.GetString(1);
                                    carfile.patchfile = reader.GetString(2).Remove(reader.GetString(2).LastIndexOf("\\") + 1);
                                    carsBox.Items.Add(datetime);
                                    cars.Add(datetime, carfile);
                                }
                            }
                            else
                            {
                                MessageBox.Show($"There are no driveways with number {numberBox.Text} in the database.", "Number not found", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                                imageBox.Image = Properties.Resources.CarNotSelected;
                            }
                        }
                    }
                });
            }
            else
            {
                MessageBox.Show($"There is no database file \n{installDir}Database\\vtvehicledb.sqlite \nor it is in a different folder.", "No database file", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            if (carsBox.Items.Count > 0)
            {
                carsBox.SelectedIndex = 0;
                numberBox.Enabled = true;
                search.Enabled = true;
                carsBox.Enabled = true;
                imagesBox.Enabled = true;
                copy.Enabled = true;
                save.Enabled = true;
                folder.Enabled = true;
                readXmlfile();
            }
            else
            {
                numberBox.Enabled = true;
                search.Enabled = true;
            }
        }

        private void numberBox_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(numberBox, "Number to search.");
        }

        private void search_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(search, "Search for a number in the database.");
        }

        private void carsBox_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(carsBox, "Found cars in the database.");
        }

        private void imagesBox_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(imagesBox, "Photos of a passing car.");
        }

        private void save_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(save, "Save selected photo.");
        }

        private void copy_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(copy, "Copy the selected photo to the clipboard.");
        }

        private void folder_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(folder, "Open the folder with the selected car.");
        }

        private void trackBar_Scroll(object sender, EventArgs e)
        {
            drawingPolygons();
        }

        private void checkLanes_CheckedChanged(object sender, EventArgs e)
        {
            drawingPolygons();
        }

        private void checkLights_CheckedChanged(object sender, EventArgs e)
        {
            drawingPolygons();
        }

        private void checkManeuvers_CheckedChanged(object sender, EventArgs e)
        {
            drawingPolygons();
        }

        private void ZoneBox_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(ZoneBox, "Setting up display zones.");
        }

        private void checkLanes_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(checkLanes, "Strip zones.");
        }

        private void checkLights_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(checkLights, "Traffic light signal zones.");
        }

        private void checkManeuvers_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(checkManeuvers, "Maneuvering zones.");
        }

        private void trackBar_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(trackBar, "Zone brightness.");
        }
    }
}
