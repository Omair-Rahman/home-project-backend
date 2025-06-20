using HomeProject.Models.Response;
using HomeProject.Models.Request.MediaContent;
using HomeProject.Models.Response.MediaContent;

namespace HomeProject.Services.MediaContentService
{
    public interface IMediaContentService
    {
        Task<ResponseModel<object>> UploadWithPreview(MediaContentInDto request);
        Task<ResponseModel<object>> UploadWithPreviewV2(MediaContentInDto request);
        Task<ResponseModel<List<ContentPreviewListDto>>> GetContents(MediaContentFilter request);
        Task<ResponseModel<ContentDetailsDto>> GetFullContentsById(int id);
        Task<ResponseModel<object>> UpdateContentIsFavourite(int id, bool isFavourite);
    }
}
