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
        public int QueryVersion = 0;
        public async Task Debounce(int ms, Func<Task> func)
        {
            TimeSpan ts = TimeSpan.FromMilliseconds(ms);
            QueryVersion++;
            var currentVersion = QueryVersion;

            await Task.Delay(ts);

            if(currentVersion == QueryVersion)
            {
                await func();
            }
            
        }
    }
}
