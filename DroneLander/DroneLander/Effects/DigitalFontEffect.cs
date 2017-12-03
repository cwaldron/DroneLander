using Xamarin.Forms;

namespace DroneLander.Effects
{
    public static class DigitalFontEffect
    {
        public static readonly BindableProperty FontFileNameProperty = BindableProperty.CreateAttached("FontFileName", typeof(string), typeof(DigitalFontEffect), "", propertyChanged: OnFileNameChanged);

        public static string GetFontFileName(BindableObject view)
        {
            return (string)view.GetValue(FontFileNameProperty);
        }

        public static void SetFontFileName(BindableObject view, string value)
        {
            view.SetValue(FontFileNameProperty, value);
        }

        private static void OnFileNameChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is View view))
            {
                return;
            }
            view.Effects.Add(new FontEffect());
        }

        private class FontEffect : RoutingEffect
        {
            public FontEffect() : base("Xamarin.FontEffect")
            {
            }
        }
    }
}