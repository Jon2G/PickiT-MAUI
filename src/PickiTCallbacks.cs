#if ANDROID
using Com.Hbisoft.Pickit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Runtime;
using Java.Interop;

namespace PickiT_MAUI
{
    public class PickiTCallbacks : Java.Lang.Object, Com.Hbisoft.Pickit.IPickiTCallbacks
    {
        public event EventHandler<PickiTonCompleteListener>? OnComplete;
        public event EventHandler<PickiTonMultipleCompleteListener>? OnMultipleComplete;
        public event EventHandler<PickiTonProgressUpdate>? OnProgressUpdate;
        public event EventHandler? OnStartListener;
        public event EventHandler? OnUriReturned;

        public virtual void PickiTonCompleteListener(string? path, bool wasDriveFile, bool wasUnknownProvider, bool wasSuccessful, string? reason)
        {
            OnComplete?.Invoke(this, new PickiTonCompleteListener(path, wasDriveFile, wasUnknownProvider, wasSuccessful, reason));
        }

        public virtual void PickiTonMultipleCompleteListener(IList<string>? paths, bool wasSuccessful, string? reason)
        {
            OnMultipleComplete?.Invoke(this, new PickiTonMultipleCompleteListener(paths, wasSuccessful, reason));
        }

        public virtual void PickiTonProgressUpdate(int progress)
        {
            OnProgressUpdate?.Invoke(this, new PickiTonProgressUpdate(progress));
        }

        public virtual void PickiTonStartListener()
        {
            OnStartListener?.Invoke(this, EventArgs.Empty);
        }

        public virtual void PickiTonUriReturned()
        {
            OnUriReturned?.Invoke(this, EventArgs.Empty);
        }

        public new void Dispose()
        {
            OnComplete = null;
            OnMultipleComplete = null;
            OnProgressUpdate = null;
            OnUriReturned = null;
            OnStartListener = null;
            base.Dispose();
        }
    }
}
#endif