using System;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Runtime.InteropServices;

public struct setStruct
{
    public string set;         // оригинальное множество
    public int value_type;     // тип значений множества: 0 - Int32, 1 - Int64, 20 - float, 50 - DateTime, 100 - string
    public string str1, str2;  // строки
    public int int1, int2;     // целые 32
    public long long1, long2;  // целые 64
    public DateTime dt1, dt2;  // даты
    public double f1, f2;      // вещественные
    public bool limit1, limit2;// включать значение или нет
    public bool isNeg;         // отрицание
}

public struct paramsStruct
{
    public string name;  // имя параметра
    public string value; // значение 
    public string isNeg; // отрицание

    public paramsStruct(string pname, string pvalue, string pisNeg)
    {
        name = pname;
        value = pvalue;
        isNeg = pisNeg;
    }
}

public struct TypeInfo
{
    public int type;
    public int size;
    public int integer;
    public int fraction;

    public TypeInfo(int ptype, int psize, int pinteger, int pfraction)
    {
        type = ptype;
        size = psize;
        integer = pinteger;
        fraction = pfraction;
    }
}

class IniFile
{
    string Path;
    public int secNone= -1;
    public int secSVN = 1;
    public int secFiles = 2;
    public int secVSProjects = 3;
    public List<string> logs;

    public struct svn_s
        {
            public string url;
            public string user;
            public string password;
            public int check_period;
            public svn_s(string purl, string puser, string ppassword, int pcheck_period)
            {
                url = purl;
                user = puser;
                password = ppassword;
                check_period = pcheck_period;
            }
        }
    public struct files_s
        {
            public string dir;
            public bool sync_path;
            public string ext;
            public List<string> file_name;
            public files_s(string pdir, bool psync_path, string pext, List<string> pfile_name)
            {
                dir = pdir;
                sync_path = psync_path;
                ext = pext;
                file_name = pfile_name;
            }
        }
    public struct vsprojects_s
        {
            public string file_prj;
            public string folder;
            public string ext;
            public List<string> file_name;
            public vsprojects_s(string pfile_prj, string pfolder, string pext, List<string> pfile_name)
            {
                file_prj = pfile_prj;
                folder = pfolder;
                ext = pext;
                file_name = pfile_name;
            }
        }

    public struct blocks_s
        {
            public svn_s svn;
            public List<files_s> files;
            public List<vsprojects_s> vsprojects;
            public blocks_s(svn_s psvn, List<files_s> pfiles, List<vsprojects_s> pvsprojects)
            {
                svn = psvn;
                files = pfiles;
                vsprojects = pvsprojects;
            }
        }

    public blocks_s[] ini_file;
    public int blocks_count;

    public IniFile()
    {
        logs = new List<string>();
    }

    private object AddSection(object item)
    {
        files_s fs;
        vsprojects_s ps;
        if (item.GetType() == typeof(vsprojects_s))
        {
            ps = (vsprojects_s)item;
            if (String.IsNullOrEmpty(ps.ext)) ps.ext = "*";
            if ((ps.file_name == null) || (ps.file_name.Count == 0)) ps.file_name.Add("*");
            if ((!String.IsNullOrEmpty(ps.file_prj)) && (!String.IsNullOrEmpty(ps.folder)))
            {
                ini_file[blocks_count].vsprojects.Add(ps);
            }
            else logs.Add("Skipped vsprojects section");
            ps = new vsprojects_s(null, null, null, new List<string>());
            return ps;
        }
        else if (item.GetType() == typeof(files_s))
        {
            fs = (files_s)item;
            if (String.IsNullOrEmpty(fs.ext)) fs.ext = "*";
            if ((fs.file_name == null) || (fs.file_name.Count == 0)) fs.file_name.Add("*");
            if (!String.IsNullOrEmpty(fs.dir))
            {
                ini_file[blocks_count].files.Add(fs);
                fs = new files_s(null, true, null, new List<string>());
            }
            else logs.Add("Skipped files section");
            return fs;
        }
        return null;
    }

