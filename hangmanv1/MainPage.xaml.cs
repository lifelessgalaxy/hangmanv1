using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hangmanv1
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private const int MaxWrong = 6;

        private string gameStatus;
        public string GameStatus
        {
            get => gameStatus;
            set { gameStatus = value; OnPropertyChanged(); }
        }

        private string message;
        public string Message
        {
            get => message;
            set { message = value; OnPropertyChanged(); }
        }

        private string spotlight;
        public string Spotlight
        {
            get => spotlight;
            set { spotlight = value; OnPropertyChanged(); }
        }

        private string currentImage;
        public string CurrentImage
        {
            get => currentImage;
            set { currentImage = value; OnPropertyChanged(); }
        }

        private int lettersNeeded;
        public int LettersNeeded
        {
            get => lettersNeeded;
            set
            {
                lettersNeeded = value;
                LettersNeededText = $"Letters needed: {lettersNeeded}";
                OnPropertyChanged();
            }
        }

        private string lettersNeededText = "";
        public string LettersNeededText
        {
            get => lettersNeededText;
            set { lettersNeededText = value; OnPropertyChanged(); }
        }

        private string answer = string.Empty;
        private readonly HashSet<char> guessed = new();
        private int mistakes = 0;

        public ObservableCollection<LetterModel> Letters { get; } = new();

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            PopulateLettersUsingImageFiles();
            StartGame();
        }

        void PopulateLettersUsingImageFiles()
        {
            Letters.Clear();
            for (char c = 'a'; c <= 'z'; c++)
            {
                Letters.Add(new LetterModel
                {
                    Key = c.ToString(),
                    ImagePath = $"{c}.png",
                    IsEnabled = true
                });
            }
        }

        void StartGame()
        {
            string[] words = { "people", "sunset", "banana", "monkey", "bridge", "dragon" };
            var rnd = new Random();
            answer = words[rnd.Next(words.Length)].ToLowerInvariant();

            guessed.Clear();
            mistakes = 0;

            lettersNeededText = $"Letters needed: {answer.Length}";
            OnPropertyChanged(nameof(LettersNeededText));

            CurrentImage = $"img{mistakes}.jgp";
            Spotlight = CreateSpotlight();
            GameStatus = "Guess the word!";
            Message = "Tap a letter image to guess.";

            EnableAllLetters();
        }

        string CreateSpotlight()
        {
            return string.Join(" ", answer.Select(ch => guessed.Contains(ch) ? char.ToUpper(ch).ToString() : "_"));
        }

        void LetterImageButton_Clicked(object sender, EventArgs e)
        {
            if (sender is not ImageButton btn) return;
            if (btn.CommandParameter is null) return;

            var key = btn.CommandParameter.ToString();
            if (string.IsNullOrEmpty(key)) return;

            char letter = char.ToLowerInvariant(key[0]);

            var model = Letters.FirstOrDefault(l => l.Key.Equals(key, StringComparison.OrdinalIgnoreCase));
            if (model != null)
                model.IsEnabled = false;

            if (guessed.Contains(letter)) return;

            if (!answer.Contains(letter))
            {
                mistakes++;

                if (mistakes >= MaxWrong)
                {
                    CurrentImage = "death.gif";
                    Message = $"You lost! The word was: {answer.ToUpper()}";
                    GameStatus = "Game over";
                    DisableAllLetters();
                    return;
                }
                else
                {
                    CurrentImage = $"img{mistakes}.jpg";
                    Message = $"Wrong! {MaxWrong - mistakes} tries left.";
                }
            }
            else
            {
                guessed.Add(letter);
                lettersNeededText = $"Remaining letters: {answer.Count(c => !guessed.Contains(c))}";
                OnPropertyChanged(nameof(LettersNeededText));
                Spotlight = CreateSpotlight();

                if (!Spotlight.Contains("_"))
                {
                    Message = "You won! Nice job.";
                    GameStatus = "Victory!";
                    DisableAllLetters();
                    return;
                }
                else
                {
                    Message = "Good guess!";
                }
            }
        }

        void DisableAllLetters()
        {
            foreach (var lm in Letters)
                lm.IsEnabled = false;
        }

        void EnableAllLetters()
        {
            foreach (var lm in Letters)
                lm.IsEnabled = true;
        }

        void Reset_Clicked(object sender, EventArgs e)
        {
            StartGame();
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        } 
    }
}
