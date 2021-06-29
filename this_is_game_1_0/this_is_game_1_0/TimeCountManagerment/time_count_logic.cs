using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace this_is_game_1_0.TimeCountManagerment
{
    public class time_count_logic
    {
        int small_pomodoro = 0;
        int long_pomodoro = 0;
        int small_break = 0;
        int long_break = 0;
        int pomodoro_interval = 0;

        public int max_ticks = 0;
        public int current_cycle = 0;
        public int current_secs = 0;
        public bool is_break = false;

        public bool is_long_pomodoro = false;
        public int total_work_time = 0;

        SoundPlayer sp_end_pomodoro = new SoundPlayer(Properties.Resources.rikka_hair_voice_2);

        public time_count_logic(
            int small_pomodoro_time, 
            int long_pomodoro_time, 
            int small_break_time, 
            int long_break_time, 
            int pomodoro_intervals, 
            bool is_long_pomodoro)
        {
            reset_all(small_pomodoro_time, long_pomodoro_time, small_break_time, long_break_time, pomodoro_intervals, is_long_pomodoro);
        }

        public void is_long_pomodoro_status(bool is_long)
        {
            is_long_pomodoro = is_long;
        }

        int conver_time_to_ticks(int time)
        {
            return time * 60;
        }

        public void reset_all(int small_pomodoro_time, int long_pomodoro_time, int small_break_time, int long_break_time, int pomodoro_intervals, bool is_long_pomodoro)
        {
            small_pomodoro = small_pomodoro_time;
            long_pomodoro = long_pomodoro_time;
            small_break = small_break_time;
            long_break = long_break_time;
            pomodoro_interval = pomodoro_intervals;

            current_cycle = 0;
            current_secs = 0;
            is_break = false;
            this.is_long_pomodoro = is_long_pomodoro;
            total_work_time = 0;
        }

        public void set_tick_profile()
        {
            if (is_break)
            {
                if(current_cycle == pomodoro_interval)
                {
                    current_secs = long_break * 60;
                    max_ticks = current_secs;
                }
                else
                {
                    current_secs = small_break * 60;
                    max_ticks = current_secs;
                }
            }
            else
            {
                if (is_long_pomodoro)
                {
                    current_secs = long_pomodoro * 60;
                    max_ticks = current_secs;
                }
                else
                {
                    current_secs = small_pomodoro * 60;
                    max_ticks = current_secs;
                }
            }
        }

        public bool count_down_1_tick()
        {
            current_secs--;
            if (current_secs == 0)
            {
                if (!is_break) current_cycle++;
                if (current_cycle == pomodoro_interval && is_break)
                    current_cycle = 0;
                is_break = !is_break;
                set_tick_profile();
                end_timer();
                
                return true;
            }
            else
            {
                return false;
            }

        }

        void end_timer()
        {
            sp_end_pomodoro.Play();
            if (is_break)
                MessageBox.Show("Time for break!", "Break go brrrr", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show("Time to work!", "Work time", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
