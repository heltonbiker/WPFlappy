using GalaSoft.MvvmLight;
using NAudio.Vorbis;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace WPFlappy
{
	public class Player : ObservableObject
	{
		double jump = -4.6;
		double gravity = 0.25;


		public double Velocity { get; private set; } = 0;
		public double Position { get; set; } = 180;
		public double Rotation { get; private set; } = 0;

		public double Bottom => Position - 10; // Introduce a more realistic calculation here;
		public double Top => Position + 10;

		SoundPlayer soundJump = new SoundPlayer("assets/sounds/sfx_wing.wav");

		WaveStream soundJumpStream = new VorbisWaveReader("assets/sounds/sfx_wing.ogg");
		WaveOutEvent jumpEvent;


		// CONSTRUCTOR
		public Player()
		{
			WaveChannel32 jumpChannel = new WaveChannel32(soundJumpStream);
			jumpEvent = new WaveOutEvent();
			jumpEvent.Init(jumpChannel);
		}



		public void Jump()
		{
			Velocity = jump;

			//soundJump.Stop();
			//soundJump.Play();
			jumpEvent.Stop();
			jumpEvent.Play();
		}

		public void Update()
		{
			Velocity += gravity;
			Position += Velocity;
			Rotation = Math.Min((Velocity / 10) * 90, 90);
		}
	}
}
