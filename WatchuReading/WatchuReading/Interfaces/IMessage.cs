using System;
namespace WatchuReading.Interfaces
{
    public interface IMessage
    {
        void LongAlert(string Message);
        void ShortAlert(string Message);
        void ShowSnackbar(string message);
    }
}
