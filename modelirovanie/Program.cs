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
            ConveyorSystem conveyorSystem = new ConveyorSystem(5,0, 4,120); // количество станков, места в буфере, интенсивность прихода заявок, время симуляции прихода заявок в минутах

            
            conveyorSystem.Simulate();
        }
    }
}
