using System;
using System.Windows;
using Newtonsoft.Json;
using System.Xml;
using System.IO;
using Formatting = Newtonsoft.Json.Formatting;
using Path = System.IO.Path;

namespace xml_json
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //создаем объект - диалоговое окно выбора файлов
        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

        XmlDocument doc = new XmlDocument();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML Files (*.xml)|*.xml";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    string filename = dlg.FileName;
                    string onlyname = Path.GetFileNameWithoutExtension(filename) + ".json";
                    text1.Text = filename;

                    //Загружаем в документ файл XML
                    doc.Load(filename);

                    //Преобразуем в JSON c читабельным видом
                    string xmlcontents = JsonConvert.SerializeXmlNode(doc, Formatting.Indented);

                    var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), onlyname);

                    //Создаем файл json
                    File.WriteAllText(filePath, xmlcontents);

                    if (!File.Exists(filePath))
                        return;

                    // открываем папку с конечным файлом
                    string argument = "/select, \"" + filePath + "\"";
                    System.Diagnostics.Process.Start("explorer.exe", argument);
                }
                catch (XmlException exception)
                {
                    MessageBox.Show("Ну врятли это XML");
                }

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            dlg.DefaultExt = ".json";
            dlg.Filter = "JSON Files (*.json)|*.json";
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                try
                {
                    string filename = dlg.FileName;
                    string onlyname = Path.GetFileNameWithoutExtension(filename) + ".xml";
                    text2.Text = filename;

                    //Загружаем в переменную строку из файла json
                    string json = File.ReadAllText(filename);

                    //конвертация в json
                    var doc = JsonConvert.DeserializeXmlNode(json, "root");

                    string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), onlyname);

                    //Создаем файл json
                    doc.Save(filePath);


                    if (!File.Exists(filePath))
                        return;

                    string argument = "/select, \"" + filePath + "\"";
                    System.Diagnostics.Process.Start("explorer.exe", argument);
                }
                catch (XmlException exception)
                {
                    MessageBox.Show("Ну врятли это XML");
                }
            }
        }
    }
}