using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Diagnostics;
//using System.Windows.Documents;

namespace SmartSVN_4
{
    public partial class mainf : Form
    {
        int modeProgIni=1;
        int modeSubscIni=2;
        static IniFile[] subsc_file;
        List<Thread> threads;
        bool thread_fl;
        string tmp_SVNFolder;
        string[,] tmp_SVNFolders;
        bool frmClose = false;
        bool forceUpdate = false;
        bool hideComments = false;
        int cb_fl = 1;
        scaninif sif;

        public mainf()
        {
            InitializeComponent();
            Bitmap bmp = Properties.Resources.svn_folder;
            this.Icon = Icon.FromHandle(bmp.GetHicon());

            Bitmap bmp1 = Properties.Resources.svn_folder;
            tray_ni.Icon = Icon.FromHandle(bmp1.GetHicon());

            LoadINI(Path.GetFullPath(".\\")+"SmartSVN.ini", modeProgIni);
            LoadINI(Files_dgv.Rows[0].Cells[2].Value.ToString(), modeSubscIni);
            sif = new scaninif();
            sif.FormClosed += new FormClosedEventHandler(sif_Closed);

            this.Text += " (" + GetAppVersion() + ")";
        }

        private string GetAppVersion()
        {
            string[] clog = ExecCmd("svn info \"http://172.18.20.222:8888/svn/NLMK/C_PROJ/DatabaseFunc/SmartSVN\"");
            string str_ver = "Last Changed Rev:";
            for (int i = 0; i < clog.Length; i++)
            {
                int pos = clog[i].IndexOf(str_ver);
                if (pos >= 0)
                {
                    return clog[i].Substring(str_ver.Length).Trim();
                }
            }
            return null;
        }

        static int ArrayContainsSubstring(string src, string[] find)
        {
            for (int i = 0; i < find.Count(); i++)
            {
                if (src.ToUpper().IndexOf(find[i].ToUpper()) > 0) return i;
            }
            return -1;
        }

        string[][] GetProjectLinkFiles(int file, int block, int prj_id)
        {
            List<string[]> tmp_result = new List<string[]>();
            var item = subsc_file[file].ini_file[block];
            int fc = item.files.Count - 1;

            string scan_dir = Path.GetFullPath(item.files[fc].dir);
            string prj_path = Path.GetFullPath(item.vsprojects[prj_id].file_prj);
            string[] ext = item.vsprojects[prj_id].ext.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            //string[] files = Directory.EnumerateFiles(scan_dir, "*.*", SearchOption.AllDirectories).Where(s => s.EndsWith(".cs") ||s.EndsWith(".cs")).ToArray();
            //string[] files = Directory.GetFiles(scan_dir, "*.cs", SearchOption.AllDirectories).Where(file => Regex.IsMatch(file, @"^.+\.(wav|mp3|txt)$")).ToArray();
            string[] files = Directory.GetFiles(scan_dir, "*.*", SearchOption.AllDirectories);
            for (int k = 0; k < files.Length; k++)
            {
                if ((Array.IndexOf(ext, Path.GetExtension(files[k]).Trim('.')) >= 0) || (ext[0] == "*"))
                { 
                    string[] temp = new string[3];
                    temp[0] = cc.GetRelativePath(files[k], prj_path);
                    temp[1] = files[k].Substring(scan_dir.Length + 1);
                    temp[2] = files[k];
                    tmp_result.Add(temp);
                }
            }

            /*
            //            for (int i = 0; i < item.files.Count; i++)
            //            {
                            for (int j = 0; j < item.files[fc].file_name.Count; j++)
                            {
                                string scan_dir = Path.GetFullPath(item.files[fc].dir + "\\" + item.files[fc].file_name[j]);
                                string[] files;
                                if (File.Exists(scan_dir))
                                {
                                    if (cc.IsFolder(scan_dir)) files = Directory.GetFiles(scan_dir, "*.cs", SearchOption.AllDirectories);
                                    else files = new string[] { scan_dir };
                                }
                                else files = new string[0];
                                for (int k = 0; k < files.Length; k++)
                                {
                                    string[] temp = new string[2];
                                    temp[0] = files[k];
                                    temp[1] = files[k].Substring(files[k].IndexOf(item.files[fc].file_name[j]));
                                    tmp_result.Add(temp);
                                }
                            }
            //            }           
            */

            List<string[]> result = new List<string[]>();
            var item_prj = item.vsprojects[prj_id];
            if ((item_prj.file_name != null) && (item_prj.file_name.Count > 0) && (item_prj.file_name[0] != "*")) {
                for (int i = 0; i < item_prj.file_name.Count; i++)
                {
                    for (int j = 0; j < tmp_result.Count; j++)
                    {
                        if (item_prj.file_name[i] == tmp_result[j][1]) { result.Add(tmp_result[j]); break; }
                        else if (tmp_result[j][1].IndexOf(item_prj.file_name[i] + "\\") == 0) { result.Add(tmp_result[j]); }
                    }
                }
            } else {
                result = tmp_result;
            }

            return result.ToArray();
        }

