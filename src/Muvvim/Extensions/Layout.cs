﻿using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Muvvim.Util;

namespace Muvvim.Extensions
{
    /// <summary>
    /// Provides attached properties to simply define the rows and columns of a Grid and to position child elements
    /// </summary>
    public static class Layout
    {
        /// <summary>
        /// Registers the attached property for simple row definitions
        /// </summary>
        public static DependencyProperty RowsProperty = 
            DependencyProperty.RegisterAttached("Rows", typeof(string), typeof(Layout),
                new FrameworkPropertyMetadata(GridUtil.StarUnit, new PropertyChangedCallback(OnRowsChanged)));

        /// <summary>
        /// Registers the attached property for simple column definitions
        /// </summary>
        public static DependencyProperty ColumnsProperty = 
            DependencyProperty.RegisterAttached("Columns", typeof(string), typeof(Layout),
                new FrameworkPropertyMetadata(GridUtil.StarUnit, new PropertyChangedCallback(OnColumnsChanged)));

        /// <summary>
        /// Registers the attached property for simple grid positioning
        /// </summary>
        public static DependencyProperty PositionProperty =
            DependencyProperty.RegisterAttached("Position", typeof(string), typeof(Layout),
                new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnPositionChanged)));


        public static string GetRows(Grid grid)
        {
            return (string)grid.GetValue(RowsProperty);
        }

        /// <summary>
        /// Sets the rows definition string for a grid
        /// </summary>
        /// <param name="grid">The grid</param>
        /// <param name="value">A string defining rows</param>
        public static void SetRows(Grid grid, string value)
        {
            grid.SetValue(RowsProperty, value);
        }

        public static string GetColumns(Grid grid)
        {
            return (string)grid.GetValue(ColumnsProperty);
        }

        /// <summary>
        /// Sets the columns definition string for a grid
        /// </summary>
        /// <param name="grid">The grid</param>
        /// <param name="value">A string defining columns</param>
        public static void SetColumns(Grid grid, string value)
        {
            grid.SetValue(ColumnsProperty, value);
        }

        public static string GetPosition(FrameworkElement element)
        {
            return (string)element.GetValue(PositionProperty);
        }

        /// <summary>
        /// Sets the position definition string for an element in a grid
        /// </summary>
        /// <param name="element">The element</param>
        /// <param name="value">A string defining a grid position</param>
        public static void SetPosition(FrameworkElement element, string value)
        {
            element.SetValue(PositionProperty, value);
        }

        private static void OnRowsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            // Cast the grid and the new row definition string
            Grid grid = (Grid)obj;
            string definitions = (string)args.NewValue;
            // Clear previous row definitions
            grid.RowDefinitions.Clear();
            // Parse and add a row definition for each entry
            definitions.Split(Separators.Space.AsChar(), Separators.Comma.AsChar())
                .Select(definition => GridUtil.ParseGridLength(definition))
                .Select(length => new RowDefinition() { Height = length })
                .ToList()
                .ForEach(grid.RowDefinitions.Add);
        }

        private static void OnColumnsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            // Cast the grid and the new column definition string
            Grid grid = (Grid)obj;
            string definitions = (string)args.NewValue;
            // Clear previous column definitions
            grid.ColumnDefinitions.Clear();
            // Parse and add a column definition for each entry
            definitions.Split(Separators.Space.AsChar(), Separators.Comma.AsChar())
                .Select(definition => GridUtil.ParseGridLength(definition))
                .Select(length => new ColumnDefinition() { Width = length })
                .ToList()
                .ForEach(grid.ColumnDefinitions.Add);
        }

        private static void OnPositionChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            // Parse the new position and apply it to the element
            GridPosition.Parse((string)args.NewValue).ApplyTo((FrameworkElement)obj);
        }
    }
}
