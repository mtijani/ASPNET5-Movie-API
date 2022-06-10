using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using movieAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace movieAPI.DTOs
{
    public class MovieCreationDTO
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Trailer { get; set; }
        public bool InTheaters { get; set; }
        public IFormFile Poster { get; set; }
        public DateTime ReleaseDate { get; set; }
        [ModelBinder(BinderType= typeof(TypeBinder<List<int>>))]
        // If i send an array of integers it is going to be fine 
        //but if i send an array of actors it 
        //will throw an error
        public List<int> GenresIds { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        // If i send an array of integers it is going to be fine 
        //but if i send an array of actors it 
        //will throw an error
        public List<int> MovieTheatersIds { get; set; }
        [ModelBinder(BinderType = typeof(TypeBinder<List<MoviesActorsCreationDTO>>))]

        public List<MoviesActorsCreationDTO> Actors { get; set; }



    }
}
