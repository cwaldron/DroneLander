﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Widget;
using Android.Graphics;
using System.ComponentModel;
using DroneLander;
using DroneLander.Droid;

[assembly: ResolutionGroupName("Xamarin")]
[assembly: ExportEffect(typeof(DroneLander.Droid.DigitalFontEffect), "FontEffect")]
namespace DroneLander.Droid
{
    public class DigitalFontEffect : PlatformEffect
    {
        TextView control;
        protected override void OnAttached()
        {
            try
            {
                control = Control as TextView;
                Typeface font = Typeface.CreateFromAsset(Forms.Context.Assets, "Fonts/" + Effects.DigitalFontEffect.GetFontFileName(Element) + ".ttf");
                control.Typeface = font;
            }
            catch (Exception)
            {
            }
        }

        protected override void OnDetached()
        {
        }

        protected override void OnElementPropertyChanged(PropertyChangedEventArgs args)
        {
            if (args.PropertyName == Effects.DigitalFontEffect.FontFileNameProperty.PropertyName)
            {
                Typeface font = Typeface.CreateFromAsset(Forms.Context.ApplicationContext.Assets, "Fonts/" + Effects.DigitalFontEffect.GetFontFileName(Element) + ".ttf");
                control.Typeface = font;
            }
        }
    }
}