        void CollectProjectFile(int file, int block, int prj_id, XmlDocument xml_doc, XmlNode childCompile)
        {
            try
            {
                var item = subsc_file[file].ini_file[block];
                string[][] link_files = GetProjectLinkFiles(file, block, prj_id);
                string prj_folder_link = item.vsprojects[prj_id].folder;
                for (int i = 0; i < link_files.Length; i++)
                {
                    XmlElement compile_el = xml_doc.CreateElement("Compile", "http://schemas.microsoft.com/developer/msbuild/2003");
                    compile_el.SetAttribute("Include", link_files[i][0]);// Path.GetFullPath(link_files[i][0]));
                    XmlElement link_el = xml_doc.CreateElement("Link", "http://schemas.microsoft.com/developer/msbuild/2003");
                    link_el.InnerText = prj_folder_link + "\\" + link_files[i][1];
                    compile_el.AppendChild(link_el);
                    childCompile.InsertAfter(compile_el, childCompile.LastChild);
                }
            }
            catch (Exception e)
            {
                Log(e.Message, 1);
            }
        }

        string[,] GetFilesUpdated(string[] cmd_output, string path)
        {
            List<string[]> tmp_result = new List<string[]>();
            for (int i = 0; i < cmd_output.Length; i++)
            {
                string[] key_value = cmd_output[i].Split(new char[] { ' ', '\x09' }, System.StringSplitOptions.RemoveEmptyEntries);
                string key = key_value[0].Trim().ToUpper(), value = (key_value.Length > 1) ? key_value[1].Trim(new char[] { ' ', '\x09', '\'', '\"' }) : null;
                //if (((key == "U") || (key == "!") || (key == "A") || (key == "RESTORED") || (key == "A") || (key == "UPDATING")) &&
                //cc.IsFile(value)) result.Add(value);
                //string upd_fl = (File.Exists(path+"\\"+value)) ? ((key == "A") ? "A" : "U") : "D";
                string upd_fl = (key == "A") ? "A" : (((key == "U") || (key == "RESTORED")) ? "U" : "D");
                if (cc.IsFile(value)) tmp_result.Add(new string[] { upd_fl, value });
            }

            string[,] result = new string[2, tmp_result.Count];
            for (int i = 0; i < tmp_result.Count; i++) {
                result[0, i] = tmp_result[i][0];
                result[1, i] = tmp_result[i][1];
            }
            return result;
        }

        string GetCmdCopyFiles(string path_src, string path_dist, string[] ext)
        {
            string result = "echo on";
            string[] files = null;
            try
            {
                files = Directory.GetFiles(path_src, "*.*", SearchOption.AllDirectories);
            }
            catch (Exception e)
            {
                Log(e.Message, 1);
            }
            for (int i = 0; i < files.Length; i++)
            {
                if ((Array.IndexOf(ext, Path.GetExtension(files[i]).Trim('.')) >= 0) || (ext[0] == "*"))
                {
                    result += "&copy \"" + files[i] + "\" \"" + path_dist + "\\" + Path.GetFileName(files[i]) + "\" /Y ";
                }
            }

            return result;
        }

        string[][] GetCopySVNFiles(int file, int block, int ffile)
        {
            List<string[]> tmp_result = new List<string[]>();

            var item = subsc_file[file].ini_file[block].files[ffile];
            string file_dir = item.dir;
            bool sync_path = item.sync_path;
            string[] ext = item.ext.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] files_svn = null;
            string svn_tmp_path = tmp_SVNFolders[file, block];

            try
            {
                files_svn = Directory.GetFiles(tmp_SVNFolders[file, block], "*.*", SearchOption.AllDirectories);
            }
            catch (Exception e)
            {
                return null;
            }

            for (int i = 0; i < item.file_name.Count; i++)
            {
                string f = item.file_name[i];
                if (f == "*")
                {
                    for (int j = 0; j < files_svn.Length; j++)
                    {
                        if ((Array.IndexOf(ext, Path.GetExtension(files_svn[j]).Trim('.')) >= 0) || (ext[0] == "*"))
                        {
                            string fc = file_dir + "\\" + ((sync_path) ? files_svn[j].Substring(files_svn[j].IndexOf(svn_tmp_path) + 1) : Path.GetFileName(files_svn[j]));
                            tmp_result.Add(new string[] { files_svn[j], fc });
                        }
                    }
                }
                else
                {
                    string dist_path = (f.IndexOf('.') > 0) ? (f.Substring(0, f.LastIndexOf('\\'))) : f;
                    if (dist_path == f)
                    {
                        for (int j = 0; j < files_svn.Length; j++)
                        {
                            if ((files_svn[j].ToUpper().IndexOf((svn_tmp_path + "\\" + f + "\\").ToUpper()) >= 0) 
                                && ((Array.IndexOf(ext, Path.GetExtension(files_svn[j]).Trim('.')) >= 0) || (ext[0] == "*")))
                            {
                                string fc = file_dir + "\\" + ((sync_path) ? files_svn[j].Substring(files_svn[j].IndexOf(svn_tmp_path) + 1) : Path.GetFileName(files_svn[j]));
                                tmp_result.Add(new string[] { files_svn[j], fc });
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j < files_svn.Length; j++)
                        {
                            if (files_svn[j].ToUpper().IndexOf(("\\" + f).ToUpper()) >= 0)
                            {
                                string fc = file_dir + "\\" + ((sync_path) ? files_svn[j].Substring(files_svn[j].IndexOf(svn_tmp_path) + 1) : Path.GetFileName(files_svn[j]));
                                tmp_result.Add(new string[] { files_svn[j], fc });
                                break;
                            }
                        }
                    }
                }                
            }

            return tmp_result.ToArray();
        }
            
