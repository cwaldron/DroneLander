using System;

namespace DroneLander.Services
{
    public interface IAudioService
    {
        void AdjustVolume(double volume);
        void KillEngine();
        void ToggleEngine();
        Action OnFinishedPlaying { get; set; }
    }
}