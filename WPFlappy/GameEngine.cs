using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WPFlappy
{
	internal class GameEngine
	{
		private IGame _game;

		public GameEngine(IGame game)
		{
			_game = game;
		}





		const int W = 800;
		const int H = 600;

		Stopwatch sw = Stopwatch.StartNew();

		double _currentMillis = 0;



		internal ImageSource GetNextFrame()
		{
			_currentMillis = sw.Elapsed.TotalMilliseconds;
			
			_game.Input(InputQueue);

			_game.Update(_currentMillis);

			var frame = Render();

			return frame;
		}

		ConcurrentQueue<object> InputQueue = new ConcurrentQueue<object>();

		internal void KeyInput(Key key)
		{
			InputQueue.Enqueue(key);
		}

		ImageSource Render()
		{
			var dv = new DrawingVisual();

			using (DrawingContext cx = dv.RenderOpen())
			{
				cx.PushTransform(new TranslateTransform(W * 0.5, H));
				cx.PushTransform(new ScaleTransform(1, -1));
				_game.Draw(cx);
			}

			var frame = new RenderTargetBitmap(W, H, 96, 96, PixelFormats.Pbgra32);
			frame.Render(dv);
			frame.Freeze();

			return frame;
		}
	}
}