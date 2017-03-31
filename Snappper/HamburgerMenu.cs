using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Snappper;
using ContextMenu = System.Windows.Controls.ContextMenu;

namespace Snappper
{
   class HamburgerMenu: ContextMenu
   {
      private MainWindow _mw;
      private int _insertIndex = 0;

      public void AddSizeMenuItems(MainWindow mw)
      {
         _mw = mw;
         ClipSize.ClipSizeAdded += AddedClipSize;
         ClipSize.LoadClipSizes(@"snappper.json");
      }

      private void AddedClipSize(object sender, ClipSizeEventArgs e)
      {
         MenuItem item = new MenuItem();
         item.Name = "mSize_" + e.NewClipSize.Title.Replace(" ", "_");
         item.Header = e.NewClipSize.Title;
         item.IsCheckable = true;
         item.Tag = e.NewClipSize;
         item.Click += menuSize_Click;
         Items.Insert(_insertIndex, item);
         if(_insertIndex == 0 && this.Items.Count > 0)
            (this.Items[0] as MenuItem).IsChecked = true;
          _insertIndex++;
      }

      private void menuSize_Click(object sender, RoutedEventArgs e)
      {
         var menuItem = e.OriginalSource as MenuItem;
         if(menuItem != null)
         {
            foreach (Control item in (menuItem.Parent as ContextMenu).Items)
            {
               if ((item is MenuItem) && (item as MenuItem).Name.StartsWith("mSize_"))
                  (item as MenuItem).IsChecked = false;
            }
            menuItem.IsChecked = true;
            ClipSize.Current = (ClipSize)menuItem.Tag;
            _mw.Width = ClipSize.Current.ClipWidth + 2;
            _mw.Height = ClipSize.Current.ClipHeight + 2;
            _mw.ResizeMode = ClipSize.Current.IsResizable ? ResizeMode.CanResizeWithGrip : ResizeMode.NoResize;
            _mw.resizer.Visibility = ClipSize.Current.IsResizable ? Visibility.Visible : Visibility.Hidden;
         }
      }


   }
}
