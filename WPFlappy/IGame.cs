using System.Collections.Concurrent;
using System.Windows.Media;

namespace WPFlappy
{
	internal interface IGame
	{
		void Input(ConcurrentQueue<object> commandQueue);
		void Update(double _currentMillis);
		void Draw(DrawingContext cx);
	}
}