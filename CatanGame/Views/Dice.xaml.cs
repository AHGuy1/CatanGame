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

        Dice1Animation.IsVisible = true;
        Dice2Animation.IsVisible = true;

        // IsAnimationEnabled must be toggled to restart the animation in some versions
        Dice1Animation.IsAnimationEnabled = true;
        Dice2Animation.IsAnimationEnabled = true;

        ResultLabel.Text = "Rolling...";

        // 2. Wait for 1 second (simulating the roll time)
        await Task.Delay(1000);

        // 3. Generate Random Numbers
        int roll1 = _random.Next(1, 7); // Generates 1 to 6
        int roll2 = _random.Next(1, 7);
        int total = roll1 + roll2;

        // 4. Stop Animation and Show Result
        Dice1Animation.IsAnimationEnabled = false;
        Dice2Animation.IsAnimationEnabled = false;

        Dice1Animation.IsVisible = false;
        Dice2Animation.IsVisible = false;

        ResultLabel.Text = $"Rolled: {total}";
    }
}