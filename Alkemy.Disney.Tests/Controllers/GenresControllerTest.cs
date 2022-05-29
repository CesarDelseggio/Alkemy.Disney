using Alkemy.Disney.Controllers.Genres;
using Alkemy.Disney.DTOs.Genres;
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
    public class GenresControllerTest : BaseTest
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
            var list = db.Genres.ToList();

            var Controller = new GenresController(db);
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
            var entity = db.Genres.First();

            var Controller = new GenresController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            //
            var resp = await Controller.Get(entity.Id);
            var result = resp.Value;

            //
            Assert.AreEqual(entity.Id, result.Id);
        }

        [Test]
        public async Task CreateNewItemTest()
        {
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Genres.FirstAsync();
            entity.Id = 0;
            db.ChangeTracker.Clear();

            var Controller = new GenresController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            //
            var resp = await Controller.Post(new GenreEditDTO(entity));
            var result = resp.Result;

            // Assert
            Assert.IsInstanceOf(typeof(CreatedAtActionResult), result);
        }

        [Test]
        public async Task CreateNewItemBadRequestNotCeroIdTest()
        {
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Genres.FirstAsync();
            
            var Controller = new GenresController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            //
            var resp = await Controller.Post(new GenreEditDTO(entity));
            var result = resp.Result;

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        public async Task CreateNewItemBadRequestNotValidPropertyTest()
        {
            // Arrange
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Genres.FirstAsync();
            entity.Id=0;
            entity.Name = null;

            var Controller = new GenresController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();
            Controller.ObjectValidator = new ObjectValidator();


            //Act
            Controller.TryValidateModel(entity);
            var resp = await Controller.Post(new GenreEditDTO(entity));
            var result = resp.Result;

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        public async Task EditItemTest()
        {
            // Arrange
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Genres.FirstAsync();
            entity.Name = "Editado";
            db.ChangeTracker.Clear();

            var Controller = new GenresController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();
            Controller.ObjectValidator = new ObjectValidator();

            //Act
            Controller.TryValidateModel(entity);
            var resp = await Controller.Put(entity.Id, new GenreEditDTO(entity));
            var result = resp;

            // Assert
            Assert.IsInstanceOf(typeof(NoContentResult), result);
        }

        [Test]
        public async Task EditItemBadRequestIdNotEditableTest()
        {
            // Arrange
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Genres.FirstAsync();
            var oldId = entity.Id;
            entity.Id = 25893;

            var Controller = new GenresController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();
            Controller.ObjectValidator = new ObjectValidator();

            //Act
            Controller.TryValidateModel(entity);
            var resp = await Controller.Put(oldId, new GenreEditDTO(entity));
            var result = resp;

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        public async Task EditItemBadRequestNotValidEntityTest()
        {
            // Arrange
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Genres.FirstAsync();
            entity.Name = null;

            var Controller = new GenresController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();
            Controller.ObjectValidator = new ObjectValidator();

            //Act
            Controller.TryValidateModel(entity);
            var resp = await Controller.Put(entity.Id, new GenreEditDTO(entity));
            var result = resp;

            // Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), result);
        }

        [Test]
        public async Task DeleteItemTest()
        {
            // Arrange
            var db = GetDbContext(Guid.NewGuid().ToString());
            var entity = await db.Genres.FirstAsync();

            var Controller = new GenresController(db);
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
            var entity = await db.Genres.FirstAsync();
            entity.Id = 546454;

            var Controller = new GenresController(db);
            Controller.ControllerContext.HttpContext = new DefaultHttpContext();

            //Act
            var resp = await Controller.Delete(entity.Id);
            var result = resp;

            // Assert
            Assert.IsInstanceOf(typeof(NotFoundResult), result);
        }
    }
}