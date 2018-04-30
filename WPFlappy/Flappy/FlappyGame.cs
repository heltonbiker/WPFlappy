using System;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Media;
using System.ComponentModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;

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

		double ceiling = 400;  //////////////////

		List<Pipe> pipes = new List<Pipe>();
		Player player = new Player();


		SoundPlayer soundScore =  new SoundPlayer("assets/sounds/sfx_point.wav");
		SoundPlayer soundHit =    new SoundPlayer("assets/sounds/sfx_hit.wav");
		SoundPlayer soundDie =    new SoundPlayer("assets/sounds/sfx_die.wav");
		SoundPlayer soundSwoosh = new SoundPlayer("assets/sounds/sfx_swooshing.wav");


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
			player.Update();

			////did we hit the ground?
			if (player.Bottom < 0)
			{
				playerDead();
				return;
			}

			////have they tried to escape through the ceiling? :o
			if (player.Top > ceiling) //boxtop <= (ceiling.offset().top + ceiling.height()))
				player.Position = ceiling;

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
			switch (currentstate)
			{
				case states.ScoreScreen:
					Replay();
					break;
				case states.GameScreen:
					player.Jump();
					break;
				case states.SplashScreen:
					startGame();
					break;
			}
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
			player.Jump();
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


		void playerDead()
		{
		 //  //stop animating everything!
		 //  $(".animated").css('animation-play-state', 'paused');
		 //  $(".animated").css('-webkit-animation-play-state', 'paused');

			////drop the bird to the floor
			//var playerbottom = $("#player").position().top + $("#player").width(); //we use width because he'll be rotated 90 deg
			//var floor = flyArea;
			//var movey = Math.max(0, floor - playerbottom);
		 //  $("#player").transition({ y: movey + 'px', rotate: 90}, 1000, 'easeInOutCubic');

			////it's time to change states. as of now we're considered ScoreScreen to disable left click/flying
			//currentstate = states.ScoreScreen;

			////destroy our gameloops
			//clearInterval(loopGameloop);
			//clearInterval(loopPipeloop);
			//loopGameloop = null;
			//loopPipeloop = null;

			////mobile browsers don't support buzz bindOnce event
			//if (isIncompatible.any())
			//{
			//	//skip right to showing score
			//	showScore();
			//}
			//else
			//{
			//	//play the hit sound (then the dead sound) and then show score
			//	soundHit.play().bindOnce("ended", function() {
			//		soundDie.play().bindOnce("ended", function() {
			//			showScore();
			//		});
			//	});
			//}
		}


		public ICommand ClickCommand 
			=> new RelayCommand(screenClick);
	}

	enum states
	{
		SplashScreen = 0,
		GameScreen = 1,
		ScoreScreen = 2
	}
}