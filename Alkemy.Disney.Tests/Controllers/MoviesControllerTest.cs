using Alkemy.Disney.Controllers.Movies;
using Alkemy.Disney.DTOs.Movies;
using Alkemy.Disney.Models.Entities;
using Alkemy.Disney.Tests.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Alkemy.Disney.Tests.Controllers
{
    public class MoviesControllerTest : BaseTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetAllTest()
        {
            //
            var db = GetDbContext(Guid.NewGuid().ToString());
            var list = db.Movies.ToList();

            var Controller = new MoviesController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            //
            var resp = await Controller.Get();
            var result = resp.Value;

            //
            Assert.AreEqual(list.Count(),result.Count());
        }

        [Test]
        public async Task GetByIdTest()
        {
            //
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = db.Movies.First();

            var Controller = new MoviesController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            //
            var resp = await Controller.Get(entity.Id);
            var result = resp.Value;

            //
            Assert.AreEqual(entity.Id, result.Id);
        }

        [Test]
        public async Task SetCorrectCaracterTest()
        {
            //
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = db.Movies.First();
            var caracter = db.Caracters.First();

            var Controller = new MoviesController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            //
            var resp = await Controller.Caracters(entity.Id,caracter.Id);
            var result = resp;

            //
            Assert.NotNull(result);
            Assert.IsInstanceOf(typeof(NoContentResult),result);
        }

        [Test]
        public async Task SetIncorrectMovieTest()
        {
            //
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = db.Movies.First();
            var movie = db.Movies.First();

            var Controller = new MoviesController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            //
            var resp = await Controller.Caracters(entity.Id, 0);
            var result = resp;

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public async Task SetNotExistMovieTest()
        {
            //
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = db.Movies.First();
            var movie = db.Movies.First();

            var Controller = new MoviesController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            //
            var resp = await Controller.Caracters(entity.Id, 4589);
            var result = resp;

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }

        [Test]
        public async Task CreateNewItemTest()
        {
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Movies.FirstAsync();
            entity.Id = 0;
            db.ChangeTracker.Clear();

            var Controller = new MoviesController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            //
            var resp = await Controller.Post(new MovieEditDTO(entity));
            var result = resp.Result;

            // Assert
            Assert.IsInstanceOf(typeof(CreatedAtActionResult), result);
        }

        [Test]
        public async Task CreateNewItemBadRequestNotCeroIdTest()
        {
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Movies.FirstAsync();
            
            var Controller = new MoviesController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            //
            var resp = await Controller.Post(new MovieEditDTO(entity));
            var result = resp.Result;

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        public async Task CreateNewItemBadRequestNotValidPropertyTest()
        {
            // Arrange
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Movies.FirstAsync();
            entity.Id=0;
            entity.Title = null;

            var Controller = new MoviesController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();
            Controller.ObjectValidator = new ObjectValidator();


            //Act
            Controller.TryValidateModel(entity);
            var resp = await Controller.Post(new MovieEditDTO(entity));
            var result = resp.Result;

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        public async Task EditItemTest()
        {
            // Arrange
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Movies.FirstAsync();
            entity.Title = "Editado";
            db.ChangeTracker.Clear();

            var Controller = new MoviesController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();
            Controller.ObjectValidator = new ObjectValidator();

            //Act
            Controller.TryValidateModel(entity);
            var resp = await Controller.Put(entity.Id, new MovieEditDTO(entity));
            var result = resp;

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), result);
        }

        [Test]
        public async Task EditItemBadRequestIdNotEditableTest()
        {
            // Arrange
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Movies.FirstAsync();
            var oldId = entity.Id;
            entity.Id = 25893;

            var Controller = new MoviesController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();
            Controller.ObjectValidator = new ObjectValidator();

            //Act
            Controller.TryValidateModel(entity);
            var resp = await Controller.Put(oldId, new MovieEditDTO(entity));
            var result = resp;

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        public async Task EditItemBadRequestNotValidEntityTest()
        {
            // Arrange
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Movies.FirstAsync();
            entity.Title = null;

            var Controller = new MoviesController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();
            Controller.ObjectValidator = new ObjectValidator();

            //Act
            Controller.TryValidateModel(entity);
            var resp = await Controller.Put(entity.Id, new MovieEditDTO(entity));
            var result = resp;

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        public async Task DeleteItemTest()
        {
            // Arrange
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Movies.FirstAsync();

            var Controller = new MoviesController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            //Act
            var resp = await Controller.Delete(entity.Id);
            var result = resp;

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult),result);
        }

        [Test]
        public async Task DeleteItemNotFoundTest()
        {
            // Arrange
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Movies.FirstAsync();
            entity.Id = 546454;

            var Controller = new MoviesController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            //Act
            var resp = await Controller.Delete(entity.Id);
            var result = resp;

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }
    }
}