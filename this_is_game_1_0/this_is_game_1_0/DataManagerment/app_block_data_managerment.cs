using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Win32;
using this_is_game_1_0.Properties;

namespace this_is_game_1_0.DataManagerment
{
    public class app_block_data_managerment
    {
        // get lines from files here
        private List<string> lines;

        string file_path = "";
        public string get_path() { return file_path; }
        // data as str for view in main
        public string data_from_file = "";


        public app_block_data_managerment()
        {
            file_path = Settings.Default.FilePathApp;
            get_data_from_file();
        }

        public void get_data_from_file()
        {
            try
            {
                lines = File.ReadAllLines(file_path).ToList();
                foreach (var line in lines)
                {
                    data_from_file += line + "\n";
                }
            }
            catch
            {
                find_data();
                get_data_from_file();
            }
        }

        public void edit_data(string data_input)
        {
            List<string> output = new List<string>();
            string[] lines = data_input.Split('\n');
            foreach (var line in lines)
            {
                if (line.Length == 0) continue;
                if (line[line.Length - 1] == '\r')
                {
                    string tmp = line.Substring(0, line.Length - 1);
                    if (tmp.Length == 0) continue;
                    output.Add(tmp);
                }
                else
                {
                    output.Add(line);
                }
            }
            File.WriteAllLines(file_path, output);
        }

        public void find_data()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select app data file";
            op.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (op.ShowDialog() == true)
            {
                file_path = op.FileName.ToString();
            }
            save_path();
        }

        void save_path()
        {
            Settings.Default.FilePathApp = file_path;
            Settings.Default.Save();
        }
    }
}
