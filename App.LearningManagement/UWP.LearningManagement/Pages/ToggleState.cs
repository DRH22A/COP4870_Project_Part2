using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP.LearningManagement.Pages
{
    public static class Toggle_State
    {
        private static bool isChecked = false;

        public static bool IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }
    }
}
