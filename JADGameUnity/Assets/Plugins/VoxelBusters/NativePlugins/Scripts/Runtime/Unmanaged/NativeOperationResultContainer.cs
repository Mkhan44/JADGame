using VoxelBusters.CoreLibrary;

namespace VoxelBusters.CoreLibrary.NativePlugins
{
    internal class NativeOperationResultContainer<T> : IOperationResultContainer<T>
    {
        #region Fields

        private     T           m_result;

        private     Error       m_error;

        #endregion

        #region Static methods

        public static NativeOperationResultContainer<T> Create(T result, Error error = null)
        { 
            // set properties
            return new NativeOperationResultContainer<T>()
            {
                m_result    = result,
                m_error     = error,
            };
        }

        #endregion

        #region IResultContainer implementation

        public bool IsError()
        {
            return (m_error != null);
        }

        public Error GetError()
        {
            return m_error;
        }

        public T GetResult()
        {
            return m_result;
        }

        public string GetResultAsText()
        {
            return string.Empty;
        }

        #endregion
    }
}