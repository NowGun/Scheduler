﻿// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WPFUI.Controls.Interfaces;

namespace WPFUI.Demo.Views.Pages;

// This page still causes memory leaking, need improvement in Navigation service, VirtualizingUniformGrid or somewhere else...

public struct DisplayableIcon
{
    public int ID { get; set; }

    public string Name { get; set; }

    public string Code { get; set; }

    public string Symbol { get; set; }
    public WPFUI.Common.SymbolRegular Icon { get; set; }
}

public class IconsPageData : WPFUI.Common.ViewData
{
    private List<DisplayableIcon> _iconsCollection = new List<DisplayableIcon>();
    public List<DisplayableIcon> IconsCollection
    {
        get => _iconsCollection;
        set => UpdateProperty(ref _iconsCollection, value, nameof(IconsCollection));
    }

    private WPFUI.Common.SymbolRegular _selectedSymbol = WPFUI.Common.SymbolRegular.Empty;
    public WPFUI.Common.SymbolRegular SelectedSymbol
    {
        get => _selectedSymbol;
        set => UpdateProperty(ref _selectedSymbol, value, nameof(SelectedSymbol));
    }

    private string _selectedSymbolName = String.Empty;
    public string SelectedSymbolName
    {
        get => _selectedSymbolName;
        set => UpdateProperty(ref _selectedSymbolName, value, nameof(SelectedSymbolName));
    }

    private string _selectedSymbolCharacter = String.Empty;
    public string SelectedSymbolCharacter
    {
        get => _selectedSymbolCharacter;
        set => UpdateProperty(ref _selectedSymbolCharacter, value, nameof(SelectedSymbolCharacter));
    }

    private string _codeBlock = String.Empty;
    public string CodeBlock
    {
        get => _codeBlock;
        set => UpdateProperty(ref _codeBlock, value, nameof(CodeBlock));
    }
}

/// <summary>
/// Interaction logic for Icons.xaml
/// </summary>
public partial class Icons : Page, INavigable
{
    protected bool _iconsInitialized = false;

    protected DisplayableIcon _activeGlyph;

    protected IconsPageData _data;



    public Icons()
    {
        InitializeComponent();
    }

    public async void OnNavigationRequest(INavigation sender, object current)
    {
        if (!_iconsInitialized)
            await InitializeIcons();
    }

    private async Task InitializeIcons()
    {
        _data = new IconsPageData();
        DataContext = _data;

        await Application.Current.Dispatcher.InvokeAsync(async () =>
        {
            _data.IconsCollection = await PrepareIconsCollection();

            if (_data.IconsCollection.Count() <= 4)
                return;

            System.Diagnostics.Debug.WriteLine($"DEBUG | Icons try to display {_data.IconsCollection.Count} FrameworkElement's at once.");

            UpdateSymbolData(4);
        });

        _iconsInitialized = true;
    }

    private async Task<List<DisplayableIcon>> PrepareIconsCollection()
    {
        var icons = new List<DisplayableIcon>();

        await Task.Run(() =>
        {
            var id = 0;
            var names = Enum.GetNames(typeof(Common.SymbolRegular));

            names = names.OrderBy(n => n).ToArray();

            foreach (string iconName in names)
            {
                var icon = Common.Glyph.Parse(iconName);

                icons.Add(new DisplayableIcon
                {
                    ID = id++,
                    Name = iconName,
                    Icon = icon,
                    Symbol = ((char)icon).ToString(),
                    Code = ((int)icon).ToString("X4")
                });
            }
        });

        return icons;
    }

    private void UpdateSymbolData(int symbolId)
    {
        _data.SelectedSymbol = _data.IconsCollection[symbolId].Icon;
        _data.SelectedSymbolCharacter = "\\u" + _data.IconsCollection[symbolId].Code;
        _data.SelectedSymbolName = _data.IconsCollection[symbolId].Name;
        _data.CodeBlock = "<wpfui:SymbolIcon Symbol=\"" + _data.IconsCollection[symbolId].Name + "\"/>";
    }

    private void IconButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (sender is not Button button)
            return;

        var id = Int32.Parse(button.Tag?.ToString() ?? String.Empty);

        UpdateSymbolData(id);
    }
}
