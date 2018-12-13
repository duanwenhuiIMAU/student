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
using System.IO;

namespace student
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// A 0001 4 6 7 1 3
    //B 0002 5 4 3 2 1
    //C 0003 2 3 4 5 6
    //E 0004 3 4 5 9 6
    //F 0005 4 5 3 5 3
    //G 0006 8 9 4 7 9
    //H 0007 3 4 3 5 1 
    //I 0008 3 4 5 6 8
    //J 0009 3 4 5 3 5 
    //K 00010 3 4 5 5 7
    //E 000w 3 4 5 5 6
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /*
        *              ***********     通讯层   ************
        */

        private void Button_input_Click(object sender, RoutedEventArgs e)
        {
            JoinFor_input_delete_rewirte("input");
        }

        private void Button_delete_Click(object sender, RoutedEventArgs e)
        {
            JoinFor_input_delete_rewirte("delete");
        }

        private void Button_rewirte_Click(object sender, RoutedEventArgs e)
        {
            JoinFor_input_delete_rewirte("rewirte");
        }

        int count = 0;
        private void Button_visit_Click(object sender, RoutedEventArgs e)
        {
            // 当点击一次的时侯查找一条信息 再次点击的时侯输出全部信息
            if (count % 2 == 0)
            {
                string GetLine = TextBox_input.Text + ' ';
                if (Student.IsNumberic(GetLine.Substring(0, GetLine.IndexOf(' '))) == true)
                {
                    TextBox_output.Text = Student.StudentList_display[Convert.ToInt16(GetLine.Substring(0, GetLine.IndexOf(' '))) - 1].ToString();
                    count++;
                }
            }
            else
            {
                Write_Display_file();
                count--;
            }
        }

        private void Button_rannkByNumber_Click(object sender, RoutedEventArgs e)
        {/*
            string connectionString = "user id=sa;password = ;initialcatlog=northwind;datasource=localhost;";
            SqlConnection conn = new SqlConnection();*/
        }

        private void Button_rannkByGrade_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Save_Init_Click(object sender, RoutedEventArgs e)
        {

        }

        // *****************   联合   *****************
        private void JoinFor_input_delete_rewirte(string str)
        {                                          // 传入 input delete rewrite 为实现 多态
            Student stu = new Student();
            stu.SetAll(TextBox_input.Text + ' ', str);
            GetError(stu.ToInform_WList(), str);
        }

        // ***************** 异常处理 *****************
        private void GetError(int number, string str)
        {
            switch (number)
            {
                case 0: Write_Display_file(); break;
                case 1: if (str == "input") TextBox_output.Text = "                      参数不足 " + str + "需要的参数为  (位置) 姓名 学号 成绩 1 - 5"; break;
                case 2: TextBox_output.Text = "                     该生已经输入"; break;
                case 3: TextBox_output.Text = "                     学号字符不可变为数值"; break;
                case 31: TextBox_output.Text = "                    语文字符不可变为数值"; break;
                case 32: TextBox_output.Text = "                    数学字符不可变为数值"; break;
                case 33: TextBox_output.Text = "                    英语字符不可变为数值"; break;
                case 34: TextBox_output.Text = "                    物理字符不可变为数值"; break;
                case 35: TextBox_output.Text = "                    化学字符不可变为数值"; break;
                case 4: TextBox_output.Text = "                     位置字符不可变为数值"; break;
                case 5: TextBox_output.Text = "                     无参数"; break;
                case 6: TextBox_output.Text = "                     不可为负数"; break;
                case 61: TextBox_output.Text = "                     语文不可为负数"; break;
                case 62: TextBox_output.Text = "                     数学不可为负数"; break;
                case 63: TextBox_output.Text = "                     英语不可为负数"; break;
                case 64: TextBox_output.Text = "                     物理不可为负数"; break;
                case 65: TextBox_output.Text = "                     化学不可为负数"; break;
                case 7: TextBox_output.Text = "                      链表中无数据"; break;
                case 8: TextBox_output.Text = "                      链表中无数据 只是进行了添加"; break;
            }
        }

        // ***************** 读写文本 *****************
        private void Write_Display_file()
        {
            // 从 MyStuentList 写入文本      文件写入 result.txt 并关闭
            FileStream resultFile = new FileStream("display.txt", FileMode.Create);
            StreamWriter writerFile = new StreamWriter(resultFile);
            for (int i = 0; i < Student.StudentList_display.Count; i++)
            {
                int pos = i + 1;
                string str = Student.StudentList_display[i] + " 位置  ：" + pos + '\n';
                writerFile.Write(str);
            }
            writerFile.Close();
            // 从 文本中读出 MyStuentList       从 result.txt 读出全部到 output 并关闭
            StreamReader readerFile = new StreamReader("display.txt");
            TextBox_output.Text = readerFile.ReadToEnd();
            readerFile.Close();
            //Student student = new Student();
            //student.GetHashCode；
        }
    }
}

