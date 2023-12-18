using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace CookPopularUI.WPF
{
    public class DropShadowExEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(DropShadowExEffect), 0);
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(DropShadowExEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
        public static readonly DependencyProperty DepthProperty = DependencyProperty.Register("Depth", typeof(double), typeof(DropShadowExEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(1)));
        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register("Color", typeof(Color), typeof(DropShadowExEffect), new UIPropertyMetadata(Color.FromArgb(255, 0, 0, 0), PixelShaderConstantCallback(2)));
        public static readonly DependencyProperty OpacityProperty = DependencyProperty.Register("Opacity", typeof(double), typeof(DropShadowExEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(3)));
        public DropShadowExEffect()
        {
            PixelShader pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri("/CookPopularUI.WPF;component/Common/Effects/DropShadowExEffect.ps", UriKind.Relative);
            this.PixelShader = pixelShader;

            this.UpdateShaderValue(InputProperty);
            this.UpdateShaderValue(AngleProperty);
            this.UpdateShaderValue(DepthProperty);
            this.UpdateShaderValue(ColorProperty);
            this.UpdateShaderValue(OpacityProperty);
        }
        public Brush Input
        {
            get
            {
                return ((Brush)(this.GetValue(InputProperty)));
            }
            set
            {
                this.SetValue(InputProperty, value);
            }
        }
        public double Angle
        {
            get
            {
                return ((double)(this.GetValue(AngleProperty)));
            }
            set
            {
                this.SetValue(AngleProperty, value);
            }
        }
        public double Depth
        {
            get
            {
                return ((double)(this.GetValue(DepthProperty)));
            }
            set
            {
                this.SetValue(DepthProperty, value);
            }
        }
        public Color Color
        {
            get
            {
                return ((Color)(this.GetValue(ColorProperty)));
            }
            set
            {
                this.SetValue(ColorProperty, value);
            }
        }
        public double Opacity
        {
            get
            {
                return ((double)(this.GetValue(OpacityProperty)));
            }
            set
            {
                this.SetValue(OpacityProperty, value);
            }
        }
    }
}
