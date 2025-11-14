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

        public string Key { get; set; }

        
        private string imagePath;
        public string ImagePath
        {
            get => imagePath;
            set
            {
                if (imagePath != value)
                {
                    imagePath = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImagePath)));
                }
            }
        }

        private bool isEnabled;
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsEnabled)));
                }
            }
        }
    }
}
