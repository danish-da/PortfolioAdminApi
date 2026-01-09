using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using PortfolioAdminApi.Models.Experience;
using PortfolioAdminApi.Services;
using System.Data;

namespace PortfolioAdminApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExperienceController : ControllerBase
{
    private readonly OracleDbService _db;

    public ExperienceController(OracleDbService db)
    {
        _db = db;
    }

    // ✅ GET ALL EXPERIENCES
    [HttpGet]
    public IActionResult GetAll()
    {
        using var conn = _db.GetConnection();
        conn.Open();

        using var cmd = new OracleCommand("SP_EXPERIENCE_PORTFOLIO_DAN", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = 1;
        cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = DBNull.Value;
        cmd.Parameters.Add("P_TITLE", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_COMPANY", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_DURATION", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        using var reader = cmd.ExecuteReader();

        var list = new List<ExperienceResponse>();

        while (reader.Read())
        {
            list.Add(new ExperienceResponse
            {
                Id = reader.GetInt32(0),
                Title = reader.GetString(1),
                Company = reader.GetString(2),
                Duration = reader.GetString(3),
                Description = reader.GetString(4)
            });
        }

        return Ok(list);
    }

    [HttpPost]
    public IActionResult Create([FromBody] ExperienceRequest req)
    {
        // ✅ Normalize smart dashes to plain hyphen
        req.Duration = req.Duration
            .Replace("–", "-")
            .Replace("—", "-")
            .Replace("‑", "-");

        using var conn = _db.GetConnection();
        conn.Open();

        using var cmd = new OracleCommand("SP_EXPERIENCE_PORTFOLIO_DAN", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = 2;
        cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = DBNull.Value;
        cmd.Parameters.Add("P_TITLE", OracleDbType.Varchar2).Value = req.Title;
        cmd.Parameters.Add("P_COMPANY", OracleDbType.Varchar2).Value = req.Company;
        cmd.Parameters.Add("P_DURATION", OracleDbType.Varchar2).Value = req.Duration;
        cmd.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = req.Description;
        cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        cmd.ExecuteNonQuery();

        return Ok(new { status = "created" });
    }


    // ✅ UPDATE EXPERIENCE
    [HttpPost("{id}/update")]
    public IActionResult Update(int id, [FromBody] ExperienceRequest req)
    {
        using var conn = _db.GetConnection();
        conn.Open();

        using var cmd = new OracleCommand("SP_EXPERIENCE_PORTFOLIO_DAN", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = 3;
        cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id;
        cmd.Parameters.Add("P_TITLE", OracleDbType.Varchar2).Value = req.Title;
        cmd.Parameters.Add("P_COMPANY", OracleDbType.Varchar2).Value = req.Company;
        cmd.Parameters.Add("P_DURATION", OracleDbType.Varchar2).Value = req.Duration;
        cmd.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = req.Description;
        cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        cmd.ExecuteNonQuery();

        return Ok(new { status = "updated" });
    }

    // ✅ DELETE EXPERIENCE
    [HttpPost("{id}/delete")]
    public IActionResult Delete(int id)
    {
        using var conn = _db.GetConnection();
        conn.Open();

        using var cmd = new OracleCommand("SP_EXPERIENCE_PORTFOLIO_DAN", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("P_FLAG", OracleDbType.Int32).Value = 4;
        cmd.Parameters.Add("P_ID", OracleDbType.Int32).Value = id;
        cmd.Parameters.Add("P_TITLE", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_COMPANY", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_DURATION", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_DESCRIPTION", OracleDbType.Varchar2).Value = DBNull.Value;
        cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        cmd.ExecuteNonQuery();

        return Ok(new { status = "deleted" });
    }

}
