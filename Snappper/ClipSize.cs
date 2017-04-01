using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Snappper
{
   [DataContract]
   public class ClipSize
   {
      public static ClipSize Current { get; set; }

      [DataMember(Name = "Title")]
      public string Title { get; set; }

      [DataMember(Name = "Clip Width")]
      public int ClipWidth { get; set; }

      [DataMember(Name = "Clip Height")]
      public int ClipHeight { get; set; }

      [DataMember(Name = "Resizable?")]
      public bool IsResizable { get; set; }

      [DataMember(Name = "Ratio Maintained?")]
      public bool IsRatioMaintained { get; set; }

      public static void LoadClipSizes(string filename)
      {
         using (var ms = new FileStream(filename, FileMode.Open))
         {
            var serializer = new DataContractJsonSerializer(typeof(List<ClipSize>));
            serializer.ReadObject(ms);
         }
      }

      [OnDeserialized()]
      internal void NewClipSizeAdded(StreamingContext context)
      {
         if (Current == null)
            Current = this;
         OnClipSizeAdded(new ClipSizeEventArgs(this));
      }

      public delegate void ClipSizeEventHandler(object sender, ClipSizeEventArgs e);

      public static event ClipSize.ClipSizeEventHandler ClipSizeAdded = delegate { };

      public static void OnClipSizeAdded(ClipSizeEventArgs e)
      {
         ClipSizeAdded(null, e);
      }

   }
}
