﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CafeLib.Extensions;
using CafeLib.Support;
using CafeLib.ViewModels;
using DroneLander.Common;
using Xamarin.Forms;

namespace DroneLander.ViewModels
{
    [Page(typeof(MainPage))]
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
            ActiveLandingParameters = new LandingParameters();
            Altitude = ActiveLandingParameters.Altitude;
            Velocity = ActiveLandingParameters.Velocity;
            Fuel = ActiveLandingParameters.Fuel;
            Thrust = ActiveLandingParameters.Thrust;
            FuelRemaining = CoreConstants.StartingFuel;
            IsActive = false;
            ActivityPage.SetViewModel(this);
        }

        public MainPage ActivityPage => GetPage<MainPage>();

        public LandingParameters ActiveLandingParameters { get; set; }

        private double _altitude;
        public double Altitude
        {
            get { return _altitude; }
            set { SetValue(ref _altitude, value); }
        }

        private double _descentRate;
        public double DescentRate
        {
            get { return _descentRate; }
            set { SetValue(ref _descentRate, value); }
        }

        private double _velocity;
        public double Velocity
        {
            get { return _velocity; }
            set { SetValue(ref _velocity, value); }
        }

        private double _fuel;
        public double Fuel
        {
            get { return _fuel; }
            set { SetValue(ref _fuel, value); }
        }

        private double _fuelRemaining;
        public double FuelRemaining
        {
            get { return _fuelRemaining; }
            set { SetValue(ref _fuelRemaining, value); }
        }

        private double _thrust;
        public double Thrust
        {
            get { return this._thrust; }
            set { SetValue(ref _thrust, value); }
        }

        private double _throttle;
        public double Throttle
        {
            get { return _throttle; }
            set
            {
                SetValue(ref _throttle, value);
                if (IsActive && FuelRemaining > 0.0) Helpers.AudioHelper.AdjustVolume(value);
            }
        }

        private bool _isActionable() => true;

        private string _actionLabel;
        public string ActionLabel
        {
            get { return _actionLabel; }
            set { SetValue(ref _actionLabel, value); }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                SetValue(ref _isActive, value);
                ActionLabel = _isActive ? "Reset" : "Start";
            }
        }

        public ICommand AttemptLandingCommand
        {
            get
            {
                return new RelayCommand(() =>
                {
                    IsActive = !IsActive;

                    if (IsActive)
                    {
                        StartLanding();
                    }
                    else
                    {
                        ResetLanding();
                    }

                }, _isActionable);
            }
        }

        public void StartLanding()
        {
            Helpers.AudioHelper.ToggleEngine();

            Device.StartTimer(TimeSpan.FromMilliseconds(CoreConstants.PollingIncrement), () =>
            {
                UpdateFlightParameters();

                if (ActiveLandingParameters.Altitude > 0.0)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Altitude = ActiveLandingParameters.Altitude;
                        DescentRate = ActiveLandingParameters.Velocity;
                        FuelRemaining = ActiveLandingParameters.Fuel / 1000;
                        Thrust = ActiveLandingParameters.Thrust;
                    });

                    if (FuelRemaining == 0.0) Helpers.AudioHelper.KillEngine();

                    return IsActive;
                }
                else
                {
                    ActiveLandingParameters.Altitude = 0.0;
                    IsActive = false;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Altitude = ActiveLandingParameters.Altitude;
                        DescentRate = ActiveLandingParameters.Velocity;
                        FuelRemaining = ActiveLandingParameters.Fuel / 1000;
                        Thrust = ActiveLandingParameters.Thrust;
                    });

                    if (ActiveLandingParameters.Velocity > -5.0)
                    {
                        DisplayAlert("Landing Status", "The Eagle has landed!");
                    }
                    else
                    {
                        ShakeLandscapeAsync();
                        DisplayAlert("Landing Status", "That's going to leave a mark!");
                    }

                    ResetLanding();
                    return false;
                }
            });
        }

        private void UpdateFlightParameters()
        {
            double seconds = CoreConstants.PollingIncrement / 1000.0;

            // Compute thrust and remaining fuel
            //thrust = throttle * 1200.0;
            var used = (Throttle * seconds) / 10.0;
            used = Math.Min(used, ActiveLandingParameters.Fuel); // Can't burn more fuel than you have
            ActiveLandingParameters.Thrust = used * 25000.0;
            ActiveLandingParameters.Fuel -= used;

            // Compute new flight parameters
            double avgmass = CoreConstants.LanderMass + (used / 2.0);
            double force = ActiveLandingParameters.Thrust - (avgmass * CoreConstants.Gravity);
            double acc = force / avgmass;

            double vel2 = ActiveLandingParameters.Velocity + (acc * seconds);
            double avgvel = (ActiveLandingParameters.Velocity + vel2) / 2.0;
            ActiveLandingParameters.Altitude += (avgvel * seconds);
            ActiveLandingParameters.Velocity = vel2;
        }

        private async void ShakeLandscapeAsync()
        {
            var page = ActivityPage;
            try
            {
                for (var i = 0; i < 8; i++)
                {
                    await Task.WhenAll(
                            page.ScaleTo(1.1, 20, Easing.Linear),
                            page.TranslateTo(-30, 0, 20, Easing.Linear)
                        );

                    await Task.WhenAll(
                            page.TranslateTo(0, 0, 20, Easing.Linear)
                       );

                    await Task.WhenAll(
                            page.TranslateTo(0, -30, 20, Easing.Linear)
                       );

                    await Task.WhenAll(
                             page.ScaleTo(1.0, 20, Easing.Linear),
                             page.TranslateTo(0, 0, 20, Easing.Linear)
                         );
                }
            }
            catch { }
        }

        private async void ResetLanding()
        {
            Helpers.AudioHelper.ToggleEngine();

            await Task.Delay(500);

            ActiveLandingParameters = new LandingParameters();

            Altitude = 5000.0;
            Velocity = 0.0;
            Fuel = 1000.0;
            FuelRemaining = 1000.0;
            Thrust = 0.0;
            DescentRate = 0.0;
            Throttle = 0.0;
        }
    }
}