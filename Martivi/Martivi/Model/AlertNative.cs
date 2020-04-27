using System;
using System.Collections.Generic;
using System.Text;

public interface IAlertNative
{
    void ShowToast(string message);

    void ShowDialog(string v1, string message, string v2);
    void ShowDialog(string title, string message, string positive, string negative, Action<string> action);

}
