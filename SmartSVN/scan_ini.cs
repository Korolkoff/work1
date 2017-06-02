using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace SmartSVN_4
{
    public partial class scaninif : Form
    {
        List<string> subsc_files;

        public scaninif()
        {
            InitializeComponent();
            scanfolder_tb.Text = Path.GetFullPath(".\\");
            ScanFolder(scanfolder_tb.Text);
            subsc_files = new List<string>();
        }

        private bool SubscExists(string file)
        {
            if ((subsc_files == null) || (subsc_files.Count == 0))
            {
                StreamReader sr = null;
                try {
                    sr = new StreamReader(Application.StartupPath + "\\SmartSVN.ini", false);
                    while (!sr.EndOfStream)
                    {
                        string str = sr.ReadLine().Trim();
                        string[] key_value = str.Split('=');
                        string key = key_value[0].Trim().ToUpper(), value = (key_value.Length > 1) ? key_value[1].Trim() : "";
                        if ((key == "PATH") && (!String.IsNullOrEmpty(value)))
                        { 
                            value = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)[0];
                            subsc_files.Add(Path.GetFullPath(value).ToUpper());
                        }
                    }
                }
                catch (Exception e)
                {
                    sr.Close();
                    return false;
                }
                sr.Close();
            }
            string f = Path.GetFullPath(file).ToUpper();
            for (int i = 0; i < subsc_files.Count; i++)
            {
                if (f == subsc_files[i]) return true;
            }
            return false;
        }


        private void accept_b_Click(object sender, EventArgs e)
        {
            List<string> lines = new List<string>();
            string ssinif = Application.StartupPath + "\\SmartSVN.ini";
            lines = File.ReadAllLines(ssinif).ToList();
            for (int file = 0; file < subsc_dgv.Rows.Count; file++)
            {
                if (Convert.ToBoolean(subsc_dgv.Rows[file].Cells[0].Value))
                {
                    string f = Path.GetFullPath(subsc_dgv.Rows[file].Cells[2].Value.ToString());
                    if (!SubscExists(f))
                    {
                        lines.Add("");
                        lines.Add("[Subscribe]");
                        lines.Add("name = " + Path.GetFileNameWithoutExtension(f));
                        lines.Add("check_period = " + period_tb.Text);
                        lines.Add("path = " + cc.GetRelativePath(f, Application.StartupPath));
                    }
                }
            }
            lines.Add("");
            File.WriteAllLines(ssinif, lines);
            this.Close();
        }

        private void ScanFolder(string folder)
        {
            subsc_dgv.Rows.Clear();
            string[] files = Directory.GetFiles(folder, "*.ini", SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                string str = File.ReadAllText(files[i]);
                if (str.ToUpper().IndexOf("[SVN]") >= 0)
                {
                    subsc_dgv.Rows.Add(true, Path.GetFileNameWithoutExtension(files[i]), files[i]);
                    subsc_dgv.Rows[subsc_dgv.Rows.Count - 1].Cells[1].ToolTipText = files[i];
                }
            }
        }

        private void scanfolder_tb_Click(object sender, EventArgs e)
        {
            scanfolder_fbd.SelectedPath = Application.StartupPath;
            if (scanfolder_fbd.ShowDialog() == DialogResult.OK)
            {
                scanfolder_tb.Text = scanfolder_fbd.SelectedPath;
                ScanFolder(scanfolder_tb.Text);
            }
        }

        private void scanfolder_tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) ScanFolder(scanfolder_tb.Text);
        }

    }
}
