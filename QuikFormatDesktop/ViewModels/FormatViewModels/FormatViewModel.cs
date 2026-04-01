using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace QuikFormatDesktop.ViewModels.FormatViewModels
{
    public class FormatViewModel : ViewModelBase
    {
        public FormatViewModel(SelectorCardViewModel selectorCardViewModel, PreviewViewModel previewViewModel)
        {
            SelectorCardViewModel = selectorCardViewModel;
            PreviewViewModel = previewViewModel;

            SelectorCardViewModel.PropertyChanged += OnSelectorChanged;
            SelectorCardViewModel.DocumentChanged += PreviewViewModel.OnDocumentChanged;
        }

        public SelectorCardViewModel SelectorCardViewModel { get; }
        public PreviewViewModel PreviewViewModel { get; }
        public bool IsPreviewVisible => SelectorCardViewModel.IsPreviewVisible;

        private void OnSelectorChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(SelectorCardViewModel.IsPreviewVisible))
            {
                OnPropertyChanged(nameof(IsPreviewVisible));
            }
        }
    }
}
