using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snappper
{
   public class ClipSizeEventArgs
   {
      public ClipSize NewClipSize { get; set; }

      public ClipSizeEventArgs(ClipSize cs)
      {
         NewClipSize = cs;
      }


   }
}
