using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace modelirovanie
{
    public class Buffer
    {

        public double T_osv { get; set; } // время когда станок будет освобожден

        public Buffer()
        {
            T_osv = 0;
        }

    }

    public class Machine // класс станка
    {
        public int Index { get; set; }

        public int ServicedDetail { get; set; } // количество обслуженных деталей
        public double T_osv { get; set; } // время когда станок будет освобожден

        public Buffer[] Buffers { get; set; } // массив буферов


        public Machine(int index, int sizeBuffer)
        {
            Index = index;
            T_osv = 0;
            ServicedDetail = 0;
            Buffers = new Buffer[sizeBuffer];
            for (int i = 0; i < Buffers.Length; i++)
            {
                Buffers[i] = new Buffer();
            }

        }
    }

    public class ConveyorSystem
    {

        public int Lyamda { get; set; } // интенчивность прихода заявки

        public int SimulationTime { get; set; } // время симуляции прихода заявок (в минутах)


        public Machine[] Machines { get; set; } // станки



        public ConveyorSystem(int diveceCount, int sizeBuffer, int lyamda, int simulationTime)
        {
            this.Lyamda = lyamda;
            this.SimulationTime = simulationTime;

            Machines = new Machine[diveceCount];

            for (int i = 0; i < Machines.Length; i++)
            {
                Machines[i] = new Machine(i + 1, sizeBuffer);

            }

        }

        public void Simulate()
        {

            double t_prich = 0; // время прихода

            double t_osv = 0; // время освобождения заявки

            int k = 0; // счетчик деталей

            int N_obs = 0; // счетчик обслуженных деталей



            double T_obs = 0; // время обслуживания

            double T_ob = 0;

            Random random = new Random();


            while (t_prich < SimulationTime)
            {
                bool flag = false;

                t_prich += arrivalTime(random, Lyamda);
                k++;
                //Console.WriteLine($"Заявка № {k} пришла в {t_prich*60} секунд");

                foreach (Machine machine in Machines)
                {
                    if (flag)
                    {
                        break;
                    }
                    if (t_prich < machine.T_osv)
                    {
                        //Console.WriteLine($"Станок № {machine.Index} занят, будет свободен в  {machine.T_osv * 60 + (machine.Index - 1)} секунд");
                        foreach (Buffer buffer in machine.Buffers)
                        {
                            if (t_prich < buffer.T_osv)
                            {
                                //Console.WriteLine($"Буфер станка № {machine.Index} занят");
                            }
                            else
                            {
                                //Console.WriteLine($"Буфер  станка № {machine.Index} свободен");
                                double t_nach = machine.T_osv;
                                //Console.WriteLine($"Время начала обслуживания заявки № {k} - {t_nach * 60} секунд, время в пути до станка {(machine.Index - 1) * 60}");
                                double t_obs = arrivalTime(random, 1);

                                T_obs += t_obs;
                                //Console.WriteLine($"Время обслуживания заявки № {k} - {t_obs * 60} секунд");
                                t_osv = t_nach + t_obs;
                                buffer.T_osv = t_osv;
                                machine.T_osv = t_osv;

                                T_ob += (t_obs + (buffer.T_osv - machine.T_osv));



                                //Console.WriteLine($"Заявка № {k} обслужена в {t_osv * 60} секунд Станком № {machine.Index}\n");
                                N_obs++;
                                machine.ServicedDetail++;
                                flag = true;
                                break;

                            }
                        }
                    }
                    else
                    {
                        //Console.WriteLine($"Станок № {machine.Index} свободен");
                        double t_nach = t_prich + (machine.Index - 1);
                        //Console.WriteLine($"Время начала обслуживания заявки № {k} - {t_nach * 60} секунд, время в пути до станка {(machine.Index-1) * 60}");
                        double t_obs = arrivalTime(random, 1);
                        T_ob += (t_obs + (machine.Index - 1));

                        T_obs += t_obs;
                        //Console.WriteLine($"Время обслуживания заявки № {k} - {t_obs * 60} секунд");
                        t_osv = t_nach + t_obs;
                        machine.T_osv = t_osv;
                        //Console.WriteLine($"Заявка № {k} обслужена в {t_osv * 60} секунд Станком № {machine.Index}\n");
                        N_obs++;
                        machine.ServicedDetail++;
                        break;

                    }
                }
                //Console.WriteLine();

            }
            Console.WriteLine("Статистика:");
            Console.WriteLine($"Количество станков: {Machines.Length}");
            Console.WriteLine($"Количество места в буфере: {Machines.Last().Buffers.Length}");
            Console.WriteLine($"Количество заявок: {k}");
            Console.WriteLine($"Количество обслуженных заявок: {N_obs}");
            Console.WriteLine($"Количество необслуженных заявок: {k - N_obs}");


            foreach (Machine machine1 in Machines)
            {
                Console.WriteLine($"Количество обработанных деталей станком № {machine1.Index}: {machine1.ServicedDetail}");
            }
            Console.WriteLine($"Cреднее время обслуживания заявок: {T_obs / N_obs * 60} секунд");
            Console.WriteLine($"Cреднее время провожления заявок: {T_ob / N_obs * 60} секунд");

            Console.ReadLine();

        }

        public double arrivalTime(Random random, double lyamda)
        {

            return -Math.Log(1 - random.NextDouble()) / lyamda;
        }


        public void HZ(int N, double epsilon)
        {

        }


    }
}
