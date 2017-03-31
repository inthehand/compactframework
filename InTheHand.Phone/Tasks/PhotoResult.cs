using System;

using System.Collections.Generic;
using System.Text;

namespace InTheHand.Phone.Tasks
{
    /// <summary>
    /// Represents a photo returned from a call to the Show method of a <see cref="PhotoChooserTask"/> object or a <see cref="CameraCaptureTask"/> object.
    /// </summary>
    public sealed class PhotoResult : TaskEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoResult"/> class.
        /// </summary>
        public PhotoResult()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoResult"/> class with the specified <see cref="TaskResult"/>.
        /// </summary>
        /// <param name="taskResult"></param>
        public PhotoResult(TaskResult taskResult)
            : base(taskResult)
        {
        }

        /// <summary>
        /// Gets the <see cref="System.IO.Stream"/> containing the data for the photo.
        /// </summary>
        /// <value>The data for the photo.</value>
        public System.IO.Stream ChosenPhoto
        {
            get
            {
                if (OriginalFileName != null)
                {
                    return new System.IO.FileStream(OriginalFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                }

                return null;
            }
        }
        /// <summary>
        /// Gets the file name of the photo.
        /// </summary>
        /// <value>The file name of the photo.</value>
        public string OriginalFileName { set; get; }
    }
}