/*
 *              ***********     逻辑层   ************
 */
public class Student
{

    //    *********   数据域   ***********
    string Name { set; get; }                         //姓名
    UInt64 Number { set; get; }                       //学号（数值）
    string SNumber { set; get; }                      //学号（字符）
    double[] Grade { set; get; } = new double[5];     //成绩
    double Sum { set; get; }                          //成绩的和
    double Average { set; get; }                      //和的平均值
    UInt16 NumberOfSpace { set; get; }                //检查字符串中共多少空格  在 input 时空格数为  7 其他需要 8 个
    int Position { set; get; }                     //在插入和改写时要改写的位置
    int Api { set; get; }                          ////发生错误时返回错误提示代表的数字  
                                                   /// 0 无错误
                                                   /// 1 参数不足
                                                   /// 2 重复输入
                                                   /// 3 学号 需要变为数值的字符不可变                                             
                                                   /// 4 位置字符不可变
                                                   /// 5 无参数
                                                   /// 6 不可为负数
                                                   /// 7 链表中无数据
                                                   /// 8 链表中无数据 只是进行了添加

    //     *********  对象函数  ***********
    public Student()
    {
        Name = null;
        Number = 0;
        SNumber = null;
        Grade[0] = 0;
        Grade[1] = 0;
        Grade[2] = 0;
        Grade[3] = 0;
        Grade[4] = 0;
        Sum = 0;
        Average = 0;
        NumberOfSpace = 0;
        Position = 0;
        Api = 0;
    }

    public void SetName(string n)
    {
        Name = String.Concat(n);
    }
    public string GetName()
    {
        return Name;
    }

    public void SetNumber(UInt64 n)
    {
        Number = n;
    }
    public UInt64 GetNumber()
    {
        return Number;
    }
    public void SetSNumber(string str)
    {
        SNumber = str;
    }
    public string GetSNumber()
    {
        return SNumber;
    }

    public void SetGrade(int i, double gre)
    {
        Grade[i] = gre;
    }
    public double GetGrade(int i)
    {
        return Grade[i];
    }

    public void Set_Sum_Average()
    {
        Sum = Grade[0] + Grade[1] + Grade[2] + Grade[3] + Grade[4];
        Average = Sum / 5;
    }
    public double GetSum()
    {
        return Sum;
    }
    public double GetAverage()
    {
        return Average;
    }

    public UInt16 GetNumberOfSpace()
    {
        return NumberOfSpace;
    }

    public void SetPosition(int pos)
    {
        Position = pos;
    }
    public int GetPosition()
    {
        return Position;
    }

    public void SetApi(int a)
    {
        Api = a;
    }
    public int GetApi()
    {
        return Api;
    }

