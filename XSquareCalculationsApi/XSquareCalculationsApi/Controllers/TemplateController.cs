using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using XSquareCalculationsApi.Entities;
using XSquareCalculationsApi.Models;
using XSquareCalculationsApi.Repositories;
using XSquareCalculationsApi.Services.Templates;
using XSquareCalculationsApi.ViewModels;

namespace XSquareCalculationsApi.Controllers
{
    public class TemplateController : ApiController
    {
        private readonly IResolveTemplatesRepository _resolveTemplatesRepository;
        private readonly IDownloadTemplateService _downloadTemplateService;
        private readonly IResolveAthenticateRepository _resolveAthenticateRepository;
        private readonly ISystemDate _systemDate;

        public TemplateController(
            ISystemDate systemDate,
            IResolveTemplatesRepository resolveTemplatesRepository,
            IDownloadTemplateService downloadTemplateService,
            IResolveAthenticateRepository resolveAthenticateRepository)
        {
            _systemDate = systemDate;
            _resolveTemplatesRepository = resolveTemplatesRepository;
            _downloadTemplateService = downloadTemplateService;
            _resolveAthenticateRepository = resolveAthenticateRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<TemplateViewModel>> GetTemplates()
        {
            return await _resolveTemplatesRepository.GetTemplatesAsync();
        }

        [HttpGet]
        [Route("api/Template/{id}/download")]
        public async Task<HttpResponseMessage> DownloadTemplateAsync(int id)
        {
            var templateBlob = await _downloadTemplateService.DownloadTemplateAsync(id);
            if (templateBlob == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(templateBlob)
            };

            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            return result;
        }

        public async Task<HttpResponseMessage> GetTemplateDetail(int id)
        {
            var templateDetail = await _resolveTemplatesRepository.GetTemplateDetailAsync(id);
            if (templateDetail == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, templateDetail);
        }

        [HttpPost]
        public HttpResponseMessage Create()
        {
            Request.Headers.TryGetValues("Authorization", out var idTokenList);
            Request.Headers.TryGetValues("UserId", out var userIdStrList);
            var idToken = idTokenList.FirstOrDefault();
            var userIdStr = userIdStrList.FirstOrDefault();
            if (string.IsNullOrEmpty(idToken) || string.IsNullOrEmpty(userIdStr))
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            int.TryParse(userIdStr, out var userId);

            var target = _resolveAthenticateRepository.GetLoginAuthData(userId, idToken);
            if (target == null)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var isExpired = target.ExpiredDateTime < _systemDate.GetSystemDate();
            if (isExpired)
            {
                return Request.CreateResponse(HttpStatusCode.Unauthorized);
            }

            var formFiles = HttpContext.Current.Request.Files;

            var templateBlobContentType = formFiles["templateBlob"].ContentType;
            if(templateBlobContentType != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResponse
                {
                    Content = "TemplateFormatError",
                    Message = "テンプレートはExcel(.xlsx)を登録してください。"
                });
            }

            var templateBlobLength = formFiles["templateBlob"].ContentLength;
            if(templateBlobLength > 204800)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResponse
                {
                    Content = "TemplateSizeError",
                    Message = "テンプレートのサイズは2MB以下にする必要があります。"
                });
            }

            var templateBlobStream = formFiles["templateBlob"].InputStream;
            var templateBlobBytes = new BinaryReader(templateBlobStream).ReadBytes((int)templateBlobStream.Length);

            var thumbNailContentType = formFiles["thumbNail"].ContentType;
            if (thumbNailContentType != "image/png")
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResponse
                {
                    Content = "ThumbNailFormatError",
                    Message = "サムネイルは画像(.png形式)を登録してください。"
                });
            }

            var thumbNailLength = formFiles["thumbNail"].ContentLength;
            if (thumbNailLength > 204800)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, new ApiResponse
                {
                    Content = "ThumbNailSizeError",
                    Message = "サムネイルのサイズは2MB以下にする必要があります。"
                });
            }

            var thumbNailStream = formFiles["thumbNail"].InputStream;
            var thumbNailBytes = new BinaryReader(thumbNailStream).ReadBytes((int)thumbNailStream.Length);

            var templateName = HttpContext.Current.Request.Form["templateName"];

            var template = new Template
            {
                TemplateBlob = templateBlobBytes,
                TemplateName = templateName,
                ThumbNail = Convert.ToBase64String(thumbNailBytes),
                UserId = userId
            };

            _resolveTemplatesRepository.RegistTemplate(template);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
