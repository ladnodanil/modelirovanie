using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace modelirovanie
{
    public class Buffer
    {

        public double T_osv { get; set; } // время когда ,буфер будет освобожден

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

        public double T_obs { get; set; } // 


        public Machine(int index, int sizeBuffer)
        {
            Index = index;
            T_osv = 0;
            ServicedDetail = 0;
            T_obs = 0;
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



        public (double, double, int, int, double,double) Simulate()
        {

            double t_prix = 0; // время прихода

            double t_osv = 0; // время освобождения заявки

            int N = 0; // счетчик деталей

            int N_obs = 0; // счетчик обслуженных деталей

            int N_neobs = 0; // необслуженные детали

            double T_obs = 0; // время обслуживания

            double T_preb = 0; // время пребывания в системе 

            double T_buffer = 0; // время в буфере


            Random random = new Random();

            while (t_prix < SimulationTime)
            {

                bool flag = false;

                bool IsComleted = false;

                t_prix += arrivalTime(random, Lyamda);
                N++;
                //Console.WriteLine($"Заявка № {N} пришла в {t_prix*60} секунд");

                foreach (Machine machine in Machines)
                {
                    if (flag)
                    {
                        break;
                    }
                    if (t_prix < machine.T_osv)
                    {
                        //Console.WriteLine($"Станок № {machine.Index} занят, будет свободен в  {machine.T_osv * 60 + (machine.Index - 1)} секунд");
                        foreach (Buffer buffer in machine.Buffers)
                        {
                            if (t_prix < buffer.T_osv)
                            {
                                //Console.WriteLine($"Буфер станка № {machine.Index} занят до {buffer.T_osv * 60}");
                                IsComleted = false;

                            }
                            else
                            {
                                //Console.WriteLine($"Буфер  станка № {machine.Index} свободен");
                                double t_nach = machine.T_osv;
                                //Console.WriteLine($"Время начала обслуживания заявки № {N} - {t_nach * 60} секунд, время в пути до станка {(machine.Index - 1) * 60}");
                                double t_obs = arrivalTime(random, 1);
                                t_osv = t_nach + t_obs;
                                if (t_osv < SimulationTime)
                                {
                                    T_obs += t_obs;
                                    //Console.WriteLine($"Время обслуживания заявки № {N} - {t_obs * 60} секунд");
                                    buffer.T_osv = t_osv;
                                    machine.T_osv = t_osv;
                                    machine.T_obs += t_obs;

                                    //Console.WriteLine($"Время в буфере:  {(t_nach - t_prix) * 60}");

                                    T_buffer += (t_nach - t_prix);

                                    T_preb += (t_obs + (t_nach - t_prix));



                                    //Console.WriteLine($"Заявка № {N} обслужена в {t_osv * 60} секунд Станком № {machine.Index}\n");
                                    N_obs++;
                                    machine.ServicedDetail++;
                                    flag = true;
                                    IsComleted = true;
                                    break;

                                }
                                else
                                {
                                    break;
                                    continue;

                                }


                            }
                        }
                    }
                    else
                    {
                        //Console.WriteLine($"Станок № {machine.Index} свободен");
                        double t_nach = t_prix + (machine.Index - 1);
                        //Console.WriteLine($"Время начала обслуживания заявки № {N} - {t_nach * 60} секунд, время в пути до станка {(machine.Index-1) * 60}");
                        double t_obs = arrivalTime(random, 1);
                        t_osv = t_nach + t_obs;

                        //Console.WriteLine($"Время обслуживания заявки № {N} - {t_obs * 60} секунд");
                        if (t_osv < SimulationTime)

                        {
                            T_obs += t_obs;
                            T_preb += (t_obs + (machine.Index - 1));
                            machine.T_osv = t_osv;
                            machine.T_obs += t_obs;
                            //Console.WriteLine($"Заявка № {N} обслужена в {t_osv * 60} секунд Станком № {machine.Index}\n");
                            N_obs++;
                            machine.ServicedDetail++;
                            IsComleted = true;
                            break;

                        }
                        else
                        {
                            break;
                            continue;
                        }

                    }
                }
                if (IsComleted == false)
                {
                    N_neobs++;
                }
                //Thread.Sleep(10);

                //Console.WriteLine();

            }
            double ver_obs = (double)N_obs / N;
            double ver_neobs = (double)N_neobs / N;

            Console.WriteLine("Статистика:");
            Console.WriteLine($"Количество станков: {Machines.Length}");
            Console.WriteLine($"Количество места в буфере: {Machines.Last().Buffers.Length}");
            Console.WriteLine($"Количество заявок: {N}");
            Console.WriteLine($"Количество обслуженных заявок: {N_obs} - Вероятность обслуживания детали {ver_obs:F2} %");
            Console.WriteLine($"Количество необслуженных заявок: {N_neobs} - Вероятность необслуживания детали {ver_neobs:F2} %");

            foreach (Machine machine1 in Machines)
            {
                Console.WriteLine($"Количество обработанных деталей станком № {machine1.Index}: {machine1.ServicedDetail} - Вероятность попадания в станок {(double)machine1.ServicedDetail / N_obs:F2} %");
            }
            double f = 0;
            foreach (Machine machine1 in Machines)
            {
                //Console.WriteLine($"Время обслуживания деталей станком № {machine1.Index}: {machine1.T_obs / SimulationTime:F2}");
                f += (machine1.T_obs / SimulationTime);

            }
            //Console.WriteLine($"{f / Machines.Length:F2}\n\n"); // вероятность загрузки станка
            //Console.WriteLine($"Cреднее время обслуживания заявок: {T_obs / N_obs * 60} секунд");
            //Console.WriteLine($"Cреднее время в буфере: {T_buffer / N_obs * 60} секунд");
            //Console.WriteLine($"Cреднее время пребывание заявок: {T_preb / N_obs * 60} секунд\n\n");
            return (ver_obs, T_preb / N_obs * 60, Machines.Length, Machines.Last().Buffers.Length, T_buffer / N_obs * 60, f / Machines.Length);

        }

        public double arrivalTime(Random random, double lyamda)
        {

            return -Math.Log(1 - random.NextDouble()) / lyamda;
        }
    }
}
