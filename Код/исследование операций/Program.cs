namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {              
            Console.WriteLine("Введите количество переменных (столбцов):");           //ввод количества столбцов      
            int n = Int32.Parse(Console.ReadLine());
            Console.WriteLine("Введите количество ограничений (строк):");           //ввод количества строк
            int m = Int32.Parse(Console.ReadLine());

            Console.WriteLine("Введите коэффициенты целевой функции");
            double[,] table1= new double[m+1,n+1];                              // массив с размерностью для записи столбца свободных членов и симплекс разностей
            double[] a = new double[n+1];                                      // одномерный массив для записи коэффициентов целевой функции                                                    
            for (int i = 1; i < n + 1; i++)
            {
                Console.WriteLine("Введите коэфициент целевой функции перед x" + i);  
                table1[m, i] = double.Parse(Console.ReadLine()) * (-1);               // заполнение таблицы(заносим коэффициенты в начальную таблицу)
                a[i] = table1[m, i]*(-1);                                            // заполнение коэффициентов целевой функции (с отрицательным знаком)
            }
            Console.WriteLine("Введите ограничения");                                 
            for (int i = 0; i < m; i++)
            {
                for(int j = 1; j < n + 1; j++)
                {
                    Console.WriteLine("Введите коэфициент перед x"+(i+1)+j);         
                    table1[i,j] = double.Parse(Console.ReadLine());              // заполнение таблицы элементами (коэффициенты при ограничениях)
                }
               
                Console.WriteLine("Введите значение b"+(i+1));
                table1[i, 0] = double.Parse(Console.ReadLine());              // заполнение массива ограничений 
            } 

            double[] result = new double[a.Length-1];                       //массив для хранения значений базисных переменных               
            double[,] table_result;                                        //таблица для хранениня результатов подсчётов из Logic
            Simp Simplex = new Simp(table1);                              //создаём объект Simplex 
            table_result = Simplex.Logic(result,a);                      //передаём значения базисных переменных и коэффициенты целевой функции в Logic

            Console.WriteLine("Вывод таблицы:");                       //поэлементый вывод решённой таблицы
            for (int i = 0; i < table_result.GetLength(0); i++)
            {
                for (int j = 0; j < table_result.GetLength(1); j++)
                    Console.Write(table_result[i, j] + "\t");                  
                Console.WriteLine();
            }

           
            for(int i = 0; i < result.Length; i++)                       
            {
                Console.WriteLine("X["+(i+1)+"] = " + result[i]);  //вывод значений базисных переменных
            }            
            double F = 0;
            for (int i = 1; i < result.Length+1; i++) { F += a[i] * result[i-1]; } //вывод целевой функции
            Console.WriteLine("F ="+F);
        }
    }
}