    public void LoadINI(string file)
    {
        StreamReader sr;
        ini_file = null;
        blocks_count = -1;
        logs.Clear();
        try { sr = new StreamReader(file, false); } catch (Exception e) { logs.Add(e.Message); return; }
        ini_file = new blocks_s[100];
        int section = 0;
        int i, j;
        files_s tmp_files = new files_s(null, true, null, new List<string>());
        vsprojects_s tmp_vsprj = new vsprojects_s(null, null, null, new List<string>());
        section = secNone;
        while (!sr.EndOfStream)
        {
            string str = sr.ReadLine();
            str = str.Trim();
            if ((str.IndexOf(';') != 0) && !String.IsNullOrEmpty(str))
            {
                if (str.ToUpper() == "[SVN]")
                {
                    if (section == secVSProjects) tmp_vsprj = (vsprojects_s)AddSection(tmp_vsprj);
                    if (section == secFiles) tmp_files = (files_s)AddSection(tmp_files);
                    section = secSVN;
                    //tmp_files = new files_s(null, false, null, new List<string>());
                    //tmp_vsprj = new vsprojects_s(null, null, null, new List<string>());
                    if (blocks_count >= 0)
                    {
                        if (String.IsNullOrEmpty(ini_file[blocks_count].svn.url) || String.IsNullOrEmpty(ini_file[blocks_count].svn.user) ||
                            String.IsNullOrEmpty(ini_file[blocks_count].svn.password))
                        {
                            logs.Add("Skipped svn block" + blocks_count.ToString());
                            blocks_count--;
                        }
                    }
                    else logs.Clear();
                    if (blocks_count >= 0) if (ini_file[blocks_count].files.Count == 0)
                        {
                            logs.Add("SVN section is wrong: has not [Files] sections");
                            blocks_count--;
                        }
                    blocks_count++;
                    ini_file[blocks_count] = new blocks_s(new svn_s(), null, null);
                    ini_file[blocks_count].files = new List<files_s>();
                    ini_file[blocks_count].vsprojects = new List<vsprojects_s>();
                }
                else if (str.ToUpper() == "[FILES]")
                {
                    if (section == secVSProjects) tmp_vsprj = (vsprojects_s)AddSection(tmp_vsprj);
                    if (section == secFiles) tmp_files = (files_s)AddSection(tmp_files);
                    section = secFiles;
                }
                else if (str.ToUpper() == "[VSPROJECTS]")
                {
                    if (section == secVSProjects) tmp_vsprj = (vsprojects_s)AddSection(tmp_vsprj);
                    if (section == secFiles) tmp_files = (files_s)AddSection(tmp_files);
                    section = secVSProjects;
                }
                else {
                    string[] key_value = str.Split('=');
                    string key = key_value[0].Trim().ToUpper(), value = (key_value.Length > 1) ? key_value[1].Trim() : "";
                    if (section == secSVN)
                    {
                        if (key == "SVN_URL") ini_file[blocks_count].svn.url = value.Replace('/','\\').Trim('\\');
                        if (key == "SVN_USER") ini_file[blocks_count].svn.user = value;
                        if (key == "SVN_PASSWORD") ini_file[blocks_count].svn.password = value;
                        //if (key == "SVN_CHECK_PERIOD") ini_file[blocks_count].svn.check_period = Convert.ToInt16(value);
                    }
                    else if (section == secFiles)
                    {
                        if (key == "FILE_DIR") tmp_files.dir = value.Replace('/', '\\').Trim('\\');
                        if (key == "SYNC_PATH") tmp_files.sync_path = (value.ToUpper() == "Y") ? true : false;
                        if (key == "FILE_EXT") tmp_files.ext = value;
                        if (key == "FILE_NAME") tmp_files.file_name.Add(value.Replace('/', '\\').Trim('\\'));
                    }
                    else if (section == secVSProjects)
                    {
                        if (key == "VSPROJECT_FILE") tmp_vsprj.file_prj = value;
                        if (key == "VSPROJECT_FOLDER") tmp_vsprj.folder = value.Replace('/', '\\').Trim('\\');
                        if (key == "FILE_EXT") tmp_vsprj.ext = value;
                        if (key == "FILE_NAME") tmp_vsprj.file_name.Add(value.Replace('/', '\\').Trim('\\'));
                    }
                }
            }
        }
        if (section == secVSProjects) tmp_vsprj = (vsprojects_s)AddSection(tmp_vsprj);
        if (section == secFiles) tmp_files = (files_s)AddSection(tmp_files);
        if (blocks_count >= 0) if (ini_file[blocks_count].files.Count == 0)
            {
                logs.Add("SVN section is wrong: has not [Files] sections");
                blocks_count--;
            }
        if (blocks_count < 0) logs.Add("Subscription does not loaded: all blocks skipped");
        sr.Close();
    }

    public void SaveINI(string file)
    {

    }

}

public static class cc
{
    /* Типы
     *-100 - infinity
     *-1 - none
     * 0 - Int32
     * 1 - Int64
     * 20 - float
     * 50 - DateTime
     * 100 - string
     */

    public static List<string> list_string;
    public static Dictionary<int, string> dict_string;

    public static List<string> list_string1;
    public static Dictionary<int, string> dict_string1;

    public static int type_element_count;
    public static TypeInfo type_info1;
    public static TypeInfo type_info2;

    public static string str_debug;

    public static int TypeInfinity = -100;
    public static int TypeNone     = -1;
    public static int TypeInt32    = 1;
    public static int TypeInt64    = 2;
    public static int TypeDouble   = 3;
    public static int TypeDateTime = 50;
    public static int TypeString   = 100;

    public static string TypeStrNone     = "null";
    public static string TypeStrInfinity = "infinity";
    public static string TypeStrInt32    = "int";
    public static string TypeStrInt64    = "bigint";
    public static string TypeStrDouble   = "numeric";
    public static string TypeStrDateTime = "datetime";
    public static string TypeStrString   = "varchar";

