using CommunityToolkit.Mvvm.Messaging.Messages;

namespace CatanGame.Models
{
    public class AppMessage<T>(T msg) : ValueChangedMessage<T>(msg)
    {
    }
}

