using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using AndroidX.Activity.Result;
using AndroidX.Core.App;
using AndroidX.Core.Content;
using AsyncAwaitBestPractices.MVVM;
using PickiT_MAUI;

namespace PickiTsample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }
    }

    public class MainPageViewModel : INotifyPropertyChanged
    {
        #region PropertyChanged

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion

        public PickiT? PickiT { get; set; }
        public ICommand PickVideoCommand { get; }
        public ICommand PickImageCommand { get; }
        public MainPageViewModel()
        {
            PickiT = PickiT.Get();
            PickiT.Listener.OnComplete += Listener_OnComplete;
            PickiT.Listener.OnMultipleComplete += Listener_OnMultipleComplete;
            PickiT.Listener.OnProgressUpdate += Listener_OnProgressUpdate;
            PickiT.Listener.OnStartListener += Listener_OnStartListener;
            PickiT.Listener.OnUriReturned += Listener_OnUriReturned;
            PickVideoCommand = new AsyncCommand(PickVideo);
            PickImageCommand = new AsyncCommand(PickImage);
        }

        private void Listener_OnUriReturned(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Listener_OnStartListener(object? sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Listener_OnProgressUpdate(object? sender, PickiTonProgressUpdate e)
        {
            throw new NotImplementedException();
        }

        private void Listener_OnMultipleComplete(object? sender, PickiTonMultipleCompleteListener e)
        {
            throw new NotImplementedException();
        }

        private void Listener_OnComplete(object? sender, PickiTonCompleteListener e)
        {
            if (e.WasSuccessful)
            {
                _ = App.Current.MainPage.DisplayAlert("Operation failed", e.Reason??"Unknown error", "Ok");
            }
        }

        private async Task PickVideo()
        {
            if (!await CheckSelfPermission())
            {
                _ = App.Current.MainPage.DisplayAlert("Permission check failure", "Permission has not been granted", "Ok");
                return;
            }
            FileResult? file = await MediaPicker.PickVideoAsync(new MediaPickerOptions()
            {
                Title = "Pick a picture"
            });
            PickiT.GetPath(file);
        }

        private async Task PickImage()
        {
            if (!await CheckSelfPermission())
            {
                _ = App.Current.MainPage.DisplayAlert("Permission check failure", "Permission has not been granted", "Ok");
                return;
            }
            FileResult? file = await MediaPicker.PickPhotoAsync(new MediaPickerOptions()
            {
                Title = "Pick a picture"
            });
            PickiT.GetPath(file);
        }

        private async Task<bool> CheckSelfPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                await Permissions.RequestAsync<Permissions.StorageWrite>();
                return false;
            }
            return true;
        }

        ~MainPageViewModel()
        {
            PickiT?.Dispose();
            PickiT = null;
        }
    }

}
