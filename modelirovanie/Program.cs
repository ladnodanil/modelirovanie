using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelirovanie
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<(double, double, int, int, double, double)> stat = new List<(double, double, int, int, double, double)>();

            for (int i = 1; i <= 49; i++)
            {
                

                    ConveyorSystem conveyorSystem = new ConveyorSystem(3, 5, 4, 240); // количество станков, места в буфере, интенсивность прихода заявок, время симуляции прихода заявок в минутах
                    stat.Add(conveyorSystem.Simulate());
                
            }

            //stat.Sort();
            //double stats = 0;
            //foreach (var item in stat)
            //{
            //    if (item.Item1 >= 0)
            //    {
            //        //Console.WriteLine($"Количество станоков {item.Item3}, размер буфера {item.Item4}");
            //        //Console.WriteLine($"Вероятность обслуживания {item.Item1:F2} , Время пребывания {item.Item2:F0}  , Время ожидание в буфере {item.Item5:F0}");
            //        //Console.WriteLine($" {item.Item6:F2}");
            //        //stats += item.Item6;
            //        //Console.WriteLine($"{item.Item1:F2}  {item.Item2:F0}  {item.Item5:F0}  {item.Item6:F2}");
            //    }
            //}
            Console.WriteLine($"Среднее время ожидания в буфере: {stat.Average(item => item.Item5)}");
            Console.WriteLine($"Среднее время пребывания: {stat.Average(item => item.Item2)}");
            Console.WriteLine($"Средняя вероятность обслуживания: {stat.Average(item => item.Item1)}");
            
            Console.WriteLine($"Средняя вероятность загрузки станка: {stat.Average(item => item.Item6)}");

            Console.ReadLine();


            //conveyorSystem.Simulate();
        }
    }
}