        bool CheckNeededUpdPrj(int file, int block, int prj)
        {
            var item = subsc_file[file].ini_file[block].vsprojects[prj];

            StreamReader sr = new StreamReader(item.file_prj, false); 
            string file_prj = sr.ReadToEnd().ToUpper();
            sr.Close();

            string[] ext = item.ext.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int fc = subsc_file[file].ini_file[block].files.Count;
            string[][] files_link = GetProjectLinkFiles(file, block, prj);
            if ((files_link == null) || (files_link.Length == 0)) return false;
            string prj_folder_link = item.folder.ToUpper();
            for (int i = 0; i < files_link.Length; i++)
            {
                if ((Array.IndexOf(ext, Path.GetExtension(files_link[i][0]).Trim('.')) >= 0) || (ext[0] == "*"))
                {
                    if (file_prj.IndexOf("<LINK>" + prj_folder_link + "\\" + files_link[i][1].ToUpper() + "</LINK>") < 0) return true;
                }
            }
            return false;
        }

        bool CheckNeededUpdFiles(int file, int block, int ffile)
        {
            var item = subsc_file[file].ini_file[block].files[ffile];

            string[][] files_check = GetCopySVNFiles(file, block, ffile);
            if ((files_check == null) || (files_check.Length == 0)) return false;
            for (int i = 0; i < files_check.Length; i++)
            {
                if (!File.Exists(files_check[i][1])) return true;
                else if (!File.Exists(files_check[i][0])) return true;
                else if ((File.GetCreationTime(files_check[i][1]) - File.GetCreationTime(files_check[i][0])).TotalSeconds > 10) return true;
            }

            return false;
        }

        void UpdateFilesAndPrj(string[,] upd_files, int file, int block, bool first)
        {
            if (first)
            for (int i = file; i < subsc_file.Length; i++)
            {
                for (int j = block+1; j < subsc_file[i].blocks_count+1; j++)
                {
                    if (tmp_SVNFolders[i,j] == tmp_SVNFolders[file, block]) UpdateFilesAndPrj(upd_files, file, block, false);
                }
            }

            var item = subsc_file[file].ini_file[block];
            string path_svn = tmp_SVNFolders[file, block];

            if ((upd_files != null) && (upd_files.Length > 0))
            {
                string cmd_exec = "echo on";
                //bool upd_prj_fl = false;
                for (int i = 0; i < upd_files.Length / 2; i++)
                {
                    //string file_cp = path_svn + "\\"+upd_files[1, i];
                    string file_cp = upd_files[1, i].Replace('/', '\\').Trim(new char[] { '\\', ' ' });
                    string file_cp_short = file_cp.Substring(path_svn.Length + 1);
                    //if ((upd_files[0, i] == "A") || (upd_files[0, i] == "D")) upd_prj_fl = true;
                    if ((upd_files[0, i] == "A") || (upd_files[0, i] == "U"))
                    {
                        for (int j = 0; j < item.files.Count; j++)
                        {
                            bool sync_path = item.files[j].sync_path;
                            string[] ext = item.files[j].ext.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            string path_cp = item.files[j].dir;
                            if ((Array.IndexOf(ext, Path.GetExtension(file_cp).Trim('.')) >= 0) || (ext[0] == "*"))
                            {
                                if (sync_path)
                                {
                                    cmd_exec += "&echo F | xcopy \"" + file_cp + "\" \"" + path_cp + "\\" + file_cp_short + "\" /S /I /C /Y ";
                                }
                                else
                                {
                                    cmd_exec += "&copy \"" + file_cp + "\" \"" + path_cp + "\\" + Path.GetFileName(file_cp) + "\" /Y ";
                                }
                            }
                        }
                    }
                    else if (upd_files[0, i] == "D")
                    {
                        for (int j = 0; j < item.files.Count; j++)
                        {
                            string path_cp = item.files[j].dir;
                            cmd_exec += "&cd " + path_cp + "&del " + Path.GetFileName(file_cp) + " / S / F / Q";
                        }
                    }
                }
                ExecCmd(cmd_exec);

                //if (upd_prj_fl) RunSubscPrj(file, block);
                RunSubscPrj(file, block);
            }
        }

