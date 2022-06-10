using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using movieAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using movieAPI.DTOs;
using movieAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace movieAPI.Controllers
{
    [Route("api/movies")]

    public class MoviesController:ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly string container = "movies";
        private readonly IFileStorageService fileStorageService;

        public MoviesController(ApplicationDbContext context , IMapper mapper, IFileStorageService fileStorageService)
        {
            this.context = context;
            this.mapper = mapper;
            this.fileStorageService = fileStorageService;
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<MovieDTO>>Get(int id)
        {
            var movie = await context.Movies
                .Include(x => x.MovieGenres).ThenInclude(x => x.Genre)
                .Include(x => x.MovieTheatersMovies).ThenInclude(x => x.MovieTheater)
                .Include(x => x.MoviesActors).ThenInclude(x => x.Actor)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            var dto = mapper.Map<MovieDTO>(movie);
            dto.Actors = dto.Actors.OrderBy(x => x.Order).ToList();
            return dto;
        }

        [HttpGet("PostGet")]

        public async Task <ActionResult<MoviePostGetDTO>> PostGet()
        {
            var movieTheaters = await context.MovieTheaters.ToListAsync();

            var genres = await context.Genres.ToListAsync();
            var movieTheatersDTO = mapper.Map<List<MovieTheaterDTO>>(movieTheaters);
            var genresDTO = mapper.Map<List<GenreDTO>>(genres);
            return new MoviePostGetDTO() { Genres = genresDTO, MovieTheaters = movieTheatersDTO };
        }


        [HttpPost]

        public async Task<ActionResult> Post([FromForm]MovieCreationDTO movieCreationDTO)
        {
            var movie = mapper.Map<Movie>(movieCreationDTO);
            if(movieCreationDTO.Poster != null)
            {
                movie.Poster = await fileStorageService.SaveFile(container, movieCreationDTO.Poster);
            }
            AnnotateActorsOrder(movie);
            await context.SaveChangesAsync();
            return NoContent();
        }
        private void AnnotateActorsOrder(Movie movie)
        {
            if(movie.MoviesActors != null)
            {
                for(int i=0; i < movie.MoviesActors.Count; i++)
                {
                    // writing in the order property the order of actor which came from the front end
                    movie.MoviesActors[i].Order = i;
                }
            }

        }


    }
}
