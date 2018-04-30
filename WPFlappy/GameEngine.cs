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
	internal class GameEngine : IDisposable
	{
		CancellationTokenSource _cancel;

		Stopwatch sw = Stopwatch.StartNew();

		double _currentMillis = 0;


		public FlappyGame Game { get; }



		// CONSTRUTOR
		public GameEngine(FlappyGame game)
		{
			Game = game;

			Task.Run(() => Loop());
		}

		void Loop()
		{
			_cancel = new CancellationTokenSource();

			while (!_cancel.IsCancellationRequested)
			{
				_currentMillis = sw.Elapsed.TotalMilliseconds;

				//Game.Input(InputQueue);

				Game.Update(_currentMillis);

				Game.Draw();
			}
		}

		public void Dispose()
		{
			_cancel.Cancel();
			_cancel.Token.WaitHandle.WaitOne();
		}
	}
}