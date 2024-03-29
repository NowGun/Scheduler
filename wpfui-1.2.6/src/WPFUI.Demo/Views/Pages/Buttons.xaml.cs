﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.Windows;
using System.Windows.Controls;

namespace WPFUI.Demo.Views.Pages;

/// <summary>
/// Interaction logic for Buttons.xaml
/// </summary>
public partial class Buttons : Page
{
    public Buttons()
    {
        InitializeComponent();
    }

    private void ButtonMore_OnClick(object sender, RoutedEventArgs e)
    {
        (Application.Current.MainWindow as Container)?.RootNavigation.Navigate("input");
    }
}
