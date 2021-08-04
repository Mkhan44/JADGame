using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using VoxelBusters.CoreLibrary;
using VoxelBusters.CoreLibrary.NativePlugins;

namespace VoxelBusters.EssentialKit.GameServicesCore
{
    public abstract class LeaderboardBase : NativeObjectBase, ILeaderboard
    {
        #region Constructors

        protected LeaderboardBase(string id, string platformId)
        {
            // set properties
            Id              = id;
            PlatformId      = platformId;
        }

        #endregion

        #region Abstract methods

        protected abstract string GetTitleInternal();

        protected abstract LeaderboardPlayerScope GetPlayerScopeInternal();

        protected abstract void SetPlayerScopeInternal(LeaderboardPlayerScope value);

        protected abstract LeaderboardTimeScope GetTimeScopeInternal();
        
        protected abstract void SetTimeScopeInternal(LeaderboardTimeScope value);

        protected abstract IScore GetLocalPlayerScoreInternal();

        protected abstract void LoadTopScoresInternal(LoadScoresInternalCallback callback);

        protected abstract void LoadPlayerCenteredScoresInternal(LoadScoresInternalCallback callback);

        protected abstract void LoadNextInternal(LoadScoresInternalCallback callback);

        protected abstract void LoadPreviousInternal(LoadScoresInternalCallback callback);
        
        protected abstract void LoadImageInternal(LoadImageInternalCallback callback);

        #endregion

        #region Base class methods

        public override string ToString()
        {
            var     sb  = new StringBuilder();
            sb.Append("Leaderboard { ");
            sb.Append("Id: ").Append(Id).Append(" ");
            sb.Append("Title: ").Append(Title);
            sb.Append("}");
            return sb.ToString();
        }

        #endregion

        #region IGameServicesLeaderboard implementation

        public string Id
        {
            get;
            internal set;
        }

        public string PlatformId
        {
            get;
            private set;
        }

        public string Title
        {
            get
            {
                return GetTitleInternal();
            }
        }

        public LeaderboardPlayerScope PlayerScope
        {
            get
            {
                return GetPlayerScopeInternal();
            }
            set
            {
                SetPlayerScopeInternal(value);
            }
        } 
            
        public LeaderboardTimeScope TimeScope
        {
            get
            {
                return GetTimeScopeInternal();
            }
            set
            {
                SetTimeScopeInternal(value);
            }
        }

        public int LoadScoresQuerySize
        {
            get;
            set;
        }

        public IScore LocalPlayerScore
        {
            get
            {
                return GetLocalPlayerScoreInternal();
            }
        }

        public void LoadTopScores(EventCallback<LeaderboardLoadScoresResult> callback)
        {
            LoadTopScoresInternal((scores, error) =>
            {
                // send result
                SendLoadScoresResult(callback, scores, error);
            });
        }

        public void LoadPlayerCenteredScores(EventCallback<LeaderboardLoadScoresResult> callback)
        {
            LoadPlayerCenteredScoresInternal((scores, error) =>
            {
                // send result
                SendLoadScoresResult(callback, scores, error);
            });
        }

        public void LoadNext(EventCallback<LeaderboardLoadScoresResult> callback)
        {
            LoadNextInternal((scores, error) =>
            {
                // send result
                SendLoadScoresResult(callback, scores, error);
            });
        }

        public void LoadPrevious(EventCallback<LeaderboardLoadScoresResult> callback)
        {
            LoadPreviousInternal((scores, error) =>
            {
                // send result
                SendLoadScoresResult(callback, scores, error);
            });
        }

        public void LoadImage(EventCallback<TextureData> callback)
        {
            // make call
            LoadImageInternal((imageData, error) =>
            {
                // send result to caller object
                var     data    = (imageData == null) ? null : new TextureData(imageData);
                CallbackDispatcher.InvokeOnMainThread(callback, data, error);
            });
        }

        #endregion

        #region Private methods

        private void SendLoadScoresResult(EventCallback<LeaderboardLoadScoresResult> callback, IScore[] scores, Error error)
        {
            // fallback case to avoid null values
            scores          = scores ?? new IScore[0];

            // send result to caller object
            var     result = new LeaderboardLoadScoresResult()
            {
                Scores      = scores,
            };
            CallbackDispatcher.InvokeOnMainThread(callback, result, error);
        }

        #endregion
    }
}