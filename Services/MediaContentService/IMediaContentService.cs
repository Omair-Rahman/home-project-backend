using HomeProject.Models.Response;
using HomeProject.Models.Request.MediaContent;

namespace HomeProject.Services.MediaContentService
{
    public interface IMediaContentService
    {
        Task<ResponseModel<object>> UploadWithPreview(MediaContentInDto request);
    }
}
