using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFlappy
{
	internal class GameEngine
	{
		CancellationTokenSource _cancel;

		Stopwatch sw = Stopwatch.StartNew();

		double _currentMillis = 0;


		ConcurrentQueue<object> InputQueue = new ConcurrentQueue<object>();

		public FlappyGame Game { get; }



		// CONSTRUTOR
		public GameEngine(FlappyGame game)
		{
			Game = game;

			Task.Run(() => Loop());
		}

		private void Loop()
		{
			_cancel = new CancellationTokenSource();

			while (!_cancel.IsCancellationRequested)
			{
				_currentMillis = sw.Elapsed.TotalMilliseconds;

				Game.Input(InputQueue);

				Game.Update(_currentMillis);

				Game.Draw();
			}
		}
	}
}