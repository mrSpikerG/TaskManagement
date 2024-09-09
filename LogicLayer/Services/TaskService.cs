using DataAccess;
using DataAccess.DTOs;
using DataAccess.Interfaces;
using LogicLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;


namespace LogicLayer.Services
{
    public class TaskService : ITaskService
    {

        private readonly ITaskRepository _taskRepository;
        public TaskService(ITaskRepository repository)
        {
            _taskRepository = repository;
        }

        public async Task<DataAccess.Models.Task> CreateTaskAsync(TaskDTO taskDto, Guid userId)
        {
            var task = new DataAccess.Models.Task
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = taskDto.Title,
                Description = taskDto.Description,
                DueDate = taskDto.DueDate,
                Status = taskDto.Status,
                Priority = taskDto.Priority,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _taskRepository.Insert(task);
            task.User.Tasks.Clear();
            return task;
        }

        public async Task<List<DataAccess.Models.Task>> GetTasksAsync(Guid userId, TaskFilterDTO filterDto, PaginationDTO paginationDto)
        {
            var query = _taskRepository.Get().Where(t => t.UserId == userId);

            if (filterDto.Status.HasValue)
            {
                query = query.Where(t => t.Status == filterDto.Status);
            }
            if (filterDto.DueDate.HasValue)
            {
                query = query.Where(t => t.DueDate == filterDto.DueDate);
            }
            if (filterDto.Priority.HasValue)
            {
                query = query.Where(t => t.Priority == filterDto.Priority);
            }

            if (!string.IsNullOrEmpty(filterDto.SortBy))
            {
                if (filterDto.SortBy == "DueDate")
                    query = query.OrderBy(t => t.DueDate);
                else if (filterDto.SortBy == "Priority")
                    query = query.OrderBy(t => t.Priority);
            }

            if (paginationDto.PageNumber > 0 && paginationDto.PageSize > 0)
            {
                query = query.Skip((paginationDto.PageNumber - 1) * paginationDto.PageSize)
                             .Take(paginationDto.PageSize);
            }
            foreach (var item in query)
            {
                item.User.Tasks.Clear();
            }
            return query.ToList();
        }

        public async Task<DataAccess.Models.Task> GetTaskByIdAsync(Guid taskId, Guid userId)
        {
            var query = _taskRepository.Get().FirstOrDefault(t => t.Id == taskId && t.UserId == userId);
            query.User.Tasks.Clear();

            return query;
        }

        public async Task<DataAccess.Models.Task> UpdateTaskAsync(Guid taskId, TaskDTO taskDto, Guid userId)
        {
            var task = _taskRepository.Get().FirstOrDefault(t => t.Id == taskId && t.UserId == userId);
            if (task == null)
                return null;

            task.Title = taskDto.Title;
            task.Description = taskDto.Description;
            task.DueDate = taskDto.DueDate;
            task.Status = taskDto.Status;
            task.Priority = taskDto.Priority;
            task.UpdatedAt = DateTime.UtcNow;

            _taskRepository.Update(task);
            task.User.Tasks.Clear();
            return task;
        }

        public async Task<bool> DeleteTaskAsync(Guid taskId, Guid userId)
        {
            var task = _taskRepository.Get().FirstOrDefault(t => t.Id == taskId && t.UserId == userId);
            if (task == null)
                return false;

            _taskRepository.Delete(task);
            task.User.Tasks.Clear();
            return true;
        }
    }
}
