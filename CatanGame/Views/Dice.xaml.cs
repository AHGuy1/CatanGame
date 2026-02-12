namespace CatanGame.Views;

public partial class Dice : ContentPage
{
	public Dice()
	{
		InitializeComponent();
	}
    private Random _random = new Random();

    private async void OnRollClicked(object sender, EventArgs e)
    {
        Dice1Result.IsVisible = false;
        Dice2Result.IsVisible = false;
        Dice1Animation.Progress = TimeSpan.Zero;
        Dice2Animation.Progress = TimeSpan.Zero;
        Dice1Animation.IsVisible = true;
        Dice2Animation.IsVisible = true;
        Dice1Animation.IsAnimationEnabled = true;
        Dice2Animation.IsAnimationEnabled = true;
        ResultLabel.Text = "Rolling...";
        await Task.Delay(2200);
        int roll1 = _random.Next(1, 7);
        int roll2 = _random.Next(1, 7);
        int total = roll1 + roll2;
        Dice1Animation.IsVisible = false;
        Dice1Animation.IsAnimationEnabled = false;
        Dice1Result.Source = roll1 == 1 ? "dice" + "one" : 
            roll1 == 2 ? "dice" + "two" : 
            roll1 == 3 ? "dice" + "three" : 
            roll1 == 4 ? "dice" + "four" :
            roll1 == 5 ? "dice" + "five" : 
            "dice" + "six";
        Dice1Result.IsVisible = true;
        Dice2Animation.IsAnimationEnabled = false;
        Dice2Result.Source = roll2 == 1 ? "dice" + "one" :
            roll2 == 2 ? "dice" + "two" :
            roll2 == 3 ? "dice" + "three" :
            roll2 == 4 ? "dice" + "four" :
            roll2 == 5 ? "dice" + "five" :
            "dice" + "six"; 
        Dice2Result.IsVisible = true;
        Dice2Animation.IsVisible = false;
        ResultLabel.Text = $"Rolled: {total}";
    }
}