    public void SetPart(string GetLine)
    {
        // 将 GetLine 的从 0 到 ' ' 的子串赋值给 Name
        SetName(GetLine.Substring(0, GetLine.IndexOf(' ')));

        // 将 GetLine 的从 ' ' + 1 到尾的字串 赋给 swift  
        string swift = GetLine.Substring(GetLine.IndexOf(' ') + 1);

        // 将 swift  的从 0 到 ' ' 的子串赋值给 Nunmer
        if (Student.IsNumberic(swift.Substring(0, swift.IndexOf(' '))) == true)
        {
            if(Convert.ToUInt64(swift.Substring(0, swift.IndexOf(' ')))>=0)
            {
                SetSNumber(swift.Substring(0, swift.IndexOf(' ')));
                SetNumber(Convert.ToUInt64(swift.Substring(0, swift.IndexOf(' '))));
            }
            else SetApi(6);
        }
        else SetApi(3);

        // 将 swift  的从 0 到 ' ' 的子串赋值给 Grade
        for (int i = 0; i < 5; i++)
        {
            swift = swift.Substring(swift.IndexOf(' ') + 1);
            if (Student.IsNumberic(swift.Substring(0, swift.IndexOf(' '))) == true)
            {
                if (Convert.ToDouble(swift.Substring(0, swift.IndexOf(' '))) >= 0)
                    SetGrade(i, Convert.ToDouble(swift.Substring(0, swift.IndexOf(' '))));
                else
                {
                    int h = 61 + i;
                    SetApi(h);
                    break;
                }
            }
            else
            {
                int h = 31 + i;
                SetApi(h);
                break;
            }
        }
        Set_Sum_Average();

    }
    public void SetAll(string GetLine, string str_fun)
    {
        for (int i = 1; i < GetLine.Length; i++)
        {
            // 计算有效空格数
            if (GetLine[i] == ' ' && GetLine[i - 1] != ' ')
                NumberOfSpace++;
        }
        // 如果有效空格数在 1-7之间 => SetApi(1) 
        if (GetNumberOfSpace() < 7 && GetNumberOfSpace() > 1)
            SetApi(1);
        else if (GetNumberOfSpace() == 7)
            SetPart(GetLine);
        else if (GetNumberOfSpace() == 0)
            SetApi(5);
        else if (GetNumberOfSpace() == 1 && str_fun == "delete")
        {
            if (Student.IsNumberic(GetLine.Substring(0, GetLine.IndexOf(' '))) == true)
            {
                if (Student.StudentList_display.Count!=0) {
                    int pos = Convert.ToInt32(GetLine.Substring(0, GetLine.IndexOf(' '))) - 1;
                    Student.StudentList_display.RemoveAt(pos);
                }
                else SetApi(7);
            }
            else SetApi(4);
        }
        else
        {
            if (Student.IsNumberic(GetLine.Substring(0, GetLine.IndexOf(' '))) == true)
            {
                SetPosition(Convert.ToInt32(GetLine.Substring(0, GetLine.IndexOf(' '))) - 1);
                SetPart(GetLine.Substring(GetLine.IndexOf(' ') + 1));
                if (str_fun == "rewirte")
                {
                    if (Student.StudentList_display.Count != 0)
                        Student.StudentList_display.RemoveAt(GetPosition());
                    else SetApi(8);
                }                  
            }
            else
                SetApi(4);
        }
    }

    public string Display()
    {
        return "姓名 ：" + GetName() + "  学号 ： " + GetSNumber() +
                "  总成绩 :" + GetSum() + "   平均值 ：" + GetAverage() + "   语文  " + GetGrade(0) + "   数学  " + GetGrade(1)
               + "   英语  " + GetGrade(2) + "   物理  " + GetGrade(3) + "   化学  " + GetGrade(4);
    }
    public string ToSave_Init()
    {
        return GetName() + ' ' + GetSNumber() + ' ' + GetGrade(0) + ' ' + GetGrade(1) + ' ' + GetGrade(2) + ' ' + GetGrade(3) + ' ' + GetGrade(4) + '\n';
    }

    public int ToInform_WList()
    {
        // 判断 对象的 Display() 是否能加入链表 能则加入 不能输出不能的信息
        if (GetApi() == 0)
        {
            if (GetNumberOfSpace() == 7)
            {
                if (!StudentList_display.Contains(Display()))
                {
                    StudentList_display.Add(Display());
                    return GetApi();
                }
                else
                {
                    SetApi(2);
                    return GetApi();
                }
            }
            else if (GetNumberOfSpace() == 8)
            {
                if (!StudentList_display.Contains(Display()))
                {
                    StudentList_display.Insert(GetPosition(), Display());
                    return GetApi();
                }
                else
                {
                    SetApi(2);
                    return GetApi();
                }
            }
            else return GetApi();// 该控件永远用不上
        }
        else return GetApi();
    }
    // ************* 类数据 *******************
    public static List<string> StudentList_display = new List<string>();
    // ************* 类函数 ********************
    
    public static bool IsNumberic(string str)
    {
        // 判断一个字符是否能转化为 double
        double vsNum;
        bool isNum;
        isNum = double.TryParse(str, System.Globalization.NumberStyles.Float,
            System.Globalization.NumberFormatInfo.InvariantInfo, out vsNum);
        return isNum;
    }
}
    