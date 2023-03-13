using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace StudentsVisitationsWPF.Code
{
    public class DebounceMethods
    {
        public static async Task<bool> DebounceTextBox(int ms, TextBox textBox, string text)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(ms);
            await Task.Delay(ts);
            if(textBox.Text==text)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
