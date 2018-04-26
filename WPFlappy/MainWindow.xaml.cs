using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFlappy
{
	public partial class MainWindow : Window
	{
		GameEngine _engine = new GameEngine(new FloppyGame());

		public MainWindow()
		{
			InitializeComponent();

			CompositionTarget.Rendering += (a, b) => Render();

			PreviewKeyDown += Input;
		}

		void Input(object sender, KeyEventArgs e)
		{
			_engine.KeyInput(e.Key);
		}

		void Render()
		{
			//frame.Source = 
			_engine.GetNextFrame();
		}
	}
}
