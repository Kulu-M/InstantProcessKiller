﻿using System.Windows;
using System.Windows.Input;

namespace Instant_Process_Killer
{
    /// <summary>
    /// Closes the current window.
    /// </summary>
    public class CloseWindowCommand : CommandBase<CloseWindowCommand>
    {
        public override void Execute(object parameter)
        {
            GetTaskbarWindow(parameter).Close();
            CommandManager.InvalidateRequerySuggested();
        }


        public override bool CanExecute(object parameter)
        {
            Window win = GetTaskbarWindow(parameter);
            return win != null;
        }
    }
}