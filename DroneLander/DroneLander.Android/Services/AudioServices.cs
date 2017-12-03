using System;
using Android.Media;
using Xamarin.Forms;
using DroneLander.Droid.Services;
using DroneLander.Services;

[assembly: Dependency(typeof(AudioService))]
namespace DroneLander.Droid.Services
{
    public class AudioService : IAudioService
    {
        private MediaPlayer _mediaPlayer;

        public Action OnFinishedPlaying { get; set; }

        public void AdjustVolume(double level)
        {
            float volume = (float)(level / 100.0);
            if (volume == 0.0f) volume = 0.1f;
            _mediaPlayer.SetVolume(volume, volume);
        }

        public void KillEngine()
        {
            _mediaPlayer?.SetVolume(0.0f, 0.0f);
        }

        public void ToggleEngine()
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Completion -= OnMediaCompleted;
                _mediaPlayer.Stop();
                _mediaPlayer = null;
            }
            else
            {
                const string fullPath = "Sounds/engine.m4a";
                Android.Content.Res.AssetFileDescriptor afd = null;

                try
                {
                    afd = Forms.Context.Assets.OpenFd(fullPath);
                }
                catch (Exception)
                {
                    // ignored
                }

                if (afd != null)
                {
                    if (_mediaPlayer == null)
                    {
                        _mediaPlayer = new MediaPlayer();
                        _mediaPlayer.Prepared += (sender, args) =>
                        {
                            _mediaPlayer.Start();
                            _mediaPlayer.Completion += OnMediaCompleted;
                        };
                    }

                    _mediaPlayer.Reset();
                    _mediaPlayer.SetVolume(0.1f, 0.1f);
                    _mediaPlayer.SetDataSource(afd.FileDescriptor, afd.StartOffset, afd.Length);
                    _mediaPlayer.PrepareAsync();
                }
            }
        }

        private void OnMediaCompleted(object sender, EventArgs e)
        {
            OnFinishedPlaying?.Invoke();
        }
    }
}