using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TodoAPI2;

namespace TodoAPI2.Models
{
    public class mTask
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;
        public int ProjectId { get; set; }
        public mProject Project { get; set; }
    }


    public static class mTaskEndpoints
    {
        public static async void MapmTaskEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/mTask").WithTags(nameof(mTask));

            #region Endpoints
            group.MapGet("/", GetAllTask);
            group.MapGet("/{id}", GetTaskById);
            group.MapGet("/byProject/{projectId}", GetTaskByProjectId);
            group.MapPost("/{projectId}", CreateTask);
            #endregion
            static async Task<IResult> GetAllTask(DataContext db)
            {
                return TypedResults.Ok(await db.Task.ToListAsync());
            }
            static async Task<IResult> GetTaskById(int id, DataContext db)
            {
                return await db.Task.FindAsync(id) is mTask task ?
                    TypedResults.Ok(task) : TypedResults.NotFound();
            }
            static async Task<IResult> CreateTask(int projectId, mTask inputTask, DataContext db)
            {
                var project = await db.Projects.FindAsync(projectId);
                if (project is null) return TypedResults.NotFound();

                project.Task.Add(new mTask { Name = inputTask.Name });

                await db.SaveChangesAsync();
                return TypedResults.Ok();
            }
            static async Task<IResult> GetTaskByProjectId(int projectId, DataContext db)
            {
                List<mTask> tasks = new List<mTask>();
                await db.Task.Where(t => t.ProjectId == projectId).ForEachAsync(t => tasks.Add(t));
                return TypedResults.Ok(tasks);
            }
        }
    }

}
