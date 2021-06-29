using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace this_is_game_1_0.BlockManagerment
{
    public class web_block_managerment
    {
        private static string old_hosts_content = "";
        private static string new_hosts_content = "";

        public web_block_managerment(string web_sites)
        {
            load_old_host_content();
            load_new_host_content(web_sites);
        }

        public void load_new_host_content(string websites)
        {
            string[] web_sites_split = websites.Split('\n');
            new_hosts_content += old_hosts_content;
            new_hosts_content += "\n\n";
            foreach (string line in web_sites_split)
            {
                if(line != null && line != "")
                {
                    new_hosts_content += "0.0.0.0 " + line + " www." + line + "\n";
                }
            }
            //Console.WriteLine(new_hosts_content);
        }

        public void load_old_host_content()
        {
            string hostPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts");
            string[] lines = File.ReadAllLines(hostPath, Encoding.UTF8);
            foreach (string line in lines)
                old_hosts_content += line + "\n";
            //Console.WriteLine(old_hosts_content);
        }

        public static bool modify_host_file(string entry)
        {
            try
            {
                String hostPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts");
                System.IO.File.WriteAllText(hostPath, string.Empty);

                using (System.IO.StreamWriter w = System.IO.File.AppendText(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts")))
                {
                    // clear hosts content
                    w.WriteLine(entry);
                    return true;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public static bool block_host_file()
        {
            if (modify_host_file(new_hosts_content))
            {
                return true;
            }
            return false;
        }

        public static bool unblock_host_file()
        {
            try
            {
                String hostPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), @"drivers\etc\hosts");
                System.IO.File.WriteAllText(hostPath, string.Empty);
                System.IO.File.WriteAllText(hostPath, old_hosts_content);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
