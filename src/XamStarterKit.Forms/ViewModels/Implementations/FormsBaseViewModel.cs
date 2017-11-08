using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;
using XamStarterKit.Forms.Localization;
using XamStarterKit.Forms.ViewModels.Abstractions;
using XamStarterKit.ViewModels.Implementations;

namespace XamStarterKit.Forms.ViewModels.Implementations
{
    public class FormsBaseViewModel : BaseViewModel, IFormsViewModel
    {
        public DynamicLocalize Localize { get; }

        public FormsBaseViewModel()
        {
            Localize = new DynamicLocalize();
        }

        public virtual void FirstAppearing()
        {
        }
         
        public virtual void Appearing()
        {
        }

        public virtual void Disappering()
        {
        }

        public virtual bool BackButtonPressed()
        {
            CancellAll();
            return false;
        }

        public virtual void SizeAllocated(double width, double height)
        {
        }
    }
}