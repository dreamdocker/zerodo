﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ZeroStack.DeviceCenter.Application.Models.Generics;
using ZeroStack.DeviceCenter.Application.Models.Projects;
using ZeroStack.DeviceCenter.Application.PermissionProviders;
using ZeroStack.DeviceCenter.Application.Queries.Projects;
using ZeroStack.DeviceCenter.Application.Services.Generics;

namespace ZeroStack.DeviceCenter.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "role1")]
    public class ProjectsController : ControllerBase
    {
        private readonly ICrudApplicationService<int, ProjectGetResponseModel, PagedRequestModel, ProjectGetResponseModel, ProjectCreateOrUpdateRequestModel, ProjectCreateOrUpdateRequestModel> _crudService;

        private readonly IProjectQueries _projectQueries;
        public ProjectsController(ICrudApplicationService<int, ProjectGetResponseModel, PagedRequestModel, ProjectGetResponseModel, ProjectCreateOrUpdateRequestModel, ProjectCreateOrUpdateRequestModel> crudService, IProjectQueries projectQueries)
        {
            _crudService = crudService;
            _projectQueries = projectQueries;
        }

        // GET: api/<ProjectsController>
        [HttpGet]
        [Authorize(ProjectPermissions.Projects.Default)]
        public async Task<PagedResponseModel<ProjectGetResponseModel>> Get([FromQuery] ProjectPagedRequestModel model)
        {
            return await _projectQueries.GetProjectsAsync(model);
        }

        // GET api/<ProjectsController>/5
        [HttpGet("{id}")]
        [Authorize(ProjectPermissions.Projects.Default)]
        public async Task<ProjectGetResponseModel> Get(int id)
        {
            return await _projectQueries.GetProjectAsync(id);
        }

        // POST api/<ProjectsController>
        [HttpPost]
        [Authorize(ProjectPermissions.Projects.Create)]
        public async Task<ProjectGetResponseModel> Post([FromBody] ProjectCreateOrUpdateRequestModel value)
        {
            return await _crudService.CreateAsync(value);
        }

        // PUT api/<ProjectsController>/5
        [HttpPut("{id}")]
        [Authorize(ProjectPermissions.Projects.Edit)]
        public async Task<ProjectGetResponseModel> Put(int id, [FromBody] ProjectCreateOrUpdateRequestModel value)
        {
            value.Id = id;
            return await _crudService.UpdateAsync(value);
        }

        // DELETE api/<ProjectsController>/5
        [HttpDelete("{id}")]
        [Authorize(ProjectPermissions.Projects.Delete)]
        public async Task Delete(int id)
        {
            await _crudService.DeleteAsync(id);
        }
    }
}
