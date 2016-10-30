using Windows.UI.Core;
using Windows.Storage;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SpeechRecognition.Source
{
    public interface IRecording
    {
        Task<bool> CleanOldFiles();
        Task<bool> Record();
        Task<bool> Stop(CoreDispatcher dispatcher);
        Task<bool> Play(string fileName);
        Task<StorageFile> DisplayRecording();
        Task<IList<string>> GetCreatedAudioFiles();
    }
}