    public static string setMergeAB = ">";
    public static string setMergeBA = "<";
    public static string setIntersection = "^";
    public static string setUnion = "U";
    public static string setEqual = "=";
    public static string setEmpties = "*";
    public static string setRComplement = "-";
    public static string setSort = "S";

    public static string setInfinity = "*";
    public static string setNegInfinity = "_*";

    public static string TypeToString(int type_int) // тип строки
    {
        string res = TypeStrString;
        if (type_int == TypeInt32) res = TypeStrInt32;
        else if (type_int == TypeInt64) res = TypeStrInt64;
        else if (type_int == TypeDouble) res = TypeStrDouble;
        else if (type_int == TypeDateTime) res = TypeStrDateTime;
        else if (type_int == TypeInfinity) res = TypeStrInfinity;
        return res;
    }

    public static int StringType(string str, int prev_type = -1) // тип строки
    {
        if (prev_type == TypeString) return TypeString;
        if (String.IsNullOrEmpty(str)) return prev_type;

        int res = TypeString, prec_pos;
        string s, str_int, str_prec = "";
        int out0;
        //long out1;
        double out20;
        //DateTime out50;
        prec_pos = str.IndexOf(".");
        if (prec_pos > 0) str_prec = str.Substring(prec_pos + 1).TrimEnd('0');
        if (str_prec == "") // целое число
        {
            s = (str.IndexOf('-') == 0) ? str.Substring(1) : str;
            str_int = (prec_pos > 0) ? s.Substring(0, prec_pos).TrimStart('0') : s;
            str_int = ((str_int == "") && (prec_pos > 0)) ? "0" : str_int;
            if (Regex.IsMatch(str_int, "^\\d+$")) 
            if (Int32.TryParse(str_int, out out0)) res = TypeInt32;
            else if (str_int.Length < 300) res = TypeInt64;
        }
        if ((res == TypeString) && (str.IndexOf(",") < 0)) if (double.TryParse(str.Replace('.', ','), out out20)) res = TypeDouble; // дробное 
        //else if (DateTime.TryParse(str, out out50)) res = TypeDateTime; // дата
        if (str.Trim() == "*") res = TypeInfinity; // бесконечность

        //if (res == TypeDateTime) // тип дата
        //    if ((prev_type == TypeDateTime) || (prev_type == -1)) return TypeDateTime; else return TypeString;

        //Console.WriteLine("str {0}  type {1} ", str, Math.Max(res, prev_type));
        return Math.Max(res, prev_type);
    }

    public static TypeInfo StringTypeInfo(string str, TypeInfo prev_TypeInfo) // информация по типу строки
    {
        if (str == null) return prev_TypeInfo;
        if (str.Trim() == "*") return new TypeInfo(TypeInfinity, -1, -1, -1); // бесконечность

        TypeInfo result = new TypeInfo(TypeString, -1, -1, -1);
        string value = str;

        double number;
        if (Double.TryParse(value.Replace(".", ","), out number) && (value.IndexOf(',') < 0)) // число
        {
            string str_integer, str_fraction;
            int prec = value.IndexOf(".");
            str_integer = (prec >= 0) ? value.Substring(0, prec) : value.Substring(0);
            str_fraction = (prec >= 0) ? value.Substring(prec+1) : "";
            result.integer = str_integer.TrimStart(new char[] { '-', '0', ' ' }).Length;
            result.integer = (result.integer > 0) ? result.integer : 1;
            result.fraction = str_fraction.TrimEnd(new char[] { '0', ' ' }).Length;
            result.size = result.integer + result.fraction;
            if (result.fraction > 0) result.type = TypeDouble;
            else if (result.integer <= 10) result.type = TypeInt32;
            else result.type = TypeInt64;
        }
        else // строка
        {
            result.type = TypeString;
            result.size = value.Length;
            result.size = (result.size > 0) ? result.size : 1;
        }

        if ((prev_TypeInfo.type == TypeString) || (result.type == TypeString)) // сравнить с предыдущим типом
        {
            result.type = TypeString;
            result.size = Math.Max(prev_TypeInfo.size, result.size);
            result.size = (result.size <= 0) ? 1 : result.size;
            result.integer = -1;
            result.fraction = -1;
        }        
        else if ((prev_TypeInfo.type >= TypeInt32) && (prev_TypeInfo.type <= TypeDouble))
        {
            if ((prev_TypeInfo.type == TypeDouble) || (result.type == TypeDouble)) result.type = TypeDouble;
            else if ((prev_TypeInfo.type == TypeInt64) || (result.type == TypeInt64)) result.type = TypeInt64;
            else if ((prev_TypeInfo.type == TypeInt32) || (result.type == TypeInt32)) result.type = TypeInt32;
            else return prev_TypeInfo;
            result.size = Math.Max(prev_TypeInfo.size, result.size);
            result.integer = Math.Max(prev_TypeInfo.integer, result.integer);
            result.fraction = Math.Max(prev_TypeInfo.fraction, result.fraction);
        }

        return result;
    }

