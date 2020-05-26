using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Task.Data;
using Task.Dtos;
using Task.Helpers;
using Task.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Task.Controllers
{
    [Authorize]
    [Route("api/todos/{todoId}/Images")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IToDoRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;

        public ImagesController(IToDoRepository repo, IMapper mapper,
            IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _cloudinaryConfig = cloudinaryConfig;
            _mapper = mapper;
            _repo = repo;

            Account acc = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.ApiKey,
                _cloudinaryConfig.Value.ApiSecret
            );

            _cloudinary = new Cloudinary(acc);
        }

        [HttpGet("{id}", Name = "GetImage")]
        public async Task<IActionResult> GetImage(int id)
        {
            int UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var photoFromRepo = await _repo.GetImage(id, UserId);

            var photo = _mapper.Map<ImageForReturnDto>(photoFromRepo);

            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoForToDo(int todoId,
            [FromForm]ImageForCreationDto photoForCreationDto)
        {

            int UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var todoFromRepo = await _repo.GetToDo(todoId, UserId);

            var file = photoForCreationDto.File;

            var uploadResult = new ImageUploadResult();

            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                            .Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDto.Url = uploadResult.Uri.ToString();
            photoForCreationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Image>(photoForCreationDto);

            todoFromRepo.Images.Add(photo);

            if (await _repo.SaveAll())
                return Ok(todoFromRepo.Id);


            return BadRequest("Failed to add");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int todoId, int id)
        {

            int UserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var todo = await _repo.GetToDo(todoId, UserId);

            if (!todo.Images.Any(p => p.Id == id))
                return Unauthorized();

            var photoFromRepo = await _repo.GetImage(id, UserId);


            if (photoFromRepo.PublicID != null)
            {
                var deleteParams = new DeletionParams(photoFromRepo.PublicID);

                var result = _cloudinary.Destroy(deleteParams);

                if (result.Result == "ok")
                {
                    _repo.Delete(photoFromRepo);
                }
            }

            if (photoFromRepo.PublicID == null)
            {
                _repo.Delete(photoFromRepo);
            }

            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to delete the photo");
        }

    }
}