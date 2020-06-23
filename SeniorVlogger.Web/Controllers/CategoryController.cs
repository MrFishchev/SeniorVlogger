using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Constructor

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Actions

        [HttpGet]
        public async Task<IEnumerable<CategoryViewModel>> GetAll()
        {
            var category = await _unitOfWork.Categories.GetAll();
            return category.Select(c => c.ToViewModel());
        }

        #endregion
    }
}
