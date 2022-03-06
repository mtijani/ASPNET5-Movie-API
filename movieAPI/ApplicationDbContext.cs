using Microsoft.EntityFrameworkCore;
using movieAPI.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace movieAPI
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MoviesActors>()
                .HasKey(x => new { x.ActorId, x.MovieId });
            modelBuilder.Entity<MovieGenres>()
                .HasKey(x => new { x.GenreId, x.MovieId });
            modelBuilder.Entity<MovieTheatersMovies>()
                .HasKey(x => new { x.MovieTheaterId, x.MovieId });
           /* modelBuilder.Entity<MovieTheatersMovies>()
                .Property(s => s.Movie).IsRequired();
            modelBuilder.Entity<MovieTheatersMovies>()
               .Property(s => s.MovieTheater).IsRequired();
            modelBuilder.Entity<MovieTheatersMovies>()
               .Property(s => s.MovieTheaterId).IsRequired();
            modelBuilder.Entity<MovieTheatersMovies>()
               .Property(s => s.MovieId).IsRequired();*/

            base.OnModelCreating(modelBuilder);
                

        }

        public DbSet<Genre> Genres { get; set; }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<MovieTheater> MovieTheaters { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<MoviesActors> MoviesActors { get; set; }
        public DbSet<MovieGenres> MovieGenres { get; set; }
        public DbSet<MovieTheatersMovies> MovieTheaterMovies { get; set; }

      //  public DbSet<MoviTheatersMovies> MoviTheatersMovies { get; set; }


    }
}
