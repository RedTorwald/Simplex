using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Simp
    {
        double[,] chart; 
        int rows, columns;
        List<int> basis; 
        public Simp(double[,] input)
        {
            rows = input.GetLength(0);
            columns = input.GetLength(1);           
            chart = new double[rows, columns + rows - 1];                     //расширенная таблица для заполнения
            basis = new List<int>();                                         //лист для записи номеров столбцов с базисными переменными

            for (int i = 0; i < rows; i++)                                        
            {
                for (int j = 0; j < chart.GetLength(1); j++)
                {
                    if (j < columns)                                     //запись по размерности исходной таблицы
                        chart[i, j] = input[i, j];                      //перенос введённых элементов в расширенную таблицу
                    else
                        chart[i, j] = 0;                              //запись расширеенной части нулями
                }
                if ((columns + i) < chart.GetLength(1))             
                {     
                    chart[i, columns + i] = 1;                     //записываем единичную диагональ
                    basis.Add(columns + i);                       //запоминаем номер столбца, содержащего базисную переменную 
                }
            }
            columns = chart.GetLength(1);                      //обновляем значение размерности новой матрицы
        }

        public double[,] Logic(double[] result, double[] a)
        {
            int leadCol, leadRow;                          //переменные для хранения индексов ведущего элемента
            int itter = 0;                                //счётчик итеррации
            while (!Finish())                            //пока есть отрицательные элементы в строке симплекс разностей
            {
                leadCol = findLeadCol();    
                leadRow = findLeadRow(leadCol);
                basis[leadRow] = leadCol;              //запись элемента по индексу

                double[,] new_table = new double[rows, columns]; //таблица для вывода пересчитанной таблицы
                
                double F = 0; // целевая функция

                for (int j = 0; j < columns; j++)
                    new_table[leadRow, j] = Math.Round(chart[leadRow, j] / chart[leadRow, leadCol],3);       //пересчёт ведущей строки путём деления на ведущий элемент               
                if (itter == 0) {                                                                           //поэлементная запись нулевой иттерации в новую таблицу
                    Console.WriteLine("\n"+"Итерация - " + itter);
                    for (int i = 0; i < new_table.GetLength(0); i++)
                    {
                        for (int j = 0; j < new_table.GetLength(1); j++)
                            Console.Write(chart[i, j] + "\t  ");                            
                        Console.WriteLine();
                    }
                    itter++;
                }
                for (int i = 0; i < rows; i++)                                   //пересчёт остальной таблицы
                {
                    if (i == leadRow)                                           // пропуск пересчитанной строки (ведущей)
                        continue;

                    for (int j = 0; j < columns; j++)                          
                        new_table[i, j] = Math.Round(chart[i, j] - chart[i, leadCol] * new_table[leadRow, j],3);  //пересчёт остальной таблицы по правилу прямоугольника
                }
                chart = new_table;                                                                              //освобождение новой таблицы (запись решённой таблицы в старую)
                Console.WriteLine("\n" + "Итерация - " + itter);                                                                                               
                for (int i = 0; i < chart.GetLength(0); i++)
                {
                    for (int j = 0; j < chart.GetLength(1); j++)
                    {
                        Console.Write(chart[i, j] + "\t "); 
                    } 
                    Console.WriteLine();
                }
                for (int i = 0; i < result.Length; i++)                 
                {
                    int k = basis.IndexOf(i+1);      //поиск индекса базисной переменной 
                    if (k != -1)                    // при нахождении индекса в массиве
                        result[i] = chart[k, 0];   // записываем значение базисной переменной из таблицы (по индексу из нулевого столбца)
                    else
                        result[i] = 0;            // иначе нуль
                }
                for (int i = 0; i < result.Length; i++)
                {
                    Console.WriteLine("X[" + basis[i] + "] = " + result[i]);                //вывод значений базисных переменных
                }

                F = 0;
                for (int i = 1; i < result.Length+1; i++) { F += a[i] * result[i - 1]; } //подсчёт целевой функции путём умножения коэфициентов целевой функции на значения базисных переменных
                Console.WriteLine("F = " + F);
                Console.WriteLine();
                itter++;
            }  
            return chart;
        }       

        private int findLeadCol()
        {
            int leadCol = 1;
            for (int j = 2; j < columns; j++)
                if (chart[rows - 1, j] < chart[rows - 1, leadCol])  //поиск минимальной значения 
                    leadCol = j; 
            return leadCol;                                       // сохранение индекса столбца с минимальным значением (максимальным по модулю), ведущего столбца
        }
        private int findLeadRow(int mainCol)                   // передаём индекс ведущего столбца 
        { 
            int leadRow = 0;                                 // заводим перемекнную под хранение тндекс ведущей строки            
            for (int i = leadRow; i < rows - 1; i++)        //поиск минимального отношения (значение свободного члена/на соответсвующую ячейку ведущего столбца)
                if ((chart[i, mainCol] > 0) && ((chart[i, 0] / chart[i, mainCol]) < (chart[leadRow, 0] / chart[leadRow, mainCol]))) // поиск минимального элемента в столбце со значениями В
                    leadRow = i;                                 
            return leadRow;
        } 
        
         private bool Finish()                   //условие окончания поиска поиска симплекс разности
         {
            bool f = true;
            for (int j = 1; j < columns; j++)
            {                              //поиск отрицательной симплекс разности
                if (chart[rows - 1, j] < 0){f = false; break;}
            }
            return f;
         }
    }       
}