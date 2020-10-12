// ' ************************************************* ''
// ' DataTools Visual Basic Utility Library - Interop
// '
// ' Module: ISimpleShellItem.
// '         Represents a shell item with common properties.
// '
// ' Copyright (C) 2011-2020 Nathan Moschkin
// ' All Rights Reserved
// '
// ' Licensed Under the Microsoft Public License
// ' ************************************************* ''

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace DataTools.Desktop
{
    public interface ISimpleShellItem : INotifyPropertyChanged
    {
        FileAttributes Attributes { get; set; }
        ICollection<ISimpleShellItem> Children { get; }
        DateTime CreationTime { get; set; }
        string DisplayName { get; set; }
        ICollection<ISimpleShellItem> Folders { get; }
        System.Windows.Media.Imaging.BitmapSource IconImage { get; }
        StandardIcons IconSize { get; set; }
        bool IsFolder { get; }
        bool IsSpecial { get; }
        DateTime LastAccessTime { get; set; }
        DateTime LastWriteTime { get; set; }
        ISimpleShellItem Parent { get; }
        string ParsingName { get; }
        long Size { get; }

        void Refresh(StandardIcons? iconSize = default);
    }
}