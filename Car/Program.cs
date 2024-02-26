using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car
{
	internal class Program
	{
		static void Main(string[] args)
		{
			//Tank tank = new Tank(15);
			//tank.Info();
			//int amout;
			//do
			//{
			//	Console.WriteLine("Введите объем топлива: ");
			//	amout = Convert.ToInt32(Console.ReadLine());
			//	tank.Fill(amout); 
			//	tank.Info();
			//} while (amout > 0);

			//Engine engine = new Engine(10);
			//engine.Info();

			Car car = new Car(10, 40, 250);
			car.Info();
			car.Control();
		}
	}
}
