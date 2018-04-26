using System.Windows.Media;

namespace WPFlappy
{
	internal interface IGame
	{
		void Input();
		void Update(double _currentMillis);
		void Draw(DrawingContext cx);
	}
}