    public static string TypeInfoToStr(TypeInfo type_info)
    {
        string result = "";
        result = TypeToString(type_info.type);
        if (type_info.type == TypeString) result += "(" +((type_info.size > 0) ? type_info.size : 1).ToString()+")";
        else if (type_info.type == TypeDouble) result += "(" + type_info.size.ToString() + ","+ type_info.fraction.ToString() + ")";

        return result;
    }


    public static string StringMerge(string str1, string str2, string delim) // "правильно" объединяет строки с разделителем
    {
        string res = "";
        if (str1.IndexOf(delim) == str1.Length) res = str1 + str2;
        else res = str1 + delim + str2;
        return res;
    }

    public static void RemoveDuplicates(List<string> List) // убрать дубликаты из списка
    {
        int NumUnique = 0;
        for (int i = 0; i < List.Count; i++)
            if ((i == 0) || (List[NumUnique - 1] != List[i]))
                List[NumUnique++] = List[i];
        List.RemoveRange(NumUnique, List.Count - NumUnique);
    }

    public static string StringSort(string str, int type, string delim = ",", int distinct = 0) // сортировка в строке
    {
        string res = str;
        if (!string.IsNullOrEmpty(res))
        {
            List<string> split = new List<string>(res.Split(new string[] { delim }, StringSplitOptions.RemoveEmptyEntries));
            split.Sort();
            if (distinct != 0) RemoveDuplicates(split);
            string[] res_items = new string[split.Count];
            if (type == cc.TypeInt32)
            {
                Int32[] items = new Int32[split.Count];
                for (int i = 0; i < items.Length; i++) items[i] = Convert.ToInt32(split[i]);
                Array.Sort(items);
                for (int i = 0; i < items.Length; i++) res_items[i] = items[i].ToString();
                res = string.Join(delim, res_items);
                //Int32[] items = items.OrderBy(s => int.Parse(s.Split('|').Last())).ToList();
            }
            else if (type == cc.TypeDouble)
            {
                float[] items = new float[split.Count];
                for (int i = 0; i < items.Length; i++) items[i] = Convert.ToSingle(split[i].Replace('.', ','));
                Array.Sort(items);
                for (int i = 0; i < items.Length; i++) res_items[i] = items[i].ToString().Replace(',', '.');
                res = string.Join(delim, res_items);
            }
            else if (type == cc.TypeDateTime)
            {
                DateTime[] items = new DateTime[split.Count];
                for (int i = 0; i < items.Length; i++) items[i] = Convert.ToDateTime(split[i]);
                Array.Sort(items);
                for (int i = 0; i < items.Length; i++) res_items[i] = items[i].ToString();
                res = string.Join(delim, res_items);
            }
            else if (type == cc.TypeString)
            {
                res = string.Join(delim, split.ToArray());
            }
        }
        return res;
    }

    public static int CompareInfinity(string str1, string str2, int def) // сравнение безконечностей
    {
        int res = def, int1 = 0, int2 = 0;
        if (str1 == setInfinity) int1 = 1;
        if (str1 == setNegInfinity) int1 = -1;
        if (str2 == setInfinity) int2 = 1;
        if (str2 == setNegInfinity) int2 = -1;

        if ((int1 == 0) && (int2 == 0)) res = def;
        else res = (int1 < int2) ? -1 : ((int1 > int2) ? 1 : 0);

        return res;
    }

