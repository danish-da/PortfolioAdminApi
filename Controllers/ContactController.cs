using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using PortfolioAdminApi.Models.Contact;
using PortfolioAdminApi.Services;
using System.Data;

namespace PortfolioAdminApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly OracleDbService _db;

    public ContactController(OracleDbService db)
    {
        _db = db;
    }

    [HttpPost]
    public IActionResult Submit(ContactRequest req)
    {
        using var conn = _db.GetConnection();
        conn.Open();

        using var cmd = new OracleCommand("SP_CONTACT_PORTFOLIO_DAN", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.Add("P_NAME", OracleDbType.Varchar2).Value = req.Name;
        cmd.Parameters.Add("P_EMAIL", OracleDbType.Varchar2).Value = req.Email;
        cmd.Parameters.Add("P_PHONE", OracleDbType.Varchar2).Value = req.Phone;
        cmd.Parameters.Add("P_COMMENTS", OracleDbType.Varchar2).Value = req.Comments;
        cmd.Parameters.Add("P_CURSOR", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

        cmd.ExecuteNonQuery();

        return Ok(new { status = "submitted" });
    }
}
