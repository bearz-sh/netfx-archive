﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Runtime.InteropServices;

// ReSharper disable once CheckNamespace
namespace Bearz;

internal static partial class Interop
{
    internal static partial class Kernel32
    {
        [DllImport(Libraries.Kernel32)]
        internal static extern IntPtr GetStdHandle(int nStdHandle); // param is NOT a handle, but it returns one!
    }
}