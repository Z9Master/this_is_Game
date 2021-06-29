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
    public class user_data_managerment
    {
        public enum user_data_type
        {
            pomodoro_interval,
            pomodoro_time,
            pomodoro_large_time,
            short_break_time,
            long_break_time,
            diamonds,
            cb_long_pomodoro,
            cb_finished_plan,
            cb_block_during_pomodoro,
            cb_kill_app_30_after,
            cb_turn_off_block_after_exit
        }

        public string error_codes = "0";

        // tmp data from path
        public int pomodoro_interval, pomodoro_time, pomodoro_large_time = 0;
        public int short_break_time, long_break_time = 0;
        public int diamonds = 0;
        public bool cb_long_pomodoro, cb_finished_plan, cb_block_during_pomodoro, cb_kill_app_30_after, cb_turn_off_block_after_exit = false;

        // tmp list str for working with raw data lines
        public List<string> lines;

        string file_path = "";
        public string get_path() { return file_path; }

        //public string debug_str = "no input";

        public user_data_managerment()
        {
            file_path = Settings.Default.FilePath;
            load_data_to_tmp();
        }

        // loads data to tmp variables, which can be use/get in main
        public void load_data_to_tmp()
        {
            try
            {
                lines = File.ReadAllLines(file_path).ToList();
                parse_and_load_to_tmp(ref pomodoro_interval, lines[0]);
                parse_and_load_to_tmp(ref pomodoro_time, lines[1]);
                parse_and_load_to_tmp(ref pomodoro_large_time, lines[2]);
                parse_and_load_to_tmp(ref short_break_time, lines[3]);
                parse_and_load_to_tmp(ref long_break_time, lines[4]);
                parse_and_load_to_tmp(ref diamonds, lines[5]);
                bool_parse_and_load_to_tmp(ref cb_long_pomodoro, lines[6]);
                bool_parse_and_load_to_tmp(ref cb_finished_plan, lines[7]);
                bool_parse_and_load_to_tmp(ref cb_block_during_pomodoro, lines[8]);
                bool_parse_and_load_to_tmp(ref cb_kill_app_30_after, lines[9]);
                bool_parse_and_load_to_tmp(ref cb_turn_off_block_after_exit, lines[10]);
            }
            catch
            {
                error_codes = "Can't load data from selected file, make sure u choosed right one.";
                find_data();
                load_data_to_tmp();
            }
        }

        // get raw data and parse from str -> int and save it to ref
        void parse_and_load_to_tmp(ref int tmp_souce, string input_data)
        {
            string tmp_number = "";
            for(int i = 0; i < input_data.Length; i++)
            {
                if (Char.IsDigit(input_data[i]))
                    tmp_number += input_data[i];
            }
            if (tmp_number.Length > 0)
            {
                tmp_souce = int.Parse(tmp_number);
                return;
            }
            tmp_souce = -1;
        }

        // get raw data and parse from str -> bool and save it to ref
        void bool_parse_and_load_to_tmp(ref bool tmp_souce, string input_data)
        {
            string tmp_bool = "";
            for (int i = 0; i < input_data.Length; i++)
            {
                if (Char.IsDigit(input_data[i]))
                    tmp_bool += input_data[i];
            }
            if (tmp_bool.Length > 0)
            {
                if(tmp_bool.Equals("0"))
                {
                    tmp_souce = false;
                }
                else
                {
                    tmp_souce = true;
                }
            }
        }

        public bool edit_data(int data_input, user_data_type data_type)
        {
            switch (data_type)
            {
                case user_data_type.pomodoro_interval:
                    lines[0] = $"pomodoro_interval:{data_input}";
                    break;
                case user_data_type.pomodoro_time:
                    lines[1] = $"pomodoro_time:{data_input}";
                    break;
                case user_data_type.pomodoro_large_time:
                    lines[2] = $"pomodoro_large_time:{data_input}";
                    break;
                case user_data_type.short_break_time:
                    lines[3] = $"short_break_time:{data_input}";
                    break;
                case user_data_type.long_break_time:
                    lines[4] = $"long_break_time:{data_input}";
                    break;
                case user_data_type.diamonds:
                    lines[5] = $"diamonds:{data_input}";
                    break;
                case user_data_type.cb_long_pomodoro:
                    lines[6] = $"cb_long_pomodoro:{data_input}";
                    break;
                case user_data_type.cb_finished_plan:
                    lines[7] = $"cb_finished_plan:{data_input}";
                    break;
                case user_data_type.cb_block_during_pomodoro:
                    lines[8] = $"cb_block_during_pomodoro:{data_input}";
                    break;
                case user_data_type.cb_kill_app_30_after:
                    lines[9] = $"cb_kill_app_after:{data_input}";
                    break;
                case user_data_type.cb_turn_off_block_after_exit:
                    lines[10] = $"cb_turn_off_block_after_exit:{data_input}";
                    break;
                default: error_codes = "Can't write data, check enum";
                    return false;
            }
            File.WriteAllLines(file_path, lines);
            return true;
        }

        // find data
        public void find_data()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select user data file";
            op.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            if (op.ShowDialog() == true)
            {
                file_path = op.FileName.ToString();
            }
            save_path();
        }

        void save_path()
        {
            Settings.Default.FilePath = file_path;
            Settings.Default.Save();
        }
    }
}
