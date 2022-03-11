using GenShin_Launcher_Plus.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenShin_Launcher_Plus.ViewModels
{
    public class GuidePageViewModel : ObservableObject
    {
        public LanguagesModel languages { get => MainBase.lang; }
    }
}
