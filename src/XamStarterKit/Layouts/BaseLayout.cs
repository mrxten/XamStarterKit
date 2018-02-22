using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace XamStarterKit.Layouts
{
	public class BaseLayout<T> : Layout<View>
	{
		Dictionary<T, View> _views = new Dictionary<T, View>();
		Dictionary<T, Rectangle> _viewsRects;

		protected double MaxWidth { get; set; }
		protected double MaxHeight { get; set; }
		protected double MinWidth { get; set; }
		protected double MinHeight { get; set; }

		public static readonly BindableProperty ViewTypeProperty =
			BindableProperty.CreateAttached(@"ViewType", typeof(T), typeof(BaseLayout<T>), default(T));

		public static T GetViewType(BindableObject bindable)
		{
			return (T)bindable.GetValue(ViewTypeProperty);
		}

		public static void SetViewType(BindableObject bindable, T type)
		{
			bindable.SetValue(ViewTypeProperty, type);
		}

		public BaseLayout<T> Add(T type, View view)
		{
			if (_views.ContainsKey(type)) return this;
			SetViewType(view, type);
			Children.Add(view);
			view.PropertyChanged += ChildOnPropertyChanged;
			_views.Add(type, view);
			return this;
		}
		public BaseLayout<T> Add(params Tuple<T, View>[] views)
		{
			foreach (var view in views)
			{
				Add(view.Item1, view.Item2);
			}
			return this;
		}

		public SizeRequest GetViewSize(VisualElement view, double width, double height = double.PositiveInfinity)
		{
			return view.Measure(width, height, MeasureFlags.IncludeMargins);
		}

		public SizeRequest GetViewSize(T type, double width, double height = double.PositiveInfinity)
		{
			return GetViewByType(type)?.Measure(width, height, MeasureFlags.IncludeMargins) ?? new SizeRequest();
		}

		public View GetViewByType(T type)
		{
			if (_views == null) return null;
			return _views.ContainsKey(type) ? _views[type] : null;
		}

		public bool ContainsView(T type)
		{
			return _views.ContainsKey(type);
		}

		protected virtual Dictionary<T, Rectangle> CalculateRects(double width, double height, Dictionary<T, View> views)
		{
			return new Dictionary<T, Rectangle>();
		}

		protected override void LayoutChildren(double x, double y, double width, double height)
		{
			Prepare(width, height);
			var viewsRects = _viewsRects;
			try
			{
				foreach (var viewPair in _views)
				{
					if (!viewsRects.ContainsKey(viewPair.Key)) continue;
					var view = _views[viewPair.Key];
					if (!view.IsVisible) continue;
					var rect = viewsRects[viewPair.Key];

					rect.X += x;
					rect.Y += y;
					if (double.IsNaN(rect.Width)) continue;
					if (double.IsNaN(rect.Height)) continue;
					LayoutChildIntoBoundingRegion(_views[viewPair.Key], rect);
				}
			}
			catch { }
		}

		protected override void OnChildAdded(Element child)
		{
			base.OnChildAdded(child);
			child.PropertyChanged += ChildOnPropertyChanged;
		}

		protected override void OnChildRemoved(Element child)
		{
			child.PropertyChanged -= ChildOnPropertyChanged;
			base.OnChildRemoved(child);
		}

		void ChildOnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == IsVisibleProperty.PropertyName)
			{
				InvalidateLayout();
			}
		}

		protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
		{
			Prepare(widthConstraint, heightConstraint);
			if (double.IsPositiveInfinity(MinWidth)) MinWidth = 1;
			if (double.IsPositiveInfinity(MinHeight)) MinHeight = 1;
			if (double.IsPositiveInfinity(MaxWidth)) MaxWidth = 1;
			if (double.IsPositiveInfinity(MaxHeight)) MaxHeight = 1;
			return new SizeRequest(new Size(MinWidth, MinHeight), new Size(MaxWidth, MaxHeight));
		}

		void Prepare(double width, double height)
		{
			if (_views == null || !_views.Any()) _views = Children.ToDictionary(GetViewType);
			_viewsRects = CalculateRects(width, height, _views);
		}
	}
}