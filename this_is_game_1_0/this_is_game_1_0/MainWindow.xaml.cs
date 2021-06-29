using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using this_is_game_1_0.DataManagerment;
using this_is_game_1_0.TimeCountManagerment;
using System.Security.Principal;
using System.Diagnostics;
using this_is_game_1_0.BlockManagerment;

namespace this_is_game_1_0
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        user_data_managerment user_data;
        web_block_data_managerment web_data;
        app_block_data_managerment app_data;
        time_count_logic pomodoro_count;
        bool is_counting = false;

        web_block_managerment web_block;

        public MainWindow()
        {
            
            InitializeComponent();
            user_data = new user_data_managerment();
            web_data = new web_block_data_managerment();
            app_data = new app_block_data_managerment();
            
            pomodoro_count = new time_count_logic(  user_data.pomodoro_time,
                                                    user_data.pomodoro_large_time,
                                                    user_data.short_break_time,
                                                    user_data.long_break_time,
                                                    user_data.pomodoro_interval,
                                                    user_data.cb_long_pomodoro
                                                    );
            pomodoro_count.set_tick_profile();
            load_setting_from_user_data();
            timer_setup();
            //get_admin_state();
            web_block = new web_block_managerment(web_data.data_from_file);
        }

        private void get_admin_state()
        {
            WindowsPrincipal principal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            bool administrativeMode = principal.IsInRole(WindowsBuiltInRole.Administrator);

            if (!administrativeMode)
            {
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.Verb = "runas";
                startInfo.FileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
                try
                {
                    Process.Start(startInfo);
                }
                catch
                {
                    return;
                }
                return;
            }
        }


        void timer_setup()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            count_logic();
        }

        void convert_tict_to_mins_sec_display_time(int ticks)
        {
            int mins = 0;
            int secs = 0;
            mins = (ticks-(ticks % 60))/60;
            secs = ticks % 60;
            if(mins < 10)
            {
                if(secs < 10)
                    tb_timer_count.Text = "0" + mins + ":" + "0" + secs;
                else
                    tb_timer_count.Text = "0" + mins + ":"  + secs;
            }
            else
            {
                if (secs < 10)
                    tb_timer_count.Text = mins + ":" + "0" + secs;
                else
                    tb_timer_count.Text = mins + ":" + secs;
            }
            
        }

        void progress_bar_tick()
        {
            pb_left.Maximum = pomodoro_count.max_ticks;
            pb_left.Value = pomodoro_count.current_secs;
        }


        void load_setting_from_user_data()
        {
            cb_long_pomodoro.IsChecked = user_data.cb_long_pomodoro;
            cb_finished_plan.IsChecked = user_data.cb_finished_plan;
            cb_block_during_pomodoro.IsChecked = user_data.cb_block_during_pomodoro;
            lb_diamonds.Content = user_data.diamonds.ToString();

            cb_kill_application_after_30.IsChecked = user_data.cb_kill_app_30_after;
            cb_turn_off_weblock_after_exit.IsChecked = user_data.cb_turn_off_block_after_exit;

            tb_long_Break_interval.Text = user_data.pomodoro_interval.ToString();
            tb_pomodoro_time.Text = user_data.pomodoro_time.ToString();
            tb_pomodoro_large_time.Text = user_data.pomodoro_large_time.ToString();
            tb_short_break_time.Text = user_data.short_break_time.ToString();
            tb_large_break_time.Text = user_data.long_break_time.ToString();

            sl_long_Break_interval.Value = user_data.pomodoro_interval;
            sl_pomodoro_time.Value = user_data.pomodoro_time;
            sl_pomodoro_large_time.Value = user_data.pomodoro_large_time;
            sl_short_break_time.Value = user_data.short_break_time;
            sl_large_break_time.Value = user_data.long_break_time;

            tb_user_data_path.Text = user_data.get_path();
            tb_web_data_path.Text = web_data.get_path();
            tb_app_data_path.Text = web_data.get_path();

            tb_web_block.Text = web_data.data_from_file;
            tb_app_block.Text = app_data.data_from_file;
            convert_tict_to_mins_sec_display_time(pomodoro_count.max_ticks);
        }


        #region slider_setting_event
        private void ev_sl_break_interval(object sender, MouseButtonEventArgs e)
        {
            tb_long_Break_interval.Text = sl_long_Break_interval.Value.ToString();
            user_data.edit_data((int)sl_long_Break_interval.Value, user_data_managerment.user_data_type.pomodoro_interval);
            live_reload_setting();
        }

        private void ev_sl_pomodoro_time(object sender, MouseButtonEventArgs e)
        {
            tb_pomodoro_time.Text = sl_pomodoro_time.Value.ToString();
            user_data.edit_data((int)sl_pomodoro_time.Value, user_data_managerment.user_data_type.pomodoro_time);
            live_reload_setting();
        }

        private void ev_sl_pomodoro_large_time(object sender, MouseButtonEventArgs e)
        {
            tb_pomodoro_large_time.Text = sl_pomodoro_large_time.Value.ToString();
            user_data.edit_data((int)sl_pomodoro_large_time.Value, user_data_managerment.user_data_type.pomodoro_large_time);
            live_reload_setting();
        }

        private void ev_sl_short_break(object sender, MouseButtonEventArgs e)
        {
            tb_short_break_time.Text = sl_short_break_time.Value.ToString();
            user_data.edit_data((int)sl_short_break_time.Value, user_data_managerment.user_data_type.short_break_time);
            live_reload_setting();
        }

        private void ev_sl_long_break(object sender, MouseButtonEventArgs e)
        {
            tb_large_break_time.Text = sl_large_break_time.Value.ToString();
            user_data.edit_data((int)sl_large_break_time.Value, user_data_managerment.user_data_type.long_break_time);
            live_reload_setting();
        }


        #endregion


        #region cb_setting_event
        private void ev_c_cb_long_pomodoro(object sender, RoutedEventArgs e)
        {
            user_data.edit_data(1, user_data_managerment.user_data_type.cb_long_pomodoro);
            pomodoro_count.is_long_pomodoro_status(true);
            is_counting = false;
            btn_start.IsEnabled = true;
            user_data.load_data_to_tmp();
            pomodoro_count.set_tick_profile();
            convert_tict_to_mins_sec_display_time(pomodoro_count.max_ticks);
        }

        private void ev_u_cb_long_pomodoro(object sender, RoutedEventArgs e)
        {
            user_data.edit_data(0, user_data_managerment.user_data_type.cb_long_pomodoro);
            pomodoro_count.is_long_pomodoro_status(false);
            is_counting = false;
            btn_start.IsEnabled = true;
            user_data.load_data_to_tmp();
            pomodoro_count.set_tick_profile();
            convert_tict_to_mins_sec_display_time(pomodoro_count.max_ticks);
        }

        private void ev_c_cb_finished_plan(object sender, RoutedEventArgs e)
        {
            user_data.edit_data(1, user_data_managerment.user_data_type.cb_finished_plan);

        }

        private void ev_u_cb_finished_plan(object sender, RoutedEventArgs e)
        {
            user_data.edit_data(0, user_data_managerment.user_data_type.cb_finished_plan);

        }

        private void ev_c_cb_block_during_pomodoro(object sender, RoutedEventArgs e)
        {
            user_data.edit_data(1, user_data_managerment.user_data_type.cb_block_during_pomodoro);

        }

        private void ev_u_cb_block_during_pomodoro(object sender, RoutedEventArgs e)
        {
            user_data.edit_data(0, user_data_managerment.user_data_type.cb_block_during_pomodoro);

        }

        private void ev_c_cb_kill_app_after_30(object sender, RoutedEventArgs e)
        {
            user_data.edit_data(1, user_data_managerment.user_data_type.cb_kill_app_30_after);

        }

        private void ev_u_cb_kill_app_after_30(object sender, RoutedEventArgs e)
        {
            user_data.edit_data(0, user_data_managerment.user_data_type.cb_kill_app_30_after);

        }

        private void ev_c_cb_turn_off_after_exit(object sender, RoutedEventArgs e)
        {
            user_data.edit_data(1, user_data_managerment.user_data_type.cb_turn_off_block_after_exit);

        }

        private void ev_u_cb_turn_off_after_exit(object sender, RoutedEventArgs e)
        {
            user_data.edit_data(0, user_data_managerment.user_data_type.cb_turn_off_block_after_exit);

        }
        #endregion

        private void btn_browse_user_data(object sender, RoutedEventArgs e)
        {
            user_data.find_data();
            user_data.load_data_to_tmp();
            load_setting_from_user_data();
        }

        private void btn_browse_web_data(object sender, RoutedEventArgs e)
        {
            web_data.find_data();
            web_data.data_from_file = "";
            web_data.get_data_from_file();
            load_setting_from_user_data();
        }

        private void btn_browse_app_data(object sender, RoutedEventArgs e)
        {
            app_data.find_data();
            app_data.data_from_file = "";
            app_data.get_data_from_file();
            load_setting_from_user_data();
        }

        private void btn_save_web_app_data(object sender, RoutedEventArgs e)
        {
            web_data.edit_data(tb_web_block.Text);
            app_data.edit_data(tb_app_block.Text);
        }


        #region dock_panel_event
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void btn_minimize_click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void control_panel_mouse_down(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void control_panel_2_mouse_down(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }
        #endregion

        bool can_skip = false;
        private void btn_start_click(object sender, RoutedEventArgs e)
        {
            if (can_skip)
            {
                can_skip = false;
                is_counting = false;
                while (pomodoro_count.is_break) pomodoro_count.count_down_1_tick();
                convert_tict_to_mins_sec_display_time(pomodoro_count.max_ticks);
                btn_start.Content = "START";
                return;
            }
            is_counting = true;
            if (pomodoro_count.is_break)
            {
                btn_start.Content = "SKIP";
                can_skip = true;
            }
            else
            {
                btn_start.IsEnabled = false;
            }
        }

        private void btn_stop_click(object sender, RoutedEventArgs e)
        {
            is_counting = false;
            convert_tict_to_mins_sec_display_time(pomodoro_count.max_ticks);
            pomodoro_count.set_tick_profile();
            btn_start.IsEnabled = true;
            btn_start.Content = "START";
            can_skip = false;
        }

        void count_logic()
        {
            if (is_counting)
            {
                if (pomodoro_count.count_down_1_tick())
                {
                    is_counting = false;
                    btn_start.IsEnabled = true;
                    convert_tict_to_mins_sec_display_time(pomodoro_count.max_ticks);
                    btn_start.Content = "START";
                    can_skip = false;
                }
                else
                {
                    convert_tict_to_mins_sec_display_time(pomodoro_count.current_secs);
                    progress_bar_tick();
                }
            }
        }

        void live_reload_setting()
        {
            is_counting = false;
            btn_start.IsEnabled = true;
            user_data.load_data_to_tmp();
            pomodoro_count.reset_all(user_data.pomodoro_time,
                                                    user_data.pomodoro_large_time,
                                                    user_data.short_break_time,
                                                    user_data.long_break_time,
                                                    user_data.pomodoro_interval,
                                                    user_data.cb_long_pomodoro
                                                    );
            pomodoro_count.set_tick_profile();
            convert_tict_to_mins_sec_display_time(pomodoro_count.max_ticks);
        }

        
    }
}
