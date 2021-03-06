﻿using System;
using System.Windows;
using System.Windows.Threading;

namespace Snappper
{
  public static class ExtensionMethods
    {
        private static readonly Action EmptyDelegate = delegate () { };

        public static void Refresh(this UIElement uiElement)
        {
            uiElement.Dispatcher.Invoke(DispatcherPriority.Render, EmptyDelegate);
        }
    }
}
