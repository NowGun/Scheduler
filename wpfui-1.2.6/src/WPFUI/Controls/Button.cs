﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Controls;

/// <summary>
/// Inherited from the <see cref="System.Windows.Controls.Button"/>, adding <see cref="Common.SymbolRegular"/>.
/// </summary>
public class Button : System.Windows.Controls.Button, IIconControl, IAppearanceControl
{
    /// <summary>
    /// Property for <see cref="Icon"/>.
    /// </summary>
    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(nameof(Icon),
        typeof(Common.SymbolRegular), typeof(Button),
        new PropertyMetadata(Common.SymbolRegular.Empty));

    /// <summary>
    /// Property for <see cref="IconFilled"/>.
    /// </summary>
    public static readonly DependencyProperty IconFilledProperty = DependencyProperty.Register(nameof(IconFilled),
        typeof(bool), typeof(Button), new PropertyMetadata(false));

    /// <summary>
    /// Property for <see cref="IconForeground"/>.
    /// </summary>
    public static readonly DependencyProperty IconForegroundProperty = DependencyProperty.Register(nameof(IconForeground),
        typeof(Brush), typeof(Button), new FrameworkPropertyMetadata(SystemColors.ControlTextBrush,
            FrameworkPropertyMetadataOptions.Inherits));

    /// <summary>
    /// Property for <see cref="Appearance"/>.
    /// </summary>
    public static readonly DependencyProperty AppearanceProperty = DependencyProperty.Register(nameof(Appearance),
        typeof(Common.Appearance), typeof(Button),
        new PropertyMetadata(Common.Appearance.Primary));

    /// <summary>
    /// Property for <see cref="HoverBackground"/>.
    /// </summary>
    public static readonly DependencyProperty HoverBackgroundProperty = DependencyProperty.Register(nameof(HoverBackground),
        typeof(Brush), typeof(Button),
        new PropertyMetadata(Border.BackgroundProperty.DefaultMetadata.DefaultValue));

    /// <summary>
    /// Property for <see cref="HoverBorderBrush"/>.
    /// </summary>
    public static readonly DependencyProperty HoverBorderBrushProperty = DependencyProperty.Register(nameof(HoverBorderBrush),
        typeof(Brush), typeof(Button),
        new PropertyMetadata(Border.BorderBrushProperty.DefaultMetadata.DefaultValue));

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    public Common.SymbolRegular Icon
    {
        get => (Common.SymbolRegular)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    public bool IconFilled
    {
        get => (bool)GetValue(IconFilledProperty);
        set => SetValue(IconFilledProperty, value);
    }

    /// <summary>
    /// Foreground of the <see cref="WPFUI.Controls.SymbolIcon"/>.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush IconForeground
    {
        get => (Brush)GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    /// <inheritdoc />
    [Bindable(true), Category("Appearance")]
    public Common.Appearance Appearance
    {
        get => (Common.Appearance)GetValue(AppearanceProperty);
        set => SetValue(AppearanceProperty, value);
    }

    /// <summary>
    /// Background <see cref="Brush"/> when the user interacts with an element with a pointing device.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush HoverBackground
    {
        get => (Brush)GetValue(HoverBackgroundProperty);
        set => SetValue(HoverBackgroundProperty, value);
    }

    /// <summary>
    /// Border <see cref="Brush"/> when the user interacts with an element with a pointing device.
    /// </summary>
    [Bindable(true), Category("Appearance")]
    public Brush HoverBorderBrush
    {
        get => (Brush)GetValue(HoverBorderBrushProperty);
        set => SetValue(HoverBorderBrushProperty, value);
    }
}
