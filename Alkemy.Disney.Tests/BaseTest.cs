using Alkemy.Disney.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Alkemy.Disney.Models.Entities;

namespace Alkemy.Disney.Tests
{
    public class BaseTest
    {
        protected DisneyDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<DisneyDbContext>()
                .UseInMemoryDatabase(dbName).Options;
            var dbContext = new DisneyDbContext(options);

            SeedData(dbContext);

            return dbContext;
        }

        protected virtual void SeedData(DbContext dbContext)
        {
            var genres = new List<Genre>()
            {
                new Genre(){  Name = "Accion" },
                new Genre(){  Name = "Drama" },
                new Genre(){  Name = "Comedia" }
            };

            var caracters = new List<Caracter>()
            {
                new Caracter(){  Name = "Will Smit", Age = 53, Weight = 1.88m, History = "El de la bofetada" },
                new Caracter(){  Name = "Popeye el Marino", Age = 33, Weight = 1.53m, History = "Le encanta la espinaca" },
                new Caracter(){  Name = "Jerry", Age = 2, Weight = 0.5m, History = "Simpatico ratoncito" }
            };

            var movies = new List<Movie>()
            {
                new Movie(){ Title="Una noche de escandalos", CreationDate=new DateTime(2022,03,12), Rate = 5, GenreId = 1},
                new Movie(){ Title="Buscando a Olivia", CreationDate=new DateTime(1812,07,22), Rate = 4, GenreId = 2},
                new Movie(){ Title="Lero Lero", CreationDate=new DateTime(1998,11,1), Rate = 3, GenreId = 3}
            };

            dbContext.AddRange(genres);
            dbContext.AddRange(caracters);
            dbContext.AddRange(movies);

            dbContext.SaveChanges();
        }
    }
}
