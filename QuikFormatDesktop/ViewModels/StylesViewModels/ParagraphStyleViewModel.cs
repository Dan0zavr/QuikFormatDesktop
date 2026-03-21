using QuikFormatDesktop.ViewModels.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using QuikFormatDesktop.ViewModels.Enums;
using Microsoft.Extensions.Options;
using QuikFormatDesktop.Models.SupportModels;
using QuikFormatDesktop.ViewModels.Commands.TextViewModelCommands.ParagraphStyleCommands;

namespace QuikFormatDesktop.ViewModels.StylesViewModels
{
    public class ParagraphStyleViewModel : ViewModelBase
    {
        public IDialogService dialogService;
        public readonly ParagraphService paragraphService;
        public readonly AlignmentService alignmentService;

        private string _paragraphStyleName;
        private HorizontalAlignmentType _selectedAlignment;
        private double _firstLineIndent;
        private double _leftIndent;
        private double _rightIndent;
        private double _selectedInterval;
        private double _beforeInterval;
        private double _afterInterval;
        private bool _contextualSpacing = false;
        private string _pStatusMessage;

        private List<double> _intervals;

        public ParagraphStyleViewModel(IOptions<ParagraphSettings> options, ParagraphService DiParagraphService, AlignmentService DiAlignmentService, IDialogService DiDialogService)
        {
            dialogService = DiDialogService;
            paragraphService = DiParagraphService;
            alignmentService = DiAlignmentService;
            Intervals = options.Value.AllowedIntervals;
            SelectedAlignment = HorizontalAlignmentType.Both;
            SelectedInterval = options.Value.DefaultInterval;

            AddParagraphCommand = new AddParagraphStyleCommand(this);
        }

        public ICommand TextDeleteCommand;
        public ICommand AddParagraphCommand { get; }

        public string ParagraphStyleName
        {
            get => _paragraphStyleName;
            set
            {
                _paragraphStyleName = value;
                OnPropertyChanged(nameof(ParagraphStyleName));
                (AddParagraphCommand as AddParagraphStyleCommand)?.RaiseCanExecuteChanged();
            }
        }

        public HorizontalAlignmentType SelectedAlignment
        {
            get => _selectedAlignment;
            set
            {
                _selectedAlignment = value;
                OnPropertyChanged(nameof(SelectedAlignment));
            }
        }

        public double FirstLineIndent
        {
            get => _firstLineIndent;
            set
            {
                _firstLineIndent = value;
                OnPropertyChanged(nameof(FirstLineIndent));
                (AddParagraphCommand as AddParagraphStyleCommand)?.RaiseCanExecuteChanged();
            }
        }

        public double LeftIndent
        {
            get => _leftIndent;
            set
            {
                _leftIndent = value;
                OnPropertyChanged(nameof(LeftIndent));
                (AddParagraphCommand as AddParagraphStyleCommand)?.RaiseCanExecuteChanged();
            }
        }

        public double RightIndent
        {
            get => _rightIndent;
            set
            {
                _rightIndent = value;
                OnPropertyChanged(nameof(RightIndent));
                (AddParagraphCommand as AddParagraphStyleCommand)?.RaiseCanExecuteChanged();
            }
        }

        public double SelectedInterval
        {
            get => _selectedInterval;
            set
            {
                _selectedInterval = value;
                OnPropertyChanged(nameof(SelectedInterval));
            }
        }

        public double BeforeInterval
        {
            get => _beforeInterval;
            set
            {
                _beforeInterval = value;
                OnPropertyChanged(nameof(BeforeInterval));
                (AddParagraphCommand as AddParagraphStyleCommand)?.RaiseCanExecuteChanged();
            }
        }

        public double AfterInterval
        {
            get => _afterInterval;
            set
            {
                _afterInterval = value;
                OnPropertyChanged(nameof(AfterInterval));
                (AddParagraphCommand as AddParagraphStyleCommand)?.RaiseCanExecuteChanged();
            }
        }

        public bool ContextualSpacing
        {
            get => _contextualSpacing;
            set
            {
                _contextualSpacing = value;
                OnPropertyChanged(nameof(ContextualSpacing));
            }
        }

        public string PStatusMessage
        {
            get => _pStatusMessage;
            set
            {
                _pStatusMessage = value;
                OnPropertyChanged(nameof(PStatusMessage));
            }
        }

        public List<double> Intervals
        {
            get => _intervals;
            set
            {
                _intervals = value;
                OnPropertyChanged(nameof(Intervals));
            }
        }
    }
}

