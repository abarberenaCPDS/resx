 
 


using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace ServiceModelEx
{
   /// <summary>
   /// Provides thread-safe access way to set the Text property
   /// </summary>
   [ToolboxBitmap(typeof(SafeTextBox),"SafeTextbox.bmp")]
   public class SafeTextBox : TextBox
   {
      SynchronizationContext m_SynchronizationContext = SynchronizationContext.Current;

      override public string Text
      {
         set
         {
            SendOrPostCallback setText = delegate(object text)
                                         {
                                            base.Text = text as string;
                                         };
            try
            {
               m_SynchronizationContext.Send(setText,value);
            }
            catch
            {}
         }
         get
         {
            string text = String.Empty;
            SendOrPostCallback getText = delegate
                                         {
                                            text = base.Text;
                                         };
            try
            {
               m_SynchronizationContext.Send(getText,null);
            }
            catch
            {}
            return text;
         }
      }
   }
}
