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
            List<(double, double,int,int,double)> stat = new List<(double, double, int, int,double)>();

            for (int i = 1; i <= 6; i++)
            {
                for (int j = 0; j <= 5; j++)
                {

                    ConveyorSystem conveyorSystem = new ConveyorSystem(4, 3, 4, 240); // количество станков, места в буфере, интенсивность прихода заявок, время симуляции прихода заявок в минутах
                    stat.Add(conveyorSystem.Simulate());
                }
            }
            stat.Sort();
            foreach (var item in stat)
            {
                if(item.Item1 >= 0.75)
                {
                Console.WriteLine($"Количество станоков {item.Item3}, размер буфера {item.Item4}");
                Console.WriteLine($"{item.Item1:F2}  {item.Item2:F0}");

                }
            }

            Console.ReadLine();


            //conveyorSystem.Simulate();
        }
    }
}
