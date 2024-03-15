using System;
using System.Text.RegularExpressions;

namespace mycalcul
{
    internal class Double
    {
        public double num;
        public bool flag;
        public Double(double n, bool f) 
        {
            num = n;
            flag = f;
        }
    }

    internal class BKT
    {
        public BKT() { }
        Stack<int> stack = new Stack<int>(10);
        uint stackSize = 0;

        private void Trigon (ref string str)
        {
            while (true)
            {
                Regex regex = new Regex(@"sin|cos|tan|ctg");
                Match match = regex.Match(str);
                if (!match.Success) { break; }
                int ind = match.Index + 4;
                for (int i = ind; i < str.Length; ++i)
                {
                    if (str[i] == ')') 
                    {
                        double answer = TrigAref(match.Value, Convert.ToDouble(str.Substring(ind, i - ind)));
                        str = str.Remove(match.Index, i - match.Index + 1);
                        str = str.Insert(match.Index, answer.ToString());
                        break;
                    }
                }
            }
        }

        public double TrigAref(string str, double a)
        {
            double ans = 0;
            switch (str)
            {
                case "sin":
                    ans = Math.Sin(a);
                    break;
                case "cos":
                    ans = Math.Cos(a);
                    break;
                case "tan":
                    ans = Math.Tan(a);
                    break;
                case "ctg":
                    ans = 1/(Math.Tan(a));
                    break;
            }
            return ans;
        }

        private void Pow(ref string str)
        {
            for (int i = 0; i < str.Length; ++i)
            {
                if (str[i] == '^')
                {
                    double degree = 0, num = 0;
                    int ind_1 = 0, ind_2 = 0;
                    for (int j = i; j != -1; --j) 
                    {
                       if (j == 0 || "+-×÷(".Contains(str[j - 1]))
                       {
                            num = Convert.ToDouble(str.Substring(j, i - j));
                            ind_1 = j;
                            break;
                       }
                    }
                    for (int j = i; j != str.Length; ++j)
                    {
                        if (j == str.Length - 1 || "+-×÷)".Contains(str[j + 1]))
                        {
                            degree = Convert.ToDouble(str.Substring(i + 1, j - i));
                            ind_2 = j;
                            break;
                        }
                    }
                    str = str.Remove(ind_1, ind_2 - ind_1 + 1);
                    str = str.Insert(ind_1, Math.Pow(num, degree).ToString());
                }
            }
        }

        public string BKT_sol(string str)
        {
            /*if (Check.Trig == true)
            {*/
                Trigon(ref str);
            /*}
            if (Check.Pow == true)
            {*/
                Pow(ref str);
            //}

            int bkt_L = 0;
            while (true)
            {
                bkt_L = str.IndexOf("(", bkt_L);
                if (bkt_L == -1) { break; }
                stack.Push(bkt_L);
                ++bkt_L; ++stackSize;
            }
            BKT_help_sol(ref str);
            stack.Clear();

            return Solution(str);
        }

        private void BKT_help_sol(ref string str)
        {
            int bkt_L;
            for (int i = 0; i < stackSize; ++i)
            {
                bkt_L = stack.Pop();
                int indx = str.IndexOf(")", bkt_L + 1) - bkt_L;
                string s = Solution(str.Substring(bkt_L + 1, indx - 1));
                str = str.Remove(bkt_L, indx + 1);
                str = str.Insert(bkt_L, s);
            }
        }

        private string Solution(string str)
        {
            if (str.Length == 1 || str.Length == 0)
            {
                return str;
            }
            Stack<double> Stack_d = new Stack<double>(21);
            Stack<Double> Stack_d_pri = new Stack<Double>(21);
            Stack<char> Stack_c = new Stack<char>(20);
            Stack<char> Stack_c_pri = new Stack<char>(20);

            Stack<double> Buffer = new Stack<double>(40);

            bool flag = true;
            int lenght = str.Length, sup = 0;
            int i = 0;
            if (str[0] ==  '-') { ++i; }
            for (; i < lenght; ++i)
            {
                if ("+-×÷".Contains(str[i]) || i == lenght - 1)
                {
                    if ((str[i - 1] == '×' || str[i - 1] == '÷' || str[i - 1] == '+') && str[i] == '-') { continue; }

                    if (str[i] == '+')
                    { 
                        Stack_c.Push(str[i]); 
                        Stack_d.Push(Convert.ToDouble(str.Substring(sup, i - sup)));
                        sup = i + 1;
                        if (Stack_d.Count() != 0 && flag == false)
                        {
                            Stack_d_pri.Push(new Double(Stack_d.Pop(), flag));
                        }
                        flag = true;
                        continue;
                    }

                    if (i == lenght - 1)
                    {
                        if (flag == true) { Stack_d.Push(Convert.ToDouble(str.Substring(sup, i - sup + 1))); }
                        else { Stack_d_pri.Push(new Double(Convert.ToDouble(str.Substring(sup, i - sup + 1)), false)); }
                        continue;
                    }
                    Stack_c_pri.Push(str[i]);
                    Stack_d_pri.Push(new Double(Convert.ToDouble(str.Substring(sup, i - sup)), flag));

                    sup = i + 1;
                    flag = false;
                }
            }

            while (Stack_d_pri.Count() != 0)
            {
                if (Stack_d_pri.Count() == 1)
                {
                    Buffer.Push(Stack_d_pri.Pop().num);
                    break;
                }

                Double Fir = Stack_d_pri.Pop(), Sec = Stack_d_pri.Pop();
                if ((Fir.flag||Sec.flag) == true)
                {
                    Buffer.Push(aref(Stack_c_pri.Pop(), Fir.num, Sec.num));
                }
                else
                {
                    Stack_d_pri.Push(new Double(aref(Stack_c_pri.Pop(), Fir.num, Sec.num), false));
                }
            }

            while (Buffer.Count() != 0)
            {
                Stack_d.Push(Buffer.Pop());
            }

            while (Stack_d.Count() != 1)
            {
                double Sec = Stack_d.Pop(), Fir = Stack_d.Pop();
                Stack_d.Push(aref(Stack_c.Pop(), Fir, Sec));
            }

            return (Stack_d.Pop()).ToString();
        }
        
        public double aref(char s, double r, double l)
        {
            double ans = 0;
            switch (s)
            {
                case '+':
                    ans = l + r;
                    break;
                case '×':
                    ans = l * r;
                    break;
                case '÷':
                    ans = l / r;
                    break;
            }
            return ans;
        }
    }
}