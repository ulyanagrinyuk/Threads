using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Threading;
using System.Runtime.InteropServices;

namespace Car
{
	internal class Car
	{
		static readonly int MAX_SPEED_LOW_LIMIT = 100;
		static readonly int MAX_SPEED_HIGH_LIMIT = 400;
		public int MAX_SPEED { get; }
		Engine engine;
		Tank tank;
		bool driver_inside;
		int speed;
		int acceleration;
		struct Threads
		{
			public Thread PanelThreads { get; set; }
			public Thread EngineIdleThread {  get; set; }
			public Thread FreeWheelingThread { get; set; }
		}
		Threads threads = new Threads();
		public Car(double consumtion, int volume, int max_speed, int accelleration = 1)
		{
			engine = new Engine(consumtion);
			tank = new Tank(volume);
			driver_inside = false;
			speed = 0;
			//this = accelleration;
			Threads threads = new Threads();
			if (max_speed < MAX_SPEED_LOW_LIMIT) max_speed = MAX_SPEED_LOW_LIMIT;
			if (max_speed > MAX_SPEED_HIGH_LIMIT) max_speed = MAX_SPEED_HIGH_LIMIT;
			this.MAX_SPEED = max_speed;
			this.acceleration = acceleration;
		}
		public void GetIn()
		{
			driver_inside = true;
			threads.PanelThreads = new Thread(Panel);
			threads.PanelThreads.Start();
		}
		public void GetOut()
		{
			driver_inside = false;
			threads.PanelThreads.Join();
			Console.Clear();
			Console.WriteLine("You are out of the car");
		}
		public void Start()
		{
			engine.Srart();
			threads.EngineIdleThread = new Thread(EngineIdle);
			threads.EngineIdleThread.Start();
		}
		public void Stop()
		{
			engine.Stop();
			threads.EngineIdleThread.Join();
		}
		void Accelerate()
		{
			if (engine.Started) speed += acceleration;
			if (speed > MAX_SPEED) speed = MAX_SPEED;
			if (threads.FreeWheelingThread == null)
			{
				threads.FreeWheelingThread = new Thread(FreeWheeling);
				threads.FreeWheelingThread.Start();
			}
			//if(threads.FreeWheelingThread != null)Thread.Sleep(1000);
		}
		void SlowDown()
		{
			if (speed > 0) speed -= acceleration;
			if (speed < 0) speed = 0;
			if (speed == 0 && threads.FreeWheelingThread != null) threads.FreeWheelingThread.Join();
			//Thread.Sleep(1000);
		}
		public void Control()
		{
			Console.WriteLine("Your car is ready, press Enter to get in");
			ConsoleKey key;
			do
			{
				key = Console.ReadKey(true).Key;
				switch (key)
				{
					case ConsoleKey.Enter:
						if (driver_inside) GetOut();
						else GetIn();
						break;
					case ConsoleKey.Escape:
						Stop();
						GetOut();
						break;
					case ConsoleKey.F:
						if (!driver_inside)
						{
							Console.Write("Введите объем топлива: ");
							string s_amount = Console.ReadLine();
							double amount = Convert.ToDouble(s_amount.Replace(',', '.'));
							tank.Fill(amount);
						}
						else
						{
							Console.WriteLine("Get out of the car");
						}
						break;
					case ConsoleKey.I:  //Ignition
						if (engine.Started) Stop();
						else Start();
						break;
					case ConsoleKey.W:
						Accelerate();
						break;
					case ConsoleKey.S:
						SlowDown();
						break;
				}
				//Thread.Sleep(1000);
			} while (key != ConsoleKey.Escape);
		}
		void FreeWheeling()
		{
			while (speed-- > 0)
			{
				if (speed < 0) speed = 0;
				Thread.Sleep(1000);
			}
			if (speed < 0) speed = 0;
		}
		void EngineIdle()
		{
			while (engine.Started && tank.GiveFuel(engine.ConsumptionPerSecond) > 0)
			{
				Thread.Sleep(1000);
			}
		}
		void Panel()
		{
			while (driver_inside)
			{
				Console.Clear();
				Console.Write($"Fuel level: {tank.FuelLevel} liters");
				if (tank.FuelLevel < 5)
				{
					Console.Write("\t\t");
					Console.BackgroundColor = ConsoleColor.Red;
					Console.ForegroundColor = ConsoleColor.White;
					Console.Write("LOW FUEL");
				}
				Console.ResetColor();
				Console.WriteLine();
				Console.WriteLine($"Engine is {(engine.Started ? "started" : "stopped")}");
				Console.WriteLine($"Speed: {speed} km/h");
				Thread.Sleep(200);
			}
		}

		public void Info()
		{
			engine.Info();
			tank.Info();
			Console.WriteLine($"Max speed: {MAX_SPEED}km/h");
		}
	
	}
}
