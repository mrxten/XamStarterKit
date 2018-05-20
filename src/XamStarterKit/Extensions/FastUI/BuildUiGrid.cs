using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamStarterKit.Extensions.FastUI
{
	public static class BuildUiGrid
	{
		public static ColumnDefinitionCollection GenerateColumnDefinition(params GridLength[] definitions)
		{
			var columnDefinitionCollection = new ColumnDefinitionCollection();
			foreach (var gridLength in definitions)
			{
				columnDefinitionCollection.Add(new ColumnDefinition { Width = gridLength });
			}
			return columnDefinitionCollection;
		}

		public static RowDefinitionCollection GenerateRowDefinition(params GridLength[] definitions)
		{
			var rowDefinitionCollection = new RowDefinitionCollection();
			foreach (var gridLength in definitions)
			{
				rowDefinitionCollection.Add(new RowDefinition { Height = gridLength });
			}
			return rowDefinitionCollection;
		}

		public static View SetGridRowSpan(this View view, int value)
		{
			Grid.SetRowSpan(view, value);
			return view;
		}

		public static View SetGridColumn(this View view, int value)
		{
			Grid.SetColumn(view, value);
			return view;
		}

		public static View SetGridColumnSpan(this View view, int value)
		{
			Grid.SetColumnSpan(view, value);
			return view;
		}

		public static View SetGridRow(this View view, int value)
		{
			Grid.SetRow(view, value);
			return view;
		}

		public static Grid SetColumnDefinition(this Grid grid, params GridLength[] definitions)
		{
			var columnDefinitionCollection = new ColumnDefinitionCollection();
			foreach (var gridLength in definitions)
			{
				columnDefinitionCollection.Add(new ColumnDefinition { Width = gridLength });
			}
			grid.ColumnDefinitions = columnDefinitionCollection;
			return grid;
		}

		public static Grid SetRowDefinition(this Grid grid, params GridLength[] definitions)
		{
			var rowDefinitionCollection = new RowDefinitionCollection();
			foreach (var gridLength in definitions)
			{
				rowDefinitionCollection.Add(new RowDefinition { Height = gridLength });
			}
			grid.RowDefinitions = rowDefinitionCollection;
			return grid;
		}
	}
}