    public static string setsCompare(setStruct set1, setStruct set2, string oper, out string res_out) // сравнение
    {
        string res_set = "", val1 = "", val2 = "";
        int s11s21 = 0, s12s21 = 0, s11s22 = 0, s12s22 = 0;
        int val_type = set1.value_type;
        #region получить результат сравнения границ
        if (val_type == cc.TypeString) // сравнивает строки
        {
            s11s21 = String.Compare(set1.str1, set2.str1, true);
            s12s21 = String.Compare(set1.str2, set2.str1, true);
            s11s22 = String.Compare(set1.str1, set2.str2, true);
            s12s22 = String.Compare(set1.str2, set2.str2, true);
        }
        else if (val_type == cc.TypeInt32) // сравнивает целые 32
        {
            s11s21 = (set1.int1 < set2.int1) ? -1 : ((set1.int1 > set2.int1) ? 1 : 0);
            s12s21 = (set1.int2 < set2.int1) ? -1 : ((set1.int2 > set2.int1) ? 1 : 0);
            s11s22 = (set1.int1 < set2.int2) ? -1 : ((set1.int1 > set2.int2) ? 1 : 0);
            s12s22 = (set1.int2 < set2.int2) ? -1 : ((set1.int2 > set2.int2) ? 1 : 0);
        }
        else if (val_type == cc.TypeInt64) // сравнивает целые 64
        {
            s11s21 = (set1.long1 < set2.long1) ? -1 : ((set1.long1 > set2.long1) ? 1 : 0);
            s12s21 = (set1.long2 < set2.long1) ? -1 : ((set1.long2 > set2.long1) ? 1 : 0);
            s11s22 = (set1.long1 < set2.long2) ? -1 : ((set1.long1 > set2.long2) ? 1 : 0);
            s12s22 = (set1.long2 < set2.long2) ? -1 : ((set1.long2 > set2.long2) ? 1 : 0);
        }
        else if (val_type == cc.TypeDouble) // сравнивает дробные числа
        {
            s11s21 = (set1.f1 < set2.f1) ? -1 : ((set1.f1 > set2.f1) ? 1 : 0);
            s12s21 = (set1.f2 < set2.f1) ? -1 : ((set1.f2 > set2.f1) ? 1 : 0);
            s11s22 = (set1.f1 < set2.f2) ? -1 : ((set1.f1 > set2.f2) ? 1 : 0);
            s12s22 = (set1.f2 < set2.f2) ? -1 : ((set1.f2 > set2.f2) ? 1 : 0);
        }
        else if (val_type == cc.TypeDateTime) // сравнивает даты
        {
            s11s21 = DateTime.Compare(set1.dt1, set2.dt1);
            s12s21 = DateTime.Compare(set1.dt2, set2.dt1);
            s11s22 = DateTime.Compare(set1.dt1, set2.dt2);
            s12s22 = DateTime.Compare(set1.dt2, set2.dt2);
        }
        //s11s21 = CompareInfinity(set1.str1, set2.str1, s11s21);
        //s12s21 = CompareInfinity(set1.str2, set2.str1, s12s21);
        //s11s22 = CompareInfinity(set1.str1, set2.str2, s11s22);
        //s12s22 = CompareInfinity(set1.str2, set2.str2, s12s22);
        #endregion

        res_out = setUnion;
        #region объединение
        if (oper == setUnion) 
        {
            if ((s11s21 == 0) && (s12s22 == 0)) // =
            {
                res_out = setEqual;
                res_set += (!((set1.limit1) || (set2.limit1)) ? "(" : "") + set1.str1 + ".." + set1.str2 + (!((set1.limit2) || (set2.limit2)) ? ")" : "") + ";";
            }
            else if ((s11s21 <= 0) && (s12s22 >= 0)) // >
            {
                res_out = setMergeAB;
                res_set += ((((s11s21 < 0) && (!set1.limit1)) || ((s11s21 == 0) && (!set1.limit1 && !set2.limit1))) ? "(" : "") + set1.str1 + ".." + set1.str2 + ((((s12s22 > 0) && (!set1.limit2)) || ((s12s22 == 0) && (!set1.limit2 && !set2.limit2))) ? ")" : "") + ";";
            }
            else if ((s11s21 >= 0) && (s12s22 <= 0)) // <
            {
                res_out = setMergeBA;
                res_set += ((((s11s21 > 0) && (!set2.limit1)) || ((s11s21 == 0) && (!set1.limit1 && !set2.limit1))) ? "(" : "") + set2.str1 + ".." + set2.str2 + ((((s12s22 < 0) && (!set2.limit2)) || ((s12s22 == 0) && (!set1.limit2 && !set2.limit2))) ? ")" : "") + ";";
            }
            else if (((s11s21 < 0) && (s12s21 > 0)) || ((s11s22 < 0) && (s12s22 > 0)) || ((s12s21 == 0) && (set1.limit2 || set2.limit1))
                 || ((s11s22 == 0) && (set1.limit1 || set2.limit2))) // ^
            {
                res_out = setIntersection;
                if (((s12s21 == 0) && (set1.limit2 || set2.limit1)) || ((s11s21 < 0) && (s12s21 > 0))) res_set = (!set1.limit1 ? "(" : "") + set1.str1 + ".." + set2.str2 + (!set2.limit2 ? ")" : "") + ";";
                else if (((s11s22 == 0) && (set1.limit1 || set2.limit2)) || ((s11s22 < 0) && (s12s22 > 0))) res_set = (!set2.limit1 ? "(" : "") + set2.str1 + ".." + set1.str2 + (!set1.limit2 ? ")" : "") + ";";
            }
            else // *
            {
                res_out = setUnion;
                res_set = set1.set + ";" + set2.set;
            }
        }
        #endregion
        #region пересечение
        else if (oper == setIntersection)
        {
            if ((s11s21 == 0) && (s12s22 == 0)) // =
            {
                res_out = setEqual;
                res_set += (((!set1.limit1) || (!set2.limit1)) ? "(" : "") + set1.str1 + ".." + set1.str2 + (((!set1.limit2) || (!set2.limit2)) ? ")" : "") + ";";
            }
            else if ((s11s21 <= 0) && (s12s22 >= 0)) // >
            {
                res_out = setMergeAB;
                res_set += ((((s11s21 < 0) && (!set2.limit1)) || ((s11s21 == 0) && (!set1.limit1 || !set2.limit1))) ? "(" : "") + set2.str1 + ".." + set2.str2 + ((((s12s22 > 0) && (!set2.limit2)) || ((s12s22 == 0) && (!set1.limit2 || !set2.limit2))) ? ")" : "") + ";";
            }
            else if ((s11s21 >= 0) && (s12s22 <= 0)) // <
            {
                res_out = setMergeBA;
                res_set += ((((s11s21 > 0) && (!set1.limit1)) || ((s11s21 == 0) && (!set1.limit1 || !set2.limit1))) ? "(" : "") + set1.str1 + ".." + set1.str2 + ((((s12s22 < 0) && (!set1.limit2)) || ((s12s22 == 0) && (!set1.limit2 || !set2.limit2))) ? ")" : "") + ";";
            }
            else if (((s11s21 < 0) && (s12s21 > 0)) || ((s11s22 < 0) && (s12s22 > 0)) || ((s12s21 == 0) && set1.limit2 && set2.limit1)
                 || ((s11s22 == 0) && set1.limit1 && set2.limit2)) // ^
            {
                res_out = setIntersection;
                if ((s12s21 == 0) && set1.limit2 && set2.limit1) res_set = set1.str2 + ";";
                else if ((s11s22 == 0) && set1.limit1 && set2.limit2) res_set = set1.str1 + ";";
                else
                {
                    if (s11s21 > 0) val1 = (!set1.limit1 ? "(" : "") + set1.str1;
                    else if (s11s21 == 0) val1 = ((!set1.limit1 || !set2.limit1) ? "(" : "") + set1.str1;
                    else val1 = (!set2.limit1 ? "(" : "") + set2.str1;

                    if (s12s22 > 0) val2 = set2.str2 + (!set2.limit2 ? ")" : "");
                    else if (s12s22 == 0) val2 = set1.str2 + ((!set1.limit2 || !set2.limit2) ? ")" : "");
                    else val2 = set1.str2 + (!set1.limit2 ? ")" : "");

                    res_set = val1 + ".." + val2+";";
                }
            }
            else // *
            {
                res_out = setUnion;
                res_set = "";
            }
        }
        #endregion
        #region пустоты
        else if (oper == setEmpties)
        {
            //string resout;
            //setStruct set_inf = new setStruct();
            //set_inf.limit1 = true;
            //set_inf.limit2 = true;
            //set_inf.value_type = TypeString;
            //set_inf.set = setNegInfinity + ".." + setInfinity;
            //set_inf.str1 = setNegInfinity;
            //set_inf.str2 = setInfinity;
            //res_set = setsCompare(set_inf, set1, cc.setRComplement, out resout)+";";
            //res_set += setsCompare(set_inf, set2, cc.setRComplement, out resout);
            //Console.WriteLine("res \"{0}\" ", res_set);
            //res_out = resout;

            if ((s11s21 == 0) && (s12s22 == 0)) // =
            {
                res_out = setEqual;
                res_set = ";";
            }
            else if ((s11s21 <= 0) && (s12s22 >= 0)) // >
            {
                res_out = setMergeAB;
                res_set = ";";
            }
            else if ((s11s21 >= 0) && (s12s22 <= 0)) // <
            {
                res_out = setMergeBA;
                res_set = ";";
            }
            else if (((s11s21 <= 0) && (s12s21 >= 0)) || ((s11s22 <= 0) && (s12s22 >= 0))) // ^
            {
                res_out = setIntersection;
                res_set = ";";
                if ((s12s21 == 0) && !set1.limit2 && !set2.limit1) res_set = set1.str2+ ";";
                else if ((s11s22 == 0) && !set1.limit1 && !set2.limit2) res_set = set1.str1 + ";";
            }
            else // *
            {
                res_out = setUnion;
                if (s11s22 > 0) res_set = ((set2.limit2) ? "(" : "") + set2.str2 + ".." + set1.str1 + ((set1.limit1) ? ")" : "") + ";";
                else res_set = ((set1.limit2) ? "(" : "") + set1.str2 + ".." + set2.str1 + ((set2.limit1) ? ")" : "") + ";";
            }
            //Console.WriteLine("res \"{0}\" ", res_set);
        }
        #endregion
        #region разность
        else if (oper == setRComplement)
        {
            if ((s11s21 == 0) && (s12s22 == 0)) // =
            {
                res_out = setEqual;
                if ((set1.limit1) && (!set2.limit1)) res_set += set1.str1 + ";";
                if ((set1.limit2) && (!set2.limit2)) res_set += set1.str2 + ";";
            }
            else if ((s11s21 <= 0) && (s12s22 >= 0)) // >
            {
                res_out = setMergeAB;
                if (s11s21 == 0)
                {
                    val1 = (set2.limit2 ? "(" : "") + set2.str2;
                    val2 = set1.str2 + (!set1.limit2 ? ")" : "");
                    if (set1.limit1 && !set2.limit1) res_set = set1.str1+";";
                    res_set += val1 + ".." + val2;
                }
                else if (s12s22 == 0)
                {
                    val1 = (!set1.limit1 ? "(" : "") + set1.str1;
                    val2 = set2.str1 + (set2.limit1 ? ")" : "");
                    if (set1.limit2 && !set2.limit2) res_set = set1.str2 + ";";
                    res_set += val1 + ".." + val2;
                }
                else
                {
                    val1 = (!set1.limit1 ? "(" : "") + set1.str1;
                    val2 = set2.str1 + (set2.limit1 ? ")" : "");
                    res_set = val1 + ".." + val2 + ";";

                    val1 = (set2.limit2 ? "(" : "") + set2.str2;
                    val2 = set1.str2 + (!set1.limit2 ? ")" : "");
                    res_set += val1 + ".." + val2 + ";";
                }
            }
            else if ((s11s21 >= 0) && (s12s22 <= 0)) // <
            {
                res_out = setMergeBA;
                if ((s11s21 == 0) && (set1.limit1) && (!set2.limit1)) res_set += set1.str1 + ";";
                if ((s12s22 == 0) && (set1.limit2) && (!set2.limit2)) res_set += set1.str2 + ";";
            }
            else if ((((s11s21 <= 0) && (s12s21 >= 0) && (s12s22 <= 0)) || ((s11s21 >= 0) && (s11s22 <= 0) && (s12s22 >= 0))) && !((s12s21 == 0) && !set1.limit2 && !set2.limit1)
                 && !((s11s22 == 0) && !set1.limit1 && !set2.limit2)) // ^
            {
                res_out = setIntersection;
                if ((s11s21 <= 0) && (s12s21 >= 0) && (s12s22 <= 0))
                {
                    val1 = (!set1.limit1 ? "(" : "") + set1.str1;
                    val2 = set2.str1 + (set2.limit1 ? ")" : "");
                    res_set = val1 + ".." + val2;
                }
                else
                {
                    val1 = (set2.limit2 ? "(" : "") + set2.str2;
                    val2 = set1.str2 + (!set1.limit2 ? ")" : "");
                    res_set = val1 + ".." + val2;
                }
            }
            else // *
            {
                res_out = setEmpties;
                res_set = set1.set;
            }
        }
        #endregion
        return res_set;
    }
    public static string setListToString(List<setStruct> set) // перевести список множеств в строку
    {
        string res_set = "";
        for (int i = 0; i < set.Count; i++) res_set += set[i].set + ";";
        return res_set;
    }
    public static List<setStruct> setStringToList(string set, int value_type = Int32.MinValue) // получает список отдельных множеств
    {
        List<setStruct> res_set = new List<setStruct>();

        string val1, val2, str;
        setStruct tmp_set;
        bool vt_fl = (value_type == Int32.MinValue) ? true : false ;
        int neg_pos;
        CultureInfo provider;

        foreach (string s in set.Split(';')) if (s.Trim(new char[] {' ', ';', ','}).Length > 0)
            {
                tmp_set = new setStruct(); // будущая структура множества
                str = s.Trim(); // строка для разбора множества
                tmp_set.set = str;

                neg_pos = str.IndexOf("!");
                tmp_set.isNeg = (neg_pos == 0) ? true : false; // признак отрицания
                if (neg_pos == 0) str = str.Substring(neg_pos + 1);

                int range_pos = str.IndexOf("..");
                if (range_pos > 0)
                {
                    val1 = str.Substring(0, range_pos).Trim();
                    val2 = str.Substring(range_pos + 2).Trim();

                    char br1 = val1[0];
                    char br2 = val2[val2.Length - 1];
                    tmp_set.limit1 = (br1 == '(') ? false : true;
                    tmp_set.limit2 = (br2 == ')') ? false : true;
                    if ((br1 == '(') || (br1 == '[')) val1 = val1.Substring(1).Trim();
                    if ((br2 == ')') || (br2 == ']')) val2 = val2.Substring(0, val2.Length - 1).Trim();
                    if (val1 == val2) tmp_set.set = val1;
                }
                else
                {
                    val1 = val2 = str;
                    tmp_set.limit1 = tmp_set.limit2 = true;
                }

                tmp_set.str1 = val1; tmp_set.str2 = val2;
                if (vt_fl) value_type = Math.Max(cc.StringType(val1, value_type), cc.StringType(val2, value_type));
                res_set.Add(tmp_set);
            }
        provider = new CultureInfo("en-US");
        value_type = ((value_type >= cc.TypeInt32) && (value_type <= cc.TypeDouble)) ? cc.TypeDouble : cc.TypeString;
        for (int i = 0; i < res_set.Count; i++)
        {
            tmp_set = res_set[i];
            tmp_set.value_type = value_type;
            /* if (value_type == cc.TypeInt32) { Int32.TryParse(tmp_set.str1, NumberStyles.Integer | NumberStyles.AllowDecimalPoint, provider, out tmp_set.int1); Int32.TryParse(tmp_set.str2.Replace('.', ','), out tmp_set.int2); }
            else if (value_type == cc.TypeInt64) { Int64.TryParse(tmp_set.str1, NumberStyles.Integer | NumberStyles.AllowDecimalPoint, provider, out tmp_set.long1); Int64.TryParse(tmp_set.str2.Replace('.', ','), out tmp_set.long2); } */
            if (value_type == cc.TypeDouble) { double.TryParse(tmp_set.str1.Replace('.',','), out tmp_set.f1); double.TryParse(tmp_set.str2.Replace('.', ','), out tmp_set.f2); }
            // else if (value_type == cc.TypeDateTime) { DateTime.TryParse(tmp_set.str1, out tmp_set.dt1); DateTime.TryParse(tmp_set.str2, out tmp_set.dt2); }
            res_set[i] = tmp_set;
        }

        return res_set;
    }

    public static List<paramsStruct> paramsStringToList(string pParams) // получает список параметров
    {
        List<paramsStruct> res = new List<paramsStruct>();
        if (String.IsNullOrEmpty(pParams)) return res;

        string str;
        int neg_pos, val_pos;
        paramsStruct tmp_params;
        string[] sp = pParams.Split('=');

        for (int i = 0; i < sp.Length-1; i++)
        {
            tmp_params = new paramsStruct();

            sp[i] = sp[i].Trim(); // получить имя
            str = Regex.Replace(sp[i], "[^\\w]", "#", 0);
            val_pos = str.LastIndexOf('#')+1;
            tmp_params.name = str.Substring((val_pos >= 0) ? val_pos : 0);

            sp[i + 1] = sp[i + 1].Trim(); // получить значение
            sp[i + 1] += (i == sp.Length - 2) ? ";" : "";
            str = Regex.Replace(sp[i+1], "[^\\w]", "#", 0);
            val_pos = str.LastIndexOf('#');
            str = sp[i + 1].Substring(0, (val_pos >= 0) ? val_pos : 0).Trim(new char[] { ';', ',', ' ' });
            neg_pos = str.IndexOf("!");
            tmp_params.isNeg = (neg_pos == 0) ? "1" : null; // признак отрицания
            if (neg_pos == 0) str = str.Substring(neg_pos + 1);
            str = str.Trim();
            if ((str == "") || (str == null)) tmp_params.value = null;
            else if (str == "''") tmp_params.value = "";
            else tmp_params.value = str.Trim();

            res.Add(tmp_params);
        }

        return res;
    }

