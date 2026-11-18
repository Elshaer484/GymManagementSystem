using GymManagment.BLL.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagment.BLL.Attachments
{
    public interface IAttachmentService
    {
        Task<Result<string?>> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default);
        Result<bool> Delete(string fileName , string FolderName);
       (Stream stream , string contentType)? GetFile(string fileName, string FolderName);
    }
}
