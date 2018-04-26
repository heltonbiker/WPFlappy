using System;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Media;

namespace WPFlappy
{
	internal class FloppyGame : IGame
	{
		double _x = 0;

		public void Input(ConcurrentQueue<object> commandQueue)
		{
			while (commandQueue.Count > 0 && 
				   commandQueue.TryDequeue(out object result))
			{
				Console.WriteLine(result);
			}
		}

		public void Draw(DrawingContext cx)
		{
			cx.DrawGeometry(Brushes.Black, null, new EllipseGeometry(new Point(_x, 300), 10, 10));
		}

		public void Update(double currentMillis)
		{
			_x = Math.Sin(currentMillis / 100) * 300;
		}
	}
}