    public static int setType(string set) // определить тип данных во множестве
    {
        int res_type = cc.TypeNone;
        set = set.Trim().TrimEnd(';');
        if (set == "") return cc.TypeNone;
        List<setStruct> sets = new List<setStruct>();

        string val1, val2;
        string str;
        int neg_pos;

        foreach (string s in set.Split(';')) if (s.Length > 0)
            {
                str = s.Trim(); // строка для разбора множества

                neg_pos = s.IndexOf("!");
                if (neg_pos == 0) str = str.Substring(neg_pos + 1);

                int range_pos = str.IndexOf("..");
                if (range_pos > 0)
                {
                    val1 = str.Substring(0, range_pos);
                    val2 = str.Substring(range_pos + 2);
                    char br1 = val1[0];
                    char br2 = val2[val2.Length - 1];
                    if ((br1 == '(') || (br1 == '[')) val1 = val1.Substring(1);
                    if ((br2 == ')') || (br2 == ']')) val2 = val2.Substring(0, val2.Length - 1);
                }
                else
                {
                    val1 = val2 = str;
                }

                res_type = Math.Max(cc.StringType(val1, res_type), cc.StringType(val2, res_type));
            }
        //Console.WriteLine(" 1=1 {0} ", res_type.ToString());
        return res_type;
    }

