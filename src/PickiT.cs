#if ANDROID
using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using Android.Runtime;
using AndroidX.Activity.Result;
using AndroidX.Activity.Result.Contract;
using AndroidX.Core.App;
using Com.Hbisoft.Pickit;
using Java.Util;
using PickiT_MAUI;
using Application = Microsoft.Maui.Controls.Application;
using Environment = Android.OS.Environment;
using Object = Java.Lang.Object;

namespace PickiT_MAUI
{
    public class ActivityResultCallback<T> : Java.Lang.Object, IActivityResultCallback where T : Object
    {
        private readonly Action<T?> onActivityResultAction;
        public ActivityResultCallback(Action<T?> OnActivityResultAction)
        {
            onActivityResultAction = OnActivityResultAction;
        }
        public void OnActivityResult(Object? result)
        {
            this.onActivityResultAction.Invoke(result as T);
        }
    }

    [Microsoft.Maui.Controls.Internals.Preserve(AllMembers = true)]
    public partial class PickiT : Com.Hbisoft.Pickit.PickiT, IDisposable
    {
        public PickiTCallbacks? Listener { get; private set; }
        protected PickiT(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer)
        {
        }

        public PickiT(PickiTCallbacks listener, Context? context = null, Activity? activity = null) : base(context ?? Microsoft.Maui.ApplicationModel.Platform.CurrentActivity, listener, activity ?? Microsoft.Maui.ApplicationModel.Platform.CurrentActivity)
        {
            Listener = listener;
        }

        public static PickiT Get()
        {
            return new PickiT(new PickiTCallbacks());
        }

        public void PickVideo()
        {
            Intent intent;
            if (Environment.ExternalStorageState.Equals(Environment.MediaMounted))
            {
                intent = new Intent(Intent.ActionPick, MediaStore.Video.Media.ExternalContentUri);
            }
            else
            {
                intent = new Intent(Intent.ActionPick, MediaStore.Video.Media.InternalContentUri);
            }
            //  In this example we will set the type to video
            intent.SetType("video/*");
            intent.SetAction(Intent.ActionGetContent);
            intent.PutExtra("return-data", true);
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.JellyBeanMr2)
            {
                intent.PutExtra(Intent.ExtraAllowMultiple, true);
            }
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);

            ActivityResultLauncher activityResultLauncher =
                ((AndroidX.Activity.ComponentActivity)Microsoft.Maui.ApplicationModel.Platform.CurrentActivity)
                .RegisterForActivityResult(new ActivityResultContracts.StartActivityForResult(), new ActivityResultCallback<ActivityResult>(
                    (result) =>
                    {
                        if (result.ResultCode == (int)Android.App.Result.Ok)
                        {
                            Intent data = result.Data;
                            //  Get path from PickiT (The path will be returned in PickiTonCompleteListener)
                            //
                            //  If the selected file is from Dropbox/Google Drive or OnDrive:
                            //  Then it will be "copied" to your app directory (see path example below) and when done the path will be returned in PickiTonCompleteListener
                            //  /storage/emulated/0/Android/data/your.package.name/files/Temp/tempDriveFile.mp4
                            //
                            //  else the path will directly be returned in PickiTonCompleteListener

                            ClipData clipData = Objects.RequireNonNull(data).getClipData();
                            if (clipData != null)
                            {
                                int numberOfFilesSelected = clipData.getItemCount();
                                if (numberOfFilesSelected > 1)
                                {
                                    pickiT.getMultiplePaths(clipData);
                                    StringBuilder allPaths = new StringBuilder("Multiple Files Selected:" + "\n");
                                    for (int i = 0; i < clipData.getItemCount(); i++)
                                    {
                                        allPaths.append("\n\n").append(clipData.getItemAt(i).getUri());
                                    }
                                    originalTv.setText(allPaths.toString());
                                }
                                else
                                {
                                    pickiT.getPath(clipData.getItemAt(0).getUri(), Build.VERSION.SDK_INT);
                                    originalTv.setText(String.valueOf(clipData.getItemAt(0).getUri()));
                                }
                            }
                            else
                            {
                                pickiT.getPath(data.getData(), Build.VERSION.SDK_INT);
                                originalTv.setText(String.valueOf(data.getData()));
                            }

                        }

                    }));

            activityResultLauncher.Launch(intent);
        }


        public new void Dispose()
        {
            DeleteTemporaryFile(Microsoft.Maui.ApplicationModel.Platform.CurrentActivity);
            Listener?.Dispose();
            Listener = null;
            base.Dispose();
        }
    }
}
#endif
