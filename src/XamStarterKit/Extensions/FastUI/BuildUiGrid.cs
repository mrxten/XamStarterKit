using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamStarterKit.Extensions.FastUI {
    public static class BuildUiGrid {
        public static ColumnDefinitionCollection GenerateColumnDefinition(params GridLength[] definitions) {
            var columnDefinitionCollection = new ColumnDefinitionCollection();
            foreach (var gridLength in definitions) {
                columnDefinitionCollection.Add(new ColumnDefinition { Width = gridLength });
            }
            return columnDefinitionCollection;
        }

        public static RowDefinitionCollection GenerateRowDefinition(params GridLength[] definitions) {
            var rowDefinitionCollection = new RowDefinitionCollection();
            foreach (var gridLength in definitions) {
                rowDefinitionCollection.Add(new RowDefinition { Height = gridLength });
            }
            return rowDefinitionCollection;
        }

        public static T SetGridRowSpan<T>(this T bo, int value) where T : BindableObject {
            Grid.SetRowSpan(bo, value);
            return bo;
        }

        public static T SetGridColumn<T>(this T bo, int value) where T : BindableObject {
            Grid.SetColumn(bo, value);
            return bo;
        }

        public static T SetGridColumnSpan<T>(this T bo, int value) where T : BindableObject {
            Grid.SetColumnSpan(bo, value);
            return bo;
        }

        public static T SetGridRow<T>(this T bo, int value) where T : BindableObject {
            Grid.SetRow(bo, value);
            return bo;
        }

        public static Grid SetColumnDefinition(this Grid grid, params GridLength[] definitions) {
            var columnDefinitionCollection = new ColumnDefinitionCollection();
            foreach (var gridLength in definitions) {
                columnDefinitionCollection.Add(new ColumnDefinition { Width = gridLength });
            }
            grid.ColumnDefinitions = columnDefinitionCollection;
            return grid;
        }

        public static Grid SetRowDefinition(this Grid grid, params GridLength[] definitions) {
            var rowDefinitionCollection = new RowDefinitionCollection();
            foreach (var gridLength in definitions) {
                rowDefinitionCollection.Add(new RowDefinition { Height = gridLength });
            }
            grid.RowDefinitions = rowDefinitionCollection;
            return grid;
        }
    }
}