    public static bool IsFolder(string path)
    {
        if (path.IndexOf('.') >= 0) return false;
        try
        {
            string str = Path.GetFullPath(path);
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
        //return ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory);
    }

    public static bool IsFile(string path)
    {
        //Regex r = new Regex(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");
        //Regex r = new Regex("^([a-zA-Z]\\:)(\\\\[^\\\\/:*?<>\"|]*(?<![ ]))*(\\.[a-zA-Z]{2,6})$");
        if (path.LastIndexOf('.') < 0) return false;
        if (Regex.IsMatch(path, @"^(?:[a-zA-Z]\:|\\\\[\w\.]+\\[\w.$]+)\\(?:[\w]+\\)*\w([\w.])+$")) return true;
        else if (Regex.IsMatch(path, @"(?:[\w]+\\)*\w([\w.])+$")) return true;
        return false;
        //string str = null;
        //try
        //{
        //    str = Path.GetFileName(path);
        //}
        //catch (Exception e)
        //{
        //    return false;
        //}
        //if (!String.IsNullOrEmpty(str)) return true;
        //else return false;
    }

    public static string GetRelativePath(string src, string dist)
    {
        string result = "", str1, str2;
        str1 = src.Replace('/', '\\').Trim(new char[] { '\\', ' ' }).ToUpper();
        str2 = dist.Replace('/', '\\').Trim(new char[] { '\\', ' ' }).ToUpper();
        int i, j;
        for (i = 0; i < str1.Length; i++) if ((str1[i] != str2[i]) || (i == str2.Length)) break;
        if (i < str2.Length)
        {
            for (j = i; j < str2.Length; j++) if (str2[j] == '\\') result += "..\\";
            if (str2.IndexOf(".") < 0) result += "..\\";
        }
        else result = ".\\";
        result += src.Substring(str1.Substring(0, i).LastIndexOf('\\') + 1);

        return result;
    }

}

