using System;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Media;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace WPFlappy
{
	public class FlappyGame : ObservableObject
	{
		
		double _lastMillis = 0;
		const long UPDATE_STEP = 1;

		states currentstate;

		bool loopGameloop = false;
		bool loopPipeloop = false;

		double _x = 0;

		double gravity = 0.25;
		double velocity = 0;
		double position = 180;

		double jump = -4.6;



		SoundPlayer soundJump =   new SoundPlayer("assets/sounds/sfx_wing.wav");
		SoundPlayer soundScore =  new SoundPlayer("assets/sounds/sfx_point.wav");
		SoundPlayer soundHit =    new SoundPlayer("assets/sounds/sfx_hit.wav");
		SoundPlayer soundDie =    new SoundPlayer("assets/sounds/sfx_die.wav");
		SoundPlayer soundSwoosh = new SoundPlayer("assets/sounds/sfx_swooshing.wav");


		public void Input(ConcurrentQueue<object> commandQueue)
		{
			while (commandQueue.Count > 0 && 
				   commandQueue.TryDequeue(out object result))
			{
				if (result is Key key)
				{
					switch (key)
					{
						case Key.Space:
							if (currentstate == states.ScoreScreen)
								Replay();
							else
								screenClick();
							break;
					}
				}
			}
		}

		public void Draw()
		{
			RaisePropertyChanged(null);
		}

		public void Update(double currentMillis)
		{
			double elapsed = currentMillis - _lastMillis;
			_lastMillis = currentMillis;

			double lag = elapsed;

			while (lag >= UPDATE_STEP)
			{
				UpdateInternal(currentMillis);
				lag -= UPDATE_STEP;
			}

		}


		void UpdateInternal(double t)
		{
			_x = Math.Sin(t / 100) * 300;

			if (loopGameloop)
				gameloop();

			if (loopPipeloop)
				updatePipes();			
		}

		void gameloop()
		{
			//var player = $("#player");

			////update the player speed/position
			velocity += gravity;
			position += velocity;

			////update the player
			//updatePlayer(player);

			////create the bounding box
			//var box = document.getElementById('player').getBoundingClientRect();
			//var origwidth = 34.0;
			//var origheight = 24.0;

			//var boxwidth = origwidth - (Math.sin(Math.abs(rotation) / 90) * 8);
			//var boxheight = (origheight + box.height) / 2;
			//var boxleft = ((box.width - boxwidth) / 2) + box.left;
			//var boxtop = ((box.height - boxheight) / 2) + box.top;
			//var boxright = boxleft + boxwidth;
			//var boxbottom = boxtop + boxheight;

			////if we're in debug mode, draw the bounding box

			////did we hit the ground?
			//if (box.bottom >= $("#land").offset().top)
			//{
			//	playerDead();
			//	return;
			//}

			////have they tried to escape through the ceiling? :o
			//var ceiling = $("#ceiling");
			//if (boxtop <= (ceiling.offset().top + ceiling.height()))
			//	position = 0;

			////we can't go any further without a pipe
			//if (pipes[0] == null)
			//	return;

			////determine the bounding box of the next pipes inner area
			//var nextpipe = pipes[0];
			//var nextpipeupper = nextpipe.children(".pipe_upper");

			//var pipetop = nextpipeupper.offset().top + nextpipeupper.height();
			//var pipeleft = nextpipeupper.offset().left - 2; // for some reason it starts at the inner pipes offset, not the outer pipes.
			//var piperight = pipeleft + pipewidth;
			//var pipebottom = pipetop + pipeheight;

			//if (debugmode)
			//{
			//	var boundingbox = $("#pipebox");
			//	boundingbox.css('left', pipeleft);
			//	boundingbox.css('top', pipetop);
			//	boundingbox.css('height', pipeheight);
			//	boundingbox.css('width', pipewidth);
			//}

			////have we gotten inside the pipe yet?
			//if (boxright > pipeleft)
			//{
			//	//we're within the pipe, have we passed between upper and lower pipes?
			//	if (boxtop > pipetop && boxbottom < pipebottom)
			//	{
			//		//yeah! we're within bounds

			//	}
			//	else
			//	{
			//		//no! we touched the pipe
			//		playerDead();
			//		return;
			//	}
			//}


			////have we passed the imminent danger?
			//if (boxleft > piperight)
			//{
			//	//yes, remove it
			//	pipes.splice(0, 1);

			//	//and score a point
			//	playerScore();
			//}
		}


		void updatePipes()
		{
			/*
		   //Do any pipes need removal?
		   $(".pipe").filter(function() { return $(this).position().left <= -100; }).remove()

		   //add a new pipe (top height + bottom height  + pipeheight == flyArea) and put it in our tracker
					var padding = 80;
					var constraint = flyArea - pipeheight - (padding * 2); //double padding (for top and bottom)
					var topheight = Math.floor((Math.random() * constraint) + padding); //add lower padding
					var bottomheight = (flyArea - pipeheight) - topheight;
					var newpipe = $('<div class="pipe animated"><div class="pipe_upper" style="height: ' + topheight + 'px;"></div><div class="pipe_lower" style="height: ' + bottomheight + 'px;"></div></div>');
		   $("#flyarea").append(newpipe);
					pipes.push(newpipe);
			*/
		}


		void screenClick()
		{
			if (currentstate == states.GameScreen)
			{
				playerJump();
			}
			else if (currentstate == states.SplashScreen)
			{
				startGame();
			}
		}

		private void playerJump()
		{
			velocity = jump;
			//play jump sound
			soundJump.Stop();
			soundJump.Play();
		}

		private void startGame()
		{
			currentstate = states.GameScreen;

			//fade out the splash
			fadeOutSplash();
			//$("#splash").stop();
			//$("#splash").transition({ opacity: 0 }, 500, 'ease');

			//update the big score
			setBigScore();


			//start up our loops
			//var updaterate = 1000.0 / 60.0; //60 times a second
			//loopGameloop = setInterval(gameloop, updaterate);
			//loopPipeloop = setInterval(updatePipes, 1400);
			loopGameloop = true;
			loopPipeloop = true;

			//jump from the start!
			playerJump();
		}


		private void setBigScore()
		{
			//throw new NotImplementedException();
			Console.WriteLine("Set Bit Score");
		}

		private void fadeOutSplash()
		{
			//throw new NotImplementedException();
			Console.WriteLine("Fade Out Splash");
		}

		void Replay()
		{
			throw new NotImplementedException();
		}


		public ICommand ClickCommand => new RelayCommand(screenClick);
	}

	enum states
	{
		SplashScreen = 0,
		GameScreen = 1,
		ScoreScreen = 2
	}
}