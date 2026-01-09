using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using PortfolioAdminApi.Models.About;
using PortfolioAdminApi.Services;
using System.Data;

namespace PortfolioAdminApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AboutController : ControllerBase
{
    private readonly OracleDbService _db;

    public AboutController(OracleDbService db)
    {
        _db = db;
    }

    // ✅ GET ABOUT
    [HttpGet]
    public IActionResult Get()
    {
        using var conn = _db.GetConnection();
        conn.Open();

        using var cmd = new OracleCommand("SP_ABOUT_SECTION_PORTFOLIO", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = 1;
        cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = DBNull.Value;
        cmd.Parameters.Add("P_INTRO_TEXT", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_FRONTEND_SKILLS", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_BACKEND_SKILLS", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_DATABASE_SKILLS", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_TOOLS_SKILLS", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        using var reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            var response = new AboutResponse
            {
                Id = reader.GetInt32(0),
                IntroText = reader.GetString(1),
                FrontendSkills = reader.GetString(2).Split(',').ToList(),
                BackendSkills = reader.GetString(3).Split(',').ToList(),
                DatabaseSkills = reader.GetString(4).Split(',').ToList(),
                ToolsSkills = reader.GetString(5).Split(',').ToList()
            };

            return Ok(response);
        }

        return NotFound();
    }

    //UPDATE

    //[HttpPut]
    [HttpPost("update")]
    public IActionResult Update([FromBody] AboutUpdateRequest request)
    {
        using var conn = _db.GetConnection();
        conn.Open();

        using var cmd = new OracleCommand("SP_ABOUT_SECTION_PORTFOLIO", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = 2;
        cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = 1;
        cmd.Parameters.Add("P_INTRO_TEXT", OracleDbType.Varchar2).Value = request.IntroText;
        cmd.Parameters.Add("P_FRONTEND_SKILLS", OracleDbType.Varchar2).Value = string.Join(",", request.FrontendSkills);
        cmd.Parameters.Add("P_BACKEND_SKILLS", OracleDbType.Varchar2).Value = string.Join(",", request.BackendSkills);
        cmd.Parameters.Add("P_DATABASE_SKILLS", OracleDbType.Varchar2).Value = string.Join(",", request.DatabaseSkills);
        cmd.Parameters.Add("P_TOOLS_SKILLS", OracleDbType.Varchar2).Value = string.Join(",", request.ToolsSkills);
        cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        cmd.ExecuteNonQuery();

        return Ok(new { status = "updated" });
    }



}

