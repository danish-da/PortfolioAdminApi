using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using PortfolioAdminApi.Models.Projects;
using PortfolioAdminApi.Services;
using System.Data;

namespace PortfolioAdminApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectsController : ControllerBase
{
    private readonly OracleDbService _db;

    public ProjectsController(OracleDbService db)
    {
        _db = db;
    }

    // ✅ GET ALL PROJECTS
    [HttpGet]
    public IActionResult GetAll()
    {
        using var conn = _db.GetConnection();
        conn.Open();

        using var cmd = new OracleCommand("SP_PROJECTS_DAN", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = 1;
        cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = DBNull.Value;
        cmd.Parameters.Add("P_TITLE", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_SUBTITLE", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_TECHSTACK", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_PROJECT_LINK", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_GITHUB_LINK", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        using var reader = cmd.ExecuteReader();

        var list = new List<ProjectResponse>();

        while (reader.Read())
        {
            list.Add(new ProjectResponse
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Subtitle = reader.GetString(2),
                Description = reader.GetString(3),
                Techstack = reader.GetString(4),
                ProjectLink = reader.GetString(5),
                GithubLink = reader.GetString(6)
            });
        }

        return Ok(list);
    }

    // ✅ CREATE PROJECT
    [HttpPost]
    public IActionResult Create([FromBody] ProjectRequest req)
    {
        using var conn = _db.GetConnection();
        conn.Open();

        using var cmd = new OracleCommand("SP_PROJECTS_DAN", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = 2;
        cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = DBNull.Value;
        cmd.Parameters.Add("P_TITLE", OracleDbType.Varchar2).Value = req.Title;
        cmd.Parameters.Add("P_SUBTITLE", OracleDbType.Varchar2).Value = req.Subtitle;
        cmd.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = req.Description;
        cmd.Parameters.Add("P_TECHSTACK", OracleDbType.Varchar2).Value = req.Techstack;
        cmd.Parameters.Add("P_PROJECT_LINK", OracleDbType.Varchar2).Value = req.ProjectLink;
        cmd.Parameters.Add("P_GITHUB_LINK", OracleDbType.Varchar2).Value = req.GithubLink;
        cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        cmd.ExecuteNonQuery();

        return Ok(new { status = "created" });
    }

    // ✅ UPDATE PROJECT
    [HttpPost("{id}")]
    public IActionResult UpdateViaPost(int id, [FromBody] ProjectRequest req)
    {
        using var conn = _db.GetConnection();
        conn.Open();

        using var cmd = new OracleCommand("SP_PROJECTS_DAN", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = 3;
        cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id;
        cmd.Parameters.Add("P_TITLE", OracleDbType.Varchar2).Value = req.Title;
        cmd.Parameters.Add("P_SUBTITLE", OracleDbType.Varchar2).Value = req.Subtitle;
        cmd.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = req.Description;
        cmd.Parameters.Add("P_TECHSTACK", OracleDbType.Varchar2).Value = req.Techstack;
        cmd.Parameters.Add("P_PROJECT_LINK", OracleDbType.Varchar2).Value = req.ProjectLink;
        cmd.Parameters.Add("P_GITHUB_LINK", OracleDbType.Varchar2).Value = req.GithubLink;
        cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        cmd.ExecuteNonQuery();

        return Ok(new { status = "updated" });
    }


    // ✅ DELETE PROJECT
    [HttpPost("{id}/delete")]
    public IActionResult Delete(int id)
    {
        using var conn = _db.GetConnection();
        conn.Open();

        using var cmd = new OracleCommand("SP_PROJECTS_DAN", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = 4;
        cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id;
        cmd.Parameters.Add("P_TITLE", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_SUBTITLE", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_TECHSTACK", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_PROJECT_LINK", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_GITHUB_LINK", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        cmd.ExecuteNonQuery();

        return Ok(new { status = "deleted" });
    }
}
