# 程序设计 demo 
#### C#开发
## 1 综述
> 代码由三部分构成 具体是
>> 1.构成图形界面的部分

>> 2.图形界面通信和执行逻辑的部分


## 2 图形界面
> 这部分由 两个 TextBox 和 七个 Bottom 构成

     输入 TextBox 与 用户的键盘和鼠标交互 
     输出 TextBox 与 执行逻辑的文本文件交互
>
     七个 Bottom 具体是 
     input delete rewrite visit number grade 和 save 
          
           input 有两组参数 
               第一组为 姓名 学号 成绩1 - 5
               第二组为 位置 姓名 学号 成绩1 - 5
           delete 参数为 位置
           rewrite 参数为 位置 姓名 学号 成绩1 - 5
           visit 参数为 位置（当第一次点击时访问具体数据，
                               第二次点击时访问全体数据）
           number 无参数 点击按学号排序
           grade  无参数 点击按总成绩排序
           save   # ————还在计划中
## 3 通讯层
> 与图形界面和下面的 Student 类进行通讯
#### 3.1 有七个通讯函数分别对应七个 Bottom 

      // visit 函数实现当第一次点击时访问具体 第二次点击时访问全体数据
          int count = 0;
               private void Button_visit_Click(object sender, RoutedEventArgs e)
               {
                    // 当点击一次的时侯查找一条信息 再次点击的时侯输出全部信息
                    if (count % 2 == 0)
                    {
                         string GetLine = TextBox_input.Text + ' ';
                         if (Student.IsNumberic(GetLine.Substring(0, GetLine.IndexOf(' '))) == true)
                         {
                              TextBox_output.Text = Student.StudentList_display
                              [Convert.ToInt16(GetLine.Substring(0, GetLine.IndexOf(' '))) - 1].ToString();
                              count++;
                         }
                    }
                    else
                    {
                         Write_Display_file();
                         count--;
                    }
               }            
         
#### 3.2 有三个配套的函数  联合 异常处理 读写文本

        // *****************   联合   *****************
        private void JoinFor_input_delete_rewirte(string str)
        {                                         // 传入 input delete rewrite
                                                  // 为实现 多态
             
            Student stu = new Student();
            stu.SetAll(TextBox_input.Text + ' ', str);
            GetError(stu.ToInform_WList(), str);
        }

        // ***************** 异常处理 *****************
                ////发生错误时返回错误提示代表的数字  
               /// 0 无错误
               /// 1 参数不足
               /// 2 重复输入
               /// 3 学号 需要变为数值的字符不可变
                    /// 31 语文
                    /// 32 数学
                    /// 33 英语
                    /// 34 物理
                    /// 35 化学
               /// 4 位置字符不可变
               /// 5 无参数                                  
        private void GetError(int number, string str)
        {                                 // 传入 input delete rewrite
            switch (number)
            {
                case 0: Write_Display_file(); break;
                case 1: if (str == "input") TextBox_output.Text = "   
                                   参数不足 " + str + "需要的参数为  (位置) 姓名 学号 成绩 1 - 5"; break;
                case 2: TextBox_output.Text = "                     该生已经输入"; break;
                case 3: TextBox_output.Text = "                     学号字符不可变为数值"; break;
                case 31: TextBox_output.Text = "                    语文字符不可变为数值"; break;
                case 32: TextBox_output.Text = "                    数学字符不可变为数值"; break;
                case 33: TextBox_output.Text = "                    英语字符不可变为数值"; break;
                case 34: TextBox_output.Text = "                    物理字符不可变为数值"; break;
                case 35: TextBox_output.Text = "                    化学字符不可变为数值"; break;
                case 4: TextBox_output.Text = "                     位置字符不可变为数值"; break;
                case 5: TextBox_output.Text = "                     无参数"; break;
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
        }
    }   
## 逻辑层
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
                                                       /// 31 语文
                                                       /// 32 数学
                                                       /// 33 英语
                                                       /// 34 物理
                                                       /// 35 化学
                                                       /// 4 位置字符不可变
                                                       /// 5 无参数

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
      // ************* 类数据 *******************
          <!-- 使用string链的
                         好处
                    1.空间复杂度低相对 student 链
                    2.算法上好操作 结构体链
                        坏处
                    1.对排序无力
                    2.保存显示时需要两个链 -->
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
     <!-- 省略了 set get 函数 -->
     public void SetPart(string GetLine)
     {
          <!-- 将 姓名 学号 成绩进行设置 并对 学号 成绩 是否能 
               转化为 数值进行判断 不能则SetApi( 31 - 35 )-->
          // 将 GetLine 的从 0 到 ' ' 的子串赋值给 Name
          SetName(GetLine.Substring(0, GetLine.IndexOf(' ')));

          // 将 GetLine 的从 ' ' + 1 到尾的字串 赋给 swift  
          string swift = GetLine.Substring(GetLine.IndexOf(' ') + 1);

          // 将 swift  的从 0 到 ' ' 的子串赋值给 Nunmer
          if (Student.IsNumberic(swift.Substring(0, swift.IndexOf(' '))) == true)
          {
               SetSNumber(swift.Substring(0, swift.IndexOf(' ')));
               SetNumber(Convert.ToUInt64(swift.Substring(0, swift.IndexOf(' '))));
          }
          else SetApi(3);

          // 将 swift  的从 0 到 ' ' 的子串赋值给 Grade
          for (int i = 0; i < 5; i++)
          {
               swift = swift.Substring(swift.IndexOf(' ') + 1);
               if (Student.IsNumberic(swift.Substring(0, swift.IndexOf(' '))) == true)
                    SetGrade(i, Convert.ToDouble(swift.Substring(0, swift.IndexOf(' '))));
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
          // 如果有效空格数为 7 则说明 为 input 函数,其无位置参数
          else if (GetNumberOfSpace() == 7 )
               SetPart(GetLine);
          // 如果有效空格数为 0 => SetApi(5)
          else if (GetNumberOfSpace() == 0)
               SetApi(5);
          // 如果有效空格数为 1 && 为 delete  则进行删除
          else if (GetNumberOfSpace() == 1 && str_fun == "delete")
          {
               if (Student.IsNumberic(GetLine.Substring(0, GetLine.IndexOf(' '))) == true)
               {
                    int pos = Convert.ToInt32(GetLine.Substring(0, GetLine.IndexOf(' '))) - 1;
                    Student.StudentList_display.RemoveAt(pos);
               }
               else SetApi(4);
          }
          else// 有效空格数为 8 可以是 input 和 rewrite 两个函数
          {
               if (Student.IsNumberic(GetLine.Substring(0, GetLine.IndexOf(' '))) == true)
               {
                    SetPosition(Convert.ToInt32(GetLine.Substring(0, GetLine.IndexOf(' '))) - 1);
                    SetPart(GetLine.Substring(GetLine.IndexOf(' ') + 1));
                    if (str_fun == "rewirte")
                         Student.StudentList_display.RemoveAt(GetPosition());
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
     // 为使程序在调试时减少报错
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
    
     }

     