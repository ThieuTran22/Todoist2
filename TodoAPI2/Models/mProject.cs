using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using TodoAPI2;

namespace TodoAPI2.Models
{
    public class mProject
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<mTask> Task { get; set; } = new List<mTask>();
    }

    public static class mProjectEndpoints
    {
        public static void MapmProjectEndpoints(this IEndpointRouteBuilder routes)
        {
            var group = routes.MapGroup("/api/mProject").WithTags(nameof(mProject));

            #region Endpoints
            group.MapGet("/", GetAllProject);
            group.MapGet("/{id}", GetProjectById);
            group.MapPut("/{id}", UpdateProject);
            group.MapPost("/", CreateProject);
            group.MapDelete("/{id}", DeleteProject);
            #endregion

            #region Methods
            static async Task<IResult> GetAllProject(DataContext db)
            {
                return TypedResults.Ok(await db.Projects.ToListAsync());
            }
            static async Task<IResult> GetProjectById(int id, DataContext db)
            {
                return await db.Projects.FindAsync(id) is mProject project ?
                    TypedResults.Ok(project) : TypedResults.NotFound();
            }
            static async Task<IResult> CreateProject(mProject inputProject, DataContext db)
            {
                db.Projects.Add(inputProject);

                await db.SaveChangesAsync();
                return TypedResults.Created($"/api/mProject/{inputProject.Id}", inputProject);
            }
            static async Task<IResult> UpdateProject(int id, mProject inputProject, DataContext db)
            {
                var project = await db.Projects.FindAsync(id);
                if (project is null) return TypedResults.NotFound();

                project.Name = inputProject.Name;
                project.Description = inputProject.Description;

                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }
            static async Task<IResult> DeleteProject(int id, DataContext db)
            {
                var project = await db.Projects.FindAsync(id);
                if (project is null) return TypedResults.NotFound();

                db.Projects.Remove(project);

                await db.SaveChangesAsync();
                return TypedResults.NoContent();
            }
            #endregion


        }
    }
}
