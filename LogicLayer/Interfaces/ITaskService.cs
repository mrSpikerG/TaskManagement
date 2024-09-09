using DataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.Interfaces
{
    public interface ITaskService
    {
        Task<bool> DeleteTaskAsync(Guid taskId, Guid userId);
        Task<DataAccess.Models.Task> UpdateTaskAsync(Guid taskId, TaskDTO taskDto, Guid userId);
        Task<DataAccess.Models.Task> GetTaskByIdAsync(Guid taskId, Guid userId);
        Task<List<DataAccess.Models.Task>> GetTasksAsync(Guid userId, TaskFilterDTO filterDto, PaginationDTO paginationDto);
        Task<DataAccess.Models.Task> CreateTaskAsync(TaskDTO taskDto, Guid userId);
    }
}