        string[] ExecCmd(string cmd)
        {
            List<string> result = new List<string>();
            if (cmd.Length > 8192)
            {
                string[] scmd = cmd.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
                string cmd_exec = "echo on";
                for (int i = 0; i < scmd.Length; i++)
                {
                    cmd_exec += "&" + scmd[i];
                    if (cmd_exec.Length > 7500)
                    {
                        result.AddRange(ExecCmd(cmd_exec).ToList());
                        cmd_exec = "echo on";
                    }
                }
                result.AddRange(ExecCmd(cmd_exec).ToList());
                return result.ToArray();
            }

            System.Diagnostics.Process proc = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "CMD.exe",

                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    //StandardOutputEncoding = Encoding.GetEncoding(1251),

                    //UseShellExecute = true,
                    //CreateNoWindow = false,
                    Arguments = " /C \"" + cmd + "\" "
                }
            };
            try
            {
                StringBuilder error = new StringBuilder();
                #region
                using (Process process = proc)
                {
                    int timeout = 10000;

                    using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
                    using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
                    {
                        process.OutputDataReceived += (sender, e) => {
                            if (e.Data == null)
                            {
                                outputWaitHandle.Set();
                            }
                            else
                            {
                                result.Add(e.Data);
                            }
                        };
                        process.ErrorDataReceived += (sender, e) =>
                        {
                            if (e.Data == null)
                            {
                                errorWaitHandle.Set();
                            }
                            else
                            {
                                error.AppendLine(e.Data);
                            }
                        };

                        process.Start();

                        process.BeginOutputReadLine();
                        process.BeginErrorReadLine();

                        if (process.WaitForExit(timeout) &&
                            outputWaitHandle.WaitOne(timeout) &&
                            errorWaitHandle.WaitOne(timeout))
                        {
                            // Process completed. Check process.ExitCode here.
                        }
                        else
                        {
                            // Timed out.
                        }
                    }
                }
                #endregion
                if (error.Length > 0) Log(error.ToString(), 1);

                return result.ToArray();

            }
            catch (Exception e)
            {
                Log(e.Message, 1);
            }
            return null;
        }

        public string CalculateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input.ToUpper());
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString();
        }

        public void RunSubscSVN(int file, int block)
        {
            var item = subsc_file[file].ini_file[block];
            string username, password, svn_url;
            username = item.svn.user;
            password = item.svn.password;
            svn_url = item.svn.url.Replace('\\', '/').Trim('/');
            if (tmp_SVNFolders == null) tmp_SVNFolders = new string[10, 100];
            tmp_SVNFolders[file, block] = tmp_SVNFolder+"\\" + CalculateMD5Hash(item.svn.url);
            //string exec_str = "rmdir \"" + tmp_SVNFolders[file, block] + "\" /S /Q&mkdir \"" + tmp_SVNFolders[file, block] + "\"";
            string exec_str = "mkdir \"" + tmp_SVNFolders[file, block] + "\"";
            exec_str += "&cd \"" + tmp_SVNFolders[file, block] + "\"";
            //exec_str += "&svn --username \"" + username + "\" --password \"" + password + "\" checkout \"" + svn_url + "\" \"" + tmp_SVNFolders[file, block] + "\" --depth empty --ignore-externals --force ";
            exec_str += "&svn --username \"" + username + "\" --password \"" + password + "\" checkout \"" + svn_url + "\" \"" + tmp_SVNFolders[file, block] + "\" --depth infinity --force ";
            Log("Checkout SVN [" + svn_url + "]",0);
            ExecCmd(exec_str);
        }

        public void RunSubscFile(int file, int block)
        {
            var item = subsc_file[file].ini_file[block];
            threads = null;
            if (item.files == null) return;
            string svn_url = item.svn.url.Replace('\\', '/').Trim('/');
            string svn_tmp_path = tmp_SVNFolders[file, block];
            for (int i = 0; i < item.files.Count; i++)
            {
                if (CheckNeededUpdFiles(file, block, i))
                {
                    string exec_cp_str = "echo on";
                    bool sync_path = item.files[i].sync_path;
                    string path_cp = item.files[i].dir;
                    string[] ext = item.files[i].ext.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    if (ext == null) ext = new string[] { "*" };
                    Log("Recreate directory [" + path_cp + "]", 0);
                    //ExecCmd("rmdir \"" + path_cp + "\" /S /Q " + "&mkdir \"" + path_cp + "\"", null);
                    if (forceUpdate) ExecCmd("rmdir \"" + path_cp + "\" /S /Q ");
                    ExecCmd("mkdir \"" + path_cp + "\"");
                    /*if (item.files[i].file_name.Count == 0)
                    {
                        //ExecCmd("svn checkout \"" + svn_url + "\" \"" + tmp_SVNFolders[file, block] + "\" --depth infinity --force ", null);
                        string[,] cmd_output = ExecCmd("svn checkout \"" + svn_url + "\" \"" + tmp_SVNFolders[file, block] + "\" --depth infinity --force ", tmp_SVNFolders[file, block]);
                        UpdateFilesAndPrj(cmd_output, file, block);
                        //exec_upd_str += "&svn checkout \"" + svn_url + "\" \"" + tmp_SVNFolders[file, block] + "\" --depth infinity --force ";
                        item.files[i].file_name.Add("*");
                    }*/
                    for (int j = 0; j < item.files[i].file_name.Count; j++)
                    {
                        string f = item.files[i].file_name[j];
                        //string path_cp = svn_url.Replace('\\', '/').Trim('/') + "/" + f.Replace('\\', '/');
                        //exec_upd_str += "&svn update " + ((f == "*") ? "" : ("\"" + f + "\""))+" --parents ";
                        if (sync_path)
                        {
                            string dist_path = (f.IndexOf('.') > 0) ? (f.Substring(0, f.LastIndexOf('\\'))) : f;
                            for (int ext_i = 0; ext_i < ext.Length; ext_i++)
                            {
                                if (dist_path == f) exec_cp_str += "&echo F | xcopy \"" + svn_tmp_path + ((f == "*") ? "" : ("\\" + f)) + "\\*." + ext[ext_i] + "\" \"" + path_cp + "\\" + dist_path + "\" /S /I /C /Y ";
                                else
                                {
                                    if (((ext[ext_i].ToUpper() == Path.GetExtension(f).Trim('.').ToUpper()) || (ext[0] == "*")) && ext_i == 0)
                                    {
                                        //if (!String.IsNullOrEmpty(dist_path)) exec_cp_str += "&mkdir \"" + path_cp + "\\" + dist_path + "\"";
                                        //exec_cp_str += "&copy \"" + svn_tmp_path + "\\" + f + "\" \"" + path_cp + "\\" + f + "\"  /Y";
                                        exec_cp_str += "&echo F | xcopy \"" + svn_tmp_path + "\\" + f + "\" \"" + path_cp + "\\" + f + "\" /S /I /C /Y ";
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (f == "*")
                            {
                                /*exec_cp_str += "&chcp 1251 > nul &for /f \"delims=\" %%a in (\"" + tmp_SVNFolders[file, block] + "\") do dir /a:-d /b /s \"%%a\" | find /V \".svn\" > %temp%\\~copy~files~only~.tmp";
                                exec_cp_str += "&for /f \"delims=\" %%b in (%temp%\\~copy~files~only~.tmp) do copy /b /y \"%%b\" \""+ path_cp + "\" ";
                                exec_cp_str += "&del %temp%\\~copy~files~only~.tmp";
                                */
                                exec_cp_str += "&" + GetCmdCopyFiles(svn_tmp_path, path_cp, ext);
                            }
                            else
                            {
                                string dist_path = (f.IndexOf('.') > 0) ? (f.Substring(0, f.LastIndexOf('\\'))) : f;
                                for (int ext_i = 0; ext_i < ext.Length; ext_i++)
                                {
                                    if (dist_path == f) exec_cp_str += "&echo F | xcopy \"" + svn_tmp_path + "\\" + f + "\\*." + ext[ext_i] + "\" \"" + path_cp + "\"  /S /I /C /Y ";
                                    else if (((ext[ext_i].ToUpper() == Path.GetExtension(f).Trim('.').ToUpper()) || (ext[0] == "*")) && ext_i == 0) exec_cp_str += "&copy \"" + svn_tmp_path + "\\" + f + "\" \"" + path_cp + "\\" + Path.GetFileName(f) + "\" /Y ";
                                }
                            }
                        }
                    }
                    ExecCmd(exec_cp_str);
                }
            }
        }

        public void RunSubscPrj(int file, int block)
        {
            var item = subsc_file[file].ini_file[block].vsprojects;
            if (item == null) return;
            for (int i = 0; i < item.Count; i++)
            {
                string prj_file = Path.GetFullPath(item[i].file_prj);
                try
                {
                    if ((CheckNeededUpdPrj(file, block, i)) || (forceUpdate))
                    {
                        XmlDocument xml_doc = new XmlDocument();
                        XmlReaderSettings readerSettings = new XmlReaderSettings();
                        readerSettings.IgnoreWhitespace = false;
                        readerSettings.IgnoreComments = false;

                        using (XmlReader reader = XmlReader.Create(prj_file, readerSettings)) xml_doc.Load(reader);

                        string prj_folder_link = item[i].folder;
                        XmlNode root = xml_doc.ChildNodes[1];
                        XmlNode childCompile = null;
                        if (root.HasChildNodes)
                        {
                            foreach (XmlNode child in root.ChildNodes)
                            {
                                if ((child.Name == "ItemGroup") && (child.HasChildNodes) && (child.ChildNodes[0].Name == "Compile"))
                                {
                                    XmlNodeList child_Compile = child.ChildNodes;
                                    XmlNode temp_node;
                                    for (int j = 0; j < child_Compile.Count; j++)
                                    {
                                        temp_node = child_Compile[j].ChildNodes[0];
                                        if ((child_Compile[j].HasChildNodes) && (temp_node.Name == "Link")
                                            && ((temp_node.InnerText.IndexOf(prj_folder_link + '\\') == 0)))
                                        {
                                            Console.WriteLine(temp_node.InnerText);
                                            child.RemoveChild(child_Compile[j]);
                                            j--;
                                        }
                                    }
                                    childCompile = child;
                                    break;
                                }
                            }
                            CollectProjectFile(file, block, i, xml_doc, childCompile);
                        }
                        xml_doc.Save(prj_file);
                        Log("Updated project [" + prj_file + "]", 0);
                    }
                }
                catch (Exception e)
                {
                    Log("Error updating project [" + prj_file + "]: " + e.Message, 1);
                }
            }

        }

        public void RunUpdateThreads(int file, int block)
        {
            if (threads == null)
            {
                threads = new List<Thread>();
                thread_fl = true;
            }
            var item = subsc_file[file].ini_file[block];
            string svn_tmp = tmp_SVNFolders[file, block];
            int period = Convert.ToInt32(Files_dgv.Rows[file].Cells[3].Value);
            string cmd_upd = "echo on&svn update \"" + svn_tmp + "\" ";
            Thread t = new Thread(delegate ()
            {
                int tblock = block, tfile = file, tperiod = period;
                string tcmd_upd = cmd_upd, tsvn_tmp = svn_tmp;
                string subsc = Files_dgv.Rows[file].Cells[1].Value.ToString() + ((block > 0) ? ("," + (block+1).ToString()) : "");
                while (thread_fl)
                {
                    if (!forceUpdate)
                    {
                        Thread.Sleep(tperiod * 1000);
                        string[,] cmd_output = GetFilesUpdated(ExecCmd(tcmd_upd), tsvn_tmp);// ExecCmd(tcmd_upd, tsvn_tmp);
                        UpdateFilesAndPrj(cmd_output, tfile, tblock, true);
                        Log("Refresh [" + subsc + "]: updated " + cmd_output.Length / 2 + " files", (cmd_output.Length == 0) ? 0 : 2);
                    }
                }
            });
            t.Start();
            threads.Add(t);
        }

        public void RunSubscribes()
        {
            int run_cnt = 0;
            for (int file = 0; file < Files_dgv.Rows.Count; file++)
            {
                if (Convert.ToBoolean(Files_dgv.Rows[file].Cells[0].Value)) {
                    for (int block = 0; block < subsc_file[file].blocks_count + 1; block++)
                    {
                        run_cnt++;
                        RunSubscSVN(file, block);
                        RunSubscFile(file, block);
                        RunSubscPrj(file, block);
                        RunUpdateThreads(file, block);
                    }
                }
            }
            if (run_cnt == 0)
            {
                Log("Nothing to run", 1);
                ChangeButtons(Stop_b);
            }
        }

        public void StopSubscribe()
        {
            if (threads != null)
            for (int i = 0; i < threads.Count; i++)
            {
                thread_fl = false;
                Thread.Sleep(300);
                threads[i].Abort();
                Thread.Sleep(300);
                threads[i] = null;
            }
            threads = null;
        }

        public void LoadSubscribes()
        {
            subsc_file = null;
            subsc_file = new IniFile[Files_dgv.Rows.Count];
            for (int i = 0; i < Files_dgv.Rows.Count; i++) {
                string file = Files_dgv.Rows[i].Cells[2].Value.ToString();
                string subsc_name = Files_dgv.Rows[i].Cells[1].Value.ToString(); ;
                subsc_file[i] = new IniFile();
                subsc_file[i].LoadINI(file);
                if (subsc_file[i].logs.Count > 0)
                {
                    for (int j = 0; j < subsc_file[i].logs.Count; j++) Log("Error loading subscribe [" + subsc_name + "]: " + subsc_file[i].logs[j], 1);
                } else Log("Loaded subscribe [" + subsc_name + "]", 0);
            }
        }

        delegate void SetLogCallback(string str, int type, bool console = true);

        //private void SetText(string text)
        //{
        //    InvokeRequired required compares the thread ID of the
        //     calling thread to the thread ID of the creating thread.
        //     If these threads are different, it returns true.
        //}

        void Log(string str, int type, bool console = true)
        {
            if (this.logs_rtb.InvokeRequired)
            {
                SetLogCallback d = new SetLogCallback(Log);
                this.Invoke(d, new object[] { str, type, true });
            }
            else
            {
                if (frmClose) return;
                logs_rtb.SelectionStart = logs_rtb.TextLength;
                logs_rtb.SelectionLength = 0;

                Color color;
                switch (type)
                {
                    case 0:
                        color = Color.Black;
                        break;
                    case 1:
                        color = Color.Red;
                        break;
                    case 2:
                        color = Color.Orange;
                        break;
                    default:
                        color = Color.DarkGray;
                        break;
                }
                logs_rtb.SelectionColor = color;
                Font old_font = logs_rtb.SelectionFont;
                logs_rtb.SelectionFont = new Font(logs_rtb.Font, FontStyle.Bold);
                logs_rtb.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ");
                logs_rtb.SelectionFont = old_font;
                logs_rtb.SelectionColor = logs_rtb.ForeColor;

                if (console)
                {
                    //Encoding enc_src = Encoding.GetEncoding(1251);
                    Encoding enc_src = Encoding.Default;
                    Encoding enc_dist = Encoding.GetEncoding(866);
                    byte[] src_bytes = enc_src.GetBytes(str);
                    str = enc_dist.GetString(src_bytes).TrimEnd('\n').TrimEnd('\r');
                }

                logs_rtb.AppendText(str + "\r\n");
            }
        }

        void OpenEditFile(string file)
        {
            if (File.Exists("C:\\Program Files (x86)\\Notepad++\\notepad++.exe")) Process.Start("C:\\Program Files (x86)\\Notepad++\\notepad++.exe", file.Trim());
            else Process.Start("C:\\Windows\\System32\\notepad.exe", file.Trim());
        }

        public void LoadINI(string file, int mode)
        {
            StreamReader sr;
            try { sr = new StreamReader(file, false); } catch (Exception e) { Text_dgv.Rows.Clear(); return; }
            if (mode == modeProgIni)
            {
                string subsc_name = null, subsc_path = null, subsc_check_period = null;
                bool subsc_active = false, fl_sr = false;
                int modeProg = -1, modeSubsc = 0, modeTMPFolder = 1;
                //Files_dgv.RowCount = 0;
                Files_dgv.Rows.Clear();
                while (!fl_sr)
                {
                    string str = (modeProg == modeSubsc) ? "[SUBSCRIBE]" : ((modeProg == modeTMPFolder) ? "[TMP_FOLDER]" : "");
                    if (!sr.EndOfStream) str = sr.ReadLine(); else fl_sr = true;
                    str = str.Trim();
                    if (str.IndexOf(';') != 0)
                    {
                        if ((str.ToUpper() == "[SUBSCRIBE]") || (str.ToUpper() == "[TMP_FOLDER]"))
                        {
                            if (!String.IsNullOrEmpty(subsc_name) && !String.IsNullOrEmpty(subsc_path))
                            {
                                /*                                str = str.Replace('/', '\\');
                                                                i = str.LastIndexOf('\\');
                                                                str_name = str.Substring(0, i);
                                                                j = str_name.LastIndexOf('\\');
                                                                str_name = str.Substring((j >= 0) ? j + 1 : i + 1);
                                */
                                Files_dgv.Rows.Add(subsc_active, subsc_name, subsc_path, (subsc_check_period ?? "60"));
                                Files_dgv.Rows[Files_dgv.Rows.Count - 1].Cells[1].ToolTipText = subsc_path;
                            }
                            else if (modeProg == modeSubsc) Log("Could not load some subscribe", 1, false);
                            subsc_name = null;
                            subsc_path = null;
                            subsc_check_period = null;
                            subsc_active = false;
                            if (str.ToUpper() == "[SUBSCRIBE]") modeProg = modeSubsc;
                            else if (str.ToUpper() == "[TMP_FOLDER]") modeProg = modeTMPFolder;
                            else modeProg = -1;
                        }
                        else if (modeProg == modeSubsc)
                        {
                            string[] key_value = str.Split('=');
                            string key = key_value[0].Trim().ToUpper(), value = (key_value.Length > 1) ? key_value[1].Trim() : "";
                            if (key == "NAME") subsc_name = value;
                            if (key == "CHECK_PERIOD") subsc_check_period = value;
                            if (key == "PATH")
                            {
                                string[] items = value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                subsc_path = (items.Length > 0) ? items[0].Trim() : null;
                                subsc_active = (items.Length > 1) ? ((items[1].Trim() == "N") ? false : true) : true;
                            }
                        }
                        else if (modeProg == modeTMPFolder)
                        {
                            string[] key_value = str.Split('=');
                            string key = key_value[0].Trim().ToUpper(), value = (key_value.Length > 1) ? key_value[1].Trim() : "";
                            if (key == "PATH") tmp_SVNFolder = value.Replace('/', '\\').Trim('\\');
                        }
                    }
                }
                LoadSubscribes();
            }
            else if (mode == modeSubscIni)
            {
                Text_dgv.Rows.Clear();
                while (!sr.EndOfStream)
                {
                    string str = sr.ReadLine();
                    str = str.Trim();
                    if (!((hideComments) && (str.Trim().IndexOf(';') == 0))) Text_dgv.Rows.Add(str);
                    if (str.Trim().IndexOf('[') == 0) Text_dgv.Rows[Text_dgv.RowCount - 2].DefaultCellStyle.BackColor = Color.LightGray;
                }
            }
            sr.Close();
        }

        void ChangeButtons(object b)
        {
            if (b == Start_b)
            {
                Start_b.Enabled = false;
                Start_b.BackColor = Color.LightGray;
                Stop_b.Enabled = true;
                Stop_b.BackColor = Color.Red;
            }
            else if (b == Stop_b)
            {
                Start_b.Enabled = true;
                Start_b.BackColor = Color.Lime;
                Stop_b.Enabled = false;
                Stop_b.BackColor = Color.LightGray;
            }
            else if (b == run_now_b)
            {
                if (cb_fl == 1)
                {
                    run_now_b.Enabled = false;
                    run_now_b.BackColor = Color.LightGray;
                    cb_fl = 2;
                }
                else
                {
                    run_now_b.Enabled = true;
                    run_now_b.BackColor = Color.OrangeRed;
                    cb_fl = 1;
                }
            }

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveSmartSVNiniToolStripMenuItem_Click(this,null);
            frmClose = true;
            Stop_b_Click(this, null);
            Application.Exit();
        }


        private bool SubscExists(string file)
        {
            string f = Path.GetFullPath(file).ToUpper();
            for (int i = 0; i < Files_dgv.RowCount; i++)
            {
                if (f == Files_dgv.Rows[i].Cells[2].Value.ToString()) return true;
            }
            return false;
        }

        private void AddSubscribe(string file)
        {
            List<string> lines = new List<string>();
            string ssinif = Application.StartupPath + "\\SmartSVN.ini";
            try
            {
                lines = File.ReadAllLines(ssinif).ToList();
                string f = Path.GetFullPath(file);
                if (!SubscExists(f))
                {
                    lines.Add("");
                    lines.Add("[Subscribe]");
                    lines.Add("name = " + Path.GetFileNameWithoutExtension(f));
                    lines.Add("check_period = 180");
                    lines.Add("path = " + cc.GetRelativePath(f, Application.StartupPath));
                }
                lines.Add("");
                File.WriteAllLines(ssinif, lines);
            }
            catch (Exception e)
            {
                Log(e.Message, 1);
            }
        }

        private void loadiniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenINI_ofd.InitialDirectory = Path.GetFullPath(".\\");
            OpenINI_ofd.ShowDialog();
            string f = OpenINI_ofd.FileName;
            AddSubscribe(f);
            LoadINI(Path.GetFullPath(".\\") + "SmartSVN.ini", modeProgIni);
        }

        private void Start_b_Click(object sender, EventArgs e)
        {
            ChangeButtons(sender);
            Log("выполняется [Запуск]", 2, false);
            RunSubscribes();
        }

        private void Stop_b_Click(object sender, EventArgs e)
        {
            StopSubscribe();
            Log("выполняется [Остановить]", 2, false);
            ChangeButtons(sender);
        }

        private void refresh_ini_b_Click(object sender, EventArgs e)
        {
            Stop_b_Click(this, null);
            Log("выполняется [Обновить SmartSVN.ini]", 2, false);
            LoadINI(Application.StartupPath + "\\" + "SmartSVN.ini", modeProgIni);
            LoadINI(Files_dgv.Rows[0].Cells[2].Value.ToString(), modeSubscIni);
        }

        private void Files_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadINI(Files_dgv.Rows[e.RowIndex].Cells[2].Value.ToString(), modeSubscIni);
        }

        private void Files_dgv_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            OpenEditFile(Files_dgv.Rows[e.RowIndex].Cells[2].Value.ToString());
        }

        private void SmartSVN_ini_link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenEditFile(Path.GetFullPath(".\\") + "SmartSVN.ini");
        }

        private void run_now_b_Click(object sender, EventArgs e)
        {
            var b = (Button)sender;
            ChangeButtons(sender);
            Log("выполняется Выполнить", 2, false);
            forceUpdate = true;
            for (int file = 0; file < Files_dgv.Rows.Count; file++)
            {
                if (Convert.ToBoolean(Files_dgv.Rows[file].Cells[0].Value))
                {
                    for (int block = 0; block < subsc_file[file].blocks_count + 1; block++)
                    {
                        RunSubscSVN(file, block);
                        RunSubscFile(file, block);
                        RunSubscPrj(file, block);
                    }
                }
            }
            forceUpdate = false;
            ChangeButtons(sender);
        }

        private void Files_dgv_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                Files_dgv_CellContentDoubleClick(this, e);
            }
        }

        private void ClearLog_b_Click(object sender, EventArgs e)
        {
            Log("выполняется [Очистить Лог]", 2, false);
            if (log_errors_cb.Checked)
            {
                RichTextBox rtb = new RichTextBox();
                rtb.Clear();
                Font f = logs_rtb.Font;
                for (int i = 0; i < logs_rtb.Lines.Length; i++)
                {
                    int start_index = logs_rtb.GetFirstCharIndexFromLine(i);
                    int count = logs_rtb.GetFirstCharIndexFromLine(i + 1) - start_index;
                    logs_rtb.SelectionStart = start_index;
                    logs_rtb.SelectionLength = 1;
                    Color color = logs_rtb.SelectionColor;
                    if ((color == Color.Red) || (color == Color.Orange))
                    {
                        logs_rtb.SelectionStart = start_index;
                        logs_rtb.SelectionLength = count;
                        string logs_text = logs_rtb.SelectedRtf;
                        rtb.Select(rtb.TextLength, 0);
                        rtb.SelectedRtf = logs_text;
                    }
                }
                logs_rtb.Rtf = rtb.Rtf;
                logs_rtb.Font = f;
            }
            else
                logs_rtb.Clear();
        }

        private void mainf_FormClosing(object sender, FormClosingEventArgs e)
        {
            exitToolStripMenuItem_Click(this, null);
        }

        private void smartIniAdd_b_Click(object sender, EventArgs e)
        {
            if (Subsc_ofd.ShowDialog() == DialogResult.OK)
            {
                string file;
                try
                {
                    if ((file = Subsc_ofd.FileName) != null)
                    {
                        string file_ssini = Path.GetFullPath(".\\") + "SmartSVN.ini";
                        StreamWriter sw;
                        sw = new StreamWriter(file_ssini, false);
                        //string str = sr.ReadToEnd();
                        string br = "/r/n";
                        string str = br + "[Subscribe]" + br + "name = " + Path.GetFileNameWithoutExtension(file) + br + "check_period = 180"
                            + br + "path = " + Path.GetFullPath(file) + ",N" + br;
                        sw.Write(str);
                        sw.Close();
                    }
                }
                catch (Exception ex)
                {
                    Log(ex.Message, 1, false);
                }
            }
        }

        private void saveSmartSVNiniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string file_ssini = Path.GetFullPath(".\\") + "SmartSVN.ini";
                string[] file = File.ReadAllLines(file_ssini);
                for (int i = 0; i < Files_dgv.RowCount; i++)
                {
                    string path = Files_dgv.Rows[i].Cells[2].Value.ToString();
                    string active = (Convert.ToBoolean(Files_dgv.Rows[i].Cells[0].EditedFormattedValue.ToString()) ? "" : ",N");
                    for (int j = 0; j < file.Length; j++)
                    {
                        if ((file[j].IndexOf(path) >= 0) && (file[j].Trim().IndexOf(";") <= 0))
                        {
                            string[] key_value = file[j].Split('=');
                            string key = key_value[0].Trim().ToUpper(), value = (key_value.Length > 1) ? key_value[1].Trim() : "";
                            if (key == "PATH") file[j] = "path = " + path + active;
                        }
                    }
                }
                File.WriteAllLines(file_ssini, file);
            }
            catch (Exception ex)
            { 
                Log(ex.Message, 1, false);
            }
        }

        private void saveSubscribeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string file_subsc = Path.GetFullPath(Files_dgv.Rows[Files_dgv.CurrentRow.Index].Cells[2].Value.ToString());
                List<string> lines = new List<string>();
                foreach (DataGridViewRow item in Text_dgv.Rows)
                {
                    if (item.Cells[0].Value != null) lines.Add(item.Cells[0].EditedFormattedValue.ToString());
                }
                File.WriteAllLines(file_subsc, lines);
            }
            catch (Exception ex)
            {
                Log(ex.Message, 1, false);
            }
        }

        void sif_Closed(object sender, FormClosedEventArgs e)
        {
            refresh_ini_b_Click(this, null);
        }

        private void scanSubscribesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sif.IsDisposed) sif = new scaninif();
            sif.Show();
        }

        private void comments_cb_CheckedChanged(object sender, EventArgs e)
        {
            hideComments = comments_cb.Checked;
            LoadINI(Files_dgv.Rows[Files_dgv.CurrentRow.Index].Cells[2].Value.ToString(), modeSubscIni);
        }

        private void mainf_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowInTaskbar = false;
                tray_ni.Visible = true;
            }
        }

        private void tray_ni_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
                this.ShowInTaskbar = true;
                tray_ni.Visible = false;
            }
        }

        private void tray_ni_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)  // shows error ate button
            {
                return;
            }
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // code for adding context menu
            }
        }


    }
}
