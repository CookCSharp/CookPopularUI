using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Media3D;


namespace CookPopularUI.WPF
{
	
	/// <summary>An effect that blurs in a single direction.</summary>
	public class DirectionalBlurEffect : ShaderEffect
	{
		public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(DirectionalBlurEffect), 0);
		public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(DirectionalBlurEffect), new UIPropertyMetadata(((double)(0D)), PixelShaderConstantCallback(0)));
		public static readonly DependencyProperty BlurAmountProperty = DependencyProperty.Register("BlurAmount", typeof(double), typeof(DirectionalBlurEffect), new UIPropertyMetadata(((double)(0.003D)), PixelShaderConstantCallback(1)));
		
		public DirectionalBlurEffect() {
			PixelShader pixelShader = new PixelShader();
			pixelShader.UriSource = new Uri("/CookPopularUI.WPF;component/Common/Effects/DirectionalBlurEffect.ps", UriKind.Relative);
			this.PixelShader = pixelShader;

			this.UpdateShaderValue(InputProperty);
			this.UpdateShaderValue(AngleProperty);
			this.UpdateShaderValue(BlurAmountProperty);
		}

		public Brush Input {
			get {
				return ((Brush)(this.GetValue(InputProperty)));
			}
			set {
				this.SetValue(InputProperty, value);
			}
		}

		/// <summary>The direction of the blur (in degrees).</summary>
		public double Angle {
			get {
				return ((double)(this.GetValue(AngleProperty)));
			}
			set {
				this.SetValue(AngleProperty, value);
			}
		}

		/// <summary>The scale of the blur (as a fraction of the input size).</summary>
		public double BlurAmount {
			get {
				return ((double)(this.GetValue(BlurAmountProperty)));
			}
			set {
				this.SetValue(BlurAmountProperty, value);
			}
		}
	}
}
