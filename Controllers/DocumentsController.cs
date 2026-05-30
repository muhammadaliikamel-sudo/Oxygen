using Microsoft.AspNetCore.Mvc;
using Oxygen.DTOs;
using Oxygen.Interfaces;

namespace Oxygen.Controllers
{
    [ApiController]
    [Route("documents")]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentsController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpPost("upload")]
        public async Task<ActionResult<DocumentUploadResponseDto>> Upload(
            [FromForm] DocumentUploadRequestDto request,
            CancellationToken cancellationToken)
        {
            try
            {
                var response = await _documentService.UploadAsync(request, cancellationToken);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway, ex.Message);
            }
        }
    }
}
