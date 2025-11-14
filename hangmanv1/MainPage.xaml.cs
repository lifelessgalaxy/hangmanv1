using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace hangmanv1
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string gameStatus;
        public string GameStatus
        {
            get => gameStatus;
            set
            {
                gameStatus = value;
                OnPropertyChanged();
            }
        }

        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                OnPropertyChanged();
            }
        }

        private string spotlight;
        public string Spotlight
        {
            get => spotlight;
            set
            {
                spotlight = value;
                OnPropertyChanged();
            }
        }

        private string currentImage;
        public string CurrentImage
        {
            get => currentImage;
            set
            {
                currentImage = value;
                OnPropertyChanged();
            }
        }

        private string lettersNeededText;
        public string LettersNeededText
        {
            get => lettersNeededText;
            set
            {
                lettersNeededText = value;
                OnPropertyChanged();
            }
        }

        private string answer;
        private List<char> guessed;
        private int mistakes;
        private int maxWrong = 6;

        public List<LetterModel> Letters { get; set; }

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
            InitializeLetters();
            StartGame();
        }

        private void InitializeLetters()
        {
            Letters = new List<LetterModel>();
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

        private void StartGame()
        {
            var wordList = new List<(string word, string category)>
        {
            ("people", "General"),
            ("sunset", "Nature"),
            ("banana", "Fruit"),
            ("monkey", "Animal"),
            ("bridge", "Structure"),
            ("dragon", "Mythical")
        };

            var rand = new Random();
            var selection = wordList[rand.Next(wordList.Count)];
            answer = selection.word.ToLower();

            guessed = new List<char>();
            mistakes = 0;

            CurrentImage = $"img{mistakes}.png";
            Spotlight = CreateSpotlight();
            LettersNeededText = $"Letters remaining: {answer.Length}";
            GameStatus = $"Category: {selection.category}";
            Message = "";

            foreach (var letter in Letters)
            {
                letter.IsEnabled = true;
                letter.ImagePath = $"{letter.Key}.png";
            }
        }

        private string CreateSpotlight()
        {
            var display = string.Join(" ", answer.Select(c => guessed.Contains(c) ? c : '_'));
            LettersNeededText = $"Letters remaining: {display.Count(c => c == '_')}";
            return display;
        }

        private void LetterImageButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            var letterModel = button.BindingContext as LetterModel;

            if (!letterModel.IsEnabled) return;
            letterModel.IsEnabled = false;
            string letter = letterModel.Key;

            if (answer.Contains(letter))
            {
                guessed.Add(letter[0]);
                Spotlight = CreateSpotlight();

                if (!Spotlight.Contains("_"))
                {
                    Message = $"You won! The word was '{answer}'";
                    DisableAllLetters();
                }
            }
            else
            {
                letterModel.ImagePath = "letter_wrong.png";
                mistakes++;

                if (mistakes >= maxWrong)
                {
                    CurrentImage = "lose_animation.gif";
                    Message = $"You lost! The word was '{answer}'";
                    DisableAllLetters();
                }
                else
                {
                    CurrentImage = $"img{mistakes}.png";
                }
            }
        }

        private void DisableAllLetters()
        {
            foreach (var letter in Letters)
                letter.IsEnabled = false;
        }

        private void Reset_Clicked(object sender, EventArgs e)
        {
            StartGame();
        }

        private void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
