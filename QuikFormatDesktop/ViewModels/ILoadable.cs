using QuikFormatDesktop.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace QuikFormatDesktop.ViewModels
{
    public interface ILoadable
    {
        void Load(object parametr, bool isEdit = false);
    }
}
