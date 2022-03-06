using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using movieAPI.DTOs;
using movieAPI.Entities;
using movieAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movieAPI.Controllers
{
    [Route("api/actors")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly string containerName = "actors";
        private readonly IFileStorageService fileStorageService;

        public ActorsController(ApplicationDbContext context, IMapper mapper , IFileStorageService fileStorageService)
        {
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
           
            this.context = context;
        }
        [HttpGet]
     public async Task<ActionResult<List<ActorDTO>>> Get([FromQuery] PaginationDTO paginationDTO)
        {
            var queryable = context.Actors.AsQueryable();
            await HttpContext.InsertParammetersPaginationsInHeader(queryable);
            var actors = await queryable.OrderBy(x => x.Name).Paginate(paginationDTO).ToListAsync();
            return mapper.Map<List<ActorDTO>>(actors);
        }
        [HttpGet("{id:int}")]

        public async Task<ActionResult<ActorDTO>> Get(int id)
        {
            var actor = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            return mapper.Map<ActorDTO>(actor);
        }
        [HttpPost]

        public async Task<ActionResult> Post ([FromForm] ActorCreationDTO actorCreationDTO )
        {
            var actor = mapper.Map<Actor>(actorCreationDTO);
           if (actorCreationDTO.Picture != null)
            {
                actor.Picture = await fileStorageService.SaveFile(containerName, actorCreationDTO.Picture);

            }
            context.Add(actor);
             await   context.SaveChangesAsync();
            return NoContent();
        } 
        [HttpPut("{id:int}")]
        // FromForm : because we want to recieve a form file
        public async Task<ActionResult> Put (int id ,[FromForm] ActorCreationDTO actorCreationDTO)
        {
            var actor = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return (NotFound());
            }

            actor = mapper.Map(actorCreationDTO, actor);
            if (actorCreationDTO.Picture  != null)
            {
                actor.Picture = await fileStorageService.EditFile(containerName,
                    actorCreationDTO.Picture, actor.Picture);
            }
            await context.SaveChangesAsync();
            return NoContent();
        } 

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var actor = await context.Actors.FirstOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return NotFound();
            }
            context.Remove(actor);
            await context.SaveChangesAsync();
            await fileStorageService.DeleteFile(actor.Picture, containerName);
            return NoContent();
        }

    }
}
