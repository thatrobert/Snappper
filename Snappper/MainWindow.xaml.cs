using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Snappper
{

   public partial class MainWindow : Window
   {
      public bool IsToFile { get; set; }
      
      public MainWindow()
      {
         InitializeComponent();
         IsToFile = true;
         this.BorderBrush = (System.Windows.Media.SolidColorBrush)(new System.Windows.Media.BrushConverter().ConvertFrom("#F94C89"));
         toolbar.Background = this.BorderBrush;
         resizer.Background = this.BorderBrush;
         AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
         MainMenu.AddSizeMenuItems(this);
      }

      protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
      {
         if (ClipSize.Current != null && ClipSize.Current.IsRatioMaintained)
         {
            double aspect = Convert.ToDouble(ClipSize.Current.ClipWidth) / Convert.ToDouble(ClipSize.Current.ClipHeight);
            double deltaWidth = (sizeInfo.NewSize.Width - sizeInfo.PreviousSize.Width);
            double deltaHeight = (sizeInfo.NewSize.Height - sizeInfo.PreviousSize.Height);
            Debug.WriteLine("width=" + deltaWidth + ", height=" + deltaHeight);
            this.Width = sizeInfo.NewSize.Height * aspect;
            this.Height = sizeInfo.NewSize.Width / aspect;
         }
      }

      private void CaptureButton_Click(object sender, RoutedEventArgs e)
      {
         this.Visibility = Visibility.Hidden;
         this.Refresh();
         Screenshot.CopyScreen(this);
         this.Visibility = Visibility.Visible;
      }

      private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
      {
         if (e.ChangedButton == MouseButton.Left)
            this.DragMove();
      }

      private void MenuButton_Click(object sender, RoutedEventArgs e)
      {
         if (MenuButton.ContextMenu != null)
            MenuButton.ContextMenu.IsOpen = true;
         e.Handled = true;
      }

      private void MenuExit_Click(object sender, RoutedEventArgs e)
      {
         this.Close();
      }

      private void MenuToX_Clicked(object sender, RoutedEventArgs e)
      {
         var menuItem = e.OriginalSource as MenuItem;
         if (menuItem != null)
         {
            MenuToClip.IsChecked = false;
            MenuToFile.IsChecked = false;
            menuItem.IsChecked = true;
            IsToFile = MenuToFile.IsChecked;
         }
      }

      private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
      {
         MessageBox.Show(e.ExceptionObject.ToString());
         Environment.Exit(1);
      }

   }
}
