﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SeniorVlogger.DataAccess.Repository.IRepository;
using SeniorVlogger.Models.ViewModels;
using SeniorVlogger.Web.Extensions;

namespace SeniorVlogger.Web.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        #region Fields

        private readonly ILogger<CategoryController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        public CategoryController(IUnitOfWork unitOfWork, ILogger<CategoryController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        #endregion

        #region Actions

        [HttpGet]
        public async Task<IEnumerable<CategoryViewModel>> GetAll()
        {
            var category = await _unitOfWork.Categories.GetAll();
            return category.Select(c => c.ToViewModel());
        }

        [HttpGet("{id}")]
        public async Task<CategoryViewModel> Get(int id)
        {
            return (await _unitOfWork.Categories.Get(id)).ToViewModel();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryViewModel category)
        {
            try
            {
                var objDb = category.ToDto();
                await _unitOfWork.Categories.Add(objDb);
                await _unitOfWork.Save();
            }
            catch(Exception e)
            {
                _logger.LogError(e, $"Cannot create category: {e.Message}");
                return Problem();
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CategoryViewModel category)
        {
            try
            {
                var categoryDb = await _unitOfWork.Categories.Get(category.Id);
                if (categoryDb == null) return NotFound();

                await _unitOfWork.Categories.Update(category.ToDto());
                await _unitOfWork.Save();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Cannot update category: {e.Message}");
                return Problem();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var category = await _unitOfWork.Categories.Get(id);
                if (category == null) return NotFound();

                await _unitOfWork.Categories.Remove(category);
                await _unitOfWork.Save();
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Cannot delete the category: {e.Message}");
                return Json(new {status = false, message = 
                    "Category is used by some posts. Please, delete those posts before."});
            }

            return Json(new {status = true});
        }

        #endregion
    }
}
