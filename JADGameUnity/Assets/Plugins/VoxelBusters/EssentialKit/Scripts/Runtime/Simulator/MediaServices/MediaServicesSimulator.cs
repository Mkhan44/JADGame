using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;
using VoxelBusters.CoreLibrary.Editor.NativePlugins;

namespace VoxelBusters.EssentialKit.MediaServicesCore.Simulator
{
    public sealed class MediaServicesSimulator : SingletonObject<MediaServicesSimulator>
    {
        #region Constants

        // messages
        private     const       string              kGalleryUnauthorizedAccessError     = "Unauthorized access! Check permission before accessing gallery.";

        private     const       string              kGalleryAuthorizedError             = "Permission for accessing gallery is already provided. Please check access status before requesting access.";

        private     const       string              kCameraUnauthorizedAccessError      = "Unauthorized access! Check permission before accessing camera.";
        
        private     const       string              kCameraAuthorizedError              = "Permission for accessing gallery is already provided. Please check access status before requesting access.";

        #endregion

        #region Fields

        private     MediaServicesSimulatorData       m_simulatorData;

        #endregion

        #region Constructors

        private MediaServicesSimulator()
        {
            // initialise
            m_simulatorData  = LoadFromDisk() ?? new MediaServicesSimulatorData();
        }

        #endregion

        #region Database methods

        private MediaServicesSimulatorData LoadFromDisk()
        {
            return SimulatorDatabase.Instance.GetObject<MediaServicesSimulatorData>(NativeFeatureType.kMediaServices);
        }

        private void SaveData()
        {
            SimulatorDatabase.Instance.SetObject(NativeFeatureType.kMediaServices, m_simulatorData);
        }

        public static void Reset() 
        {
            SimulatorDatabase.Instance.RemoveObject(NativeFeatureType.kMediaServices);
        }

        #endregion

        #region Public methods

        public void RequestGalleryAccess(Action<GalleryAccessStatus, Error> callback)
        {
            // check whether required permission is already granted
            var     accessStatus    = GetGalleryAccessStatus();
            if (GalleryAccessStatus.Authorized == accessStatus)
            {
                callback(GalleryAccessStatus.Authorized, new Error(description: kGalleryAuthorizedError));
            }
            else
            {
                // show prompt to user asking for required permission
                var     applicationSettings = EssentialKitSettings.Instance.ApplicationSettings;
                var     usagePermission     = applicationSettings.UsagePermissionSettings.GalleryUsagePermission;

                var     newAlertDialog      = new AlertDialogBuilder()
                    .SetTitle("Gallery Simulator")
                    .SetMessage(usagePermission.GetDescriptionForActivePlatform())
                    .AddButton("Authorise", () => 
                    { 
                        // save selection
                        UpdateGalleryAccessStatus(GalleryAccessStatus.Authorized);

                        // send result
                        callback(GalleryAccessStatus.Authorized, null);
                    })
                    .AddCancelButton("Cancel", () => 
                    { 
                        // save selection
                        UpdateGalleryAccessStatus(GalleryAccessStatus.Denied);

                        // send result
                        callback(GalleryAccessStatus.Denied, null);
                    })
                    .Build();
                newAlertDialog.Show();
            }
        }

        public GalleryAccessStatus GetGalleryAccessStatus()
        {
            return m_simulatorData.GalleryAccessStatus;
        }

        public void RequestCameraAccess(Action<CameraAccessStatus, Error> callback)
        {
            // check whether required permission is already granted
            var     accessStatus    = GetCameraAccessStatus();
            if (CameraAccessStatus.Authorized == accessStatus)
            {
                callback(CameraAccessStatus.Authorized, new Error(description: kCameraAuthorizedError));
            }
            else
            {
                // show prompt to user asking for required permission
                var     applicationSettings = EssentialKitSettings.Instance.ApplicationSettings;
                var     usagePermission     = applicationSettings.UsagePermissionSettings.CameraUsagePermission;

                var     newAlertDialog      = new AlertDialogBuilder()
                    .SetTitle("Gallery Simulator")
                    .SetMessage(usagePermission.GetDescriptionForActivePlatform())
                    .AddButton("Authorise", () => 
                    { 
                        // save selection
                        UpdateCameraAccessStatus(CameraAccessStatus.Authorized);

                        // send result
                        callback(CameraAccessStatus.Authorized, null);
                    })
                    .AddCancelButton("Cancel", () => 
                    { 
                        // save selection
                        UpdateCameraAccessStatus(CameraAccessStatus.Denied);

                        // send result
                        callback(CameraAccessStatus.Denied, null);
                    })
                    .Build();
                newAlertDialog.Show();
            }
        }
        
        public CameraAccessStatus GetCameraAccessStatus()
        {
            return m_simulatorData.CameraAccessStatus;
        }

        public bool CanSelectImageFromGallery()
        {
            return (GalleryAccessStatus.Authorized == GetGalleryAccessStatus());
        }

        public void SelectImageFromGallery(Action<byte[], Error> callback)
        {
            var     accessStatus    = GetGalleryAccessStatus();
            if (GalleryAccessStatus.Authorized == accessStatus)
            {
                string  imagePath   = EditorUtility.OpenFilePanelWithFilters("Select an image", "", new string[] { "Image files", "png,jpg,jpeg"});
                if (imagePath.Length != 0)
                {
                    callback(IOServices.ReadFileData(imagePath), null);
                }
                else
                {
                    callback(null, new Error(description: "User cancelled operation."));
                }
            }
            else
            {
                callback(null, new Error(description: kGalleryUnauthorizedAccessError));
            }
        }

        public bool CanCaptureImageFromCamera()
        {
            return (CameraAccessStatus.Authorized == GetCameraAccessStatus());
        }

        public void CaptureImageFromCamera(Action<byte[], Error> callback)
        {
            var     accessStatus    = GetCameraAccessStatus();
            if (CameraAccessStatus.Authorized == accessStatus)
            {
                var     image       = SimulatorDatabase.Instance.GetRandomImage();
                callback(image.EncodeToPNG(), null);
            }
            else
            {
                callback(null, new Error(description: kCameraUnauthorizedAccessError));
            }
        }

        public bool CanSaveImageToGallery()
        {
            return (GalleryAccessStatus.Authorized == GetGalleryAccessStatus());
        }

        public void SaveImageToGallery(Texture2D image, Action<bool, Error> callback)
        {
            var     accessStatus    = GetGalleryAccessStatus();
            if (GalleryAccessStatus.Authorized == accessStatus)
            {
                string  selectedPath    = EditorUtility.SaveFilePanel("Save image", "", image.name + ".png", "png");
                if (selectedPath.Length != 0)
                {
                    var     rawData     = image.EncodeToPNG();
                    if (rawData != null)
                    {
                        IOServices.CreateFile(selectedPath, rawData);

                        callback(true, null);
                        return;
                    }
                }

                callback(false, new Error(description: "User cancelled operation."));
            }
            else
            {
                callback(false, new Error(description: kGalleryUnauthorizedAccessError));
            }
        }

        #endregion

        #region Private methods

        private void UpdateGalleryAccessStatus(GalleryAccessStatus value)
        {
            m_simulatorData.GalleryAccessStatus = value;

            SaveData();
        }

        private void UpdateCameraAccessStatus(CameraAccessStatus value)
        {
            m_simulatorData.CameraAccessStatus  = value;

            SaveData();
        }

        #endregion
    }
}