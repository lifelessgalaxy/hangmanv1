using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace hangmanv1
{
    public class LetterModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Key { get; set; } = string.Empty;         
        public string ImagePath { get; set; } = string.Empty;   

        private bool isEnabled = true;
        public bool IsEnabled
        {
            get => isEnabled;
            set { isEnabled = value; OnPropertyChanged(); }
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
            
